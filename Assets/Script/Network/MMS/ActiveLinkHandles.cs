using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Yippee.Net {
	
	public class ActiveLinkHandles
	{
		private List<Object> handles_ = new List<Object>();
		private AutoResetEvent event_ = new AutoResetEvent(true);

		public ActiveLinkHandles()
		{
		}

		~ActiveLinkHandles()
		{
			lock (this)
			{
				handles_.Clear();
			}
		}

		public void Add(Object handle)
		{
			lock (this)
			{
				if (handles_.Count == 0)
				{
					event_.Reset();
				}

				handles_.Add(handle);
			}
		}

		public void Remove(Object handle)
		{
			lock (this)
			{
				handles_.Remove(handle);

				if (handles_.Count == 0)
				{
					event_.Set();
				}
			}
		}

		public void Wait()
		{
			event_.WaitOne(-1);
		}
		
		public void Set()
		{
			event_.Set();	
		}
	}
}