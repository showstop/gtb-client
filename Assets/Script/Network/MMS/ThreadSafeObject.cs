using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Reflection;

namespace Yippee.Net {

	class ThreadSafeObject<T>
	{
		private T obj_;

		public ThreadSafeObject()
		{
			obj_ = (T)typeof(T).GetConstructor(Type.EmptyTypes).Invoke(null);
		}

		public ThreadSafeObject(params Object[] args)
		{
			ConstructorInfo[] constructors = typeof(T).GetConstructors();
			foreach (ConstructorInfo constructorInfo in constructors)
			{
				ParameterInfo[] parameterInfos = constructorInfo.GetParameters();
				bool match = true;
				for (int i = 0; i < parameterInfos.Length; ++i)
				{
					if (parameterInfos[i].ParameterType == args[i])
					{
						match = false;
						break;
					}
				}

				if (match)
				{
					obj_ = (T)constructorInfo.Invoke(args);
					break;
				}
			}

			if (null == obj_)
			{
				throw new System.Exception("Cannot find matching constructor.");
			}
		}

		public T Lock()
		{
			Monitor.Enter(this);
			return obj_;
		}

		public void Unlock()
		{
			Monitor.Exit(this);
		}
	}
}