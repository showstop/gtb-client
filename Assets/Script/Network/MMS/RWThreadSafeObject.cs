using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Yippee.Net {

	public class RWThreadSafeObject<T> where T : new()
	{
		private ReaderWriterLock rwLock_;
		private T obj_;

		public RWThreadSafeObject()
		{
			rwLock_ = new ReaderWriterLock();
			obj_ = (T)typeof(T).GetConstructor(Type.EmptyTypes).Invoke(null);
		}

		public T ReadLock()
		{
			rwLock_.AcquireReaderLock(-1);
			return obj_;
		}

		public void ReadUnlock()
		{
			rwLock_.ReleaseReaderLock();
		}

		public T WriteLock()
		{
			rwLock_.AcquireWriterLock(-1);
			return obj_;
		}

		public void WriteUnlock()
		{
			rwLock_.ReleaseWriterLock();
		}
	}
}