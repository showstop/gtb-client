using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Yippee.Net {

	public delegate void DispatchFunction(WaveLink link, Stream stream);

	public class Dispatcher
	{
		private Dictionary<uint, DispatchFunction> functors_ = new Dictionary<uint, DispatchFunction>();

		public bool Add(uint id, DispatchFunction func)
		{
			if (functors_.ContainsKey(id))
			{
				return false;
			}

			functors_[id] = func;

			return true;
		}

		public bool Dispatch(uint id, WaveLink link, Stream stream)
		{
			if (!functors_.ContainsKey(id))
			{
				return false;
			}

			functors_[id](link, stream);
			return true;
		}
	}
}
