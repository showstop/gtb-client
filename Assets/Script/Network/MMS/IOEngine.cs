using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Yippee.Net {

	public delegate void EventCallback(Link link, Object context);
	public class EventHandler
	{
		private EventCallback callback_;
		private Object context_;

		public EventCallback Callback
		{
			get
			{
				return callback_;
			}
		}

		public Object Context
		{
			get
			{
				return context_;
			}
		}

		public EventHandler(EventCallback callback, Object context)
		{
			callback_ = callback;
			context_ = context;
		}
	}

	public class IOEngine : AsyncIOEngine
	{
		private RWThreadSafeObject<List<Link>> activeLinks_ = new RWThreadSafeObject<List<Link>>();
		private bool destroying_ = false;
		private ActiveLinkHandles activeLinkHandles_ = new ActiveLinkHandles();
		private IOHandler handler_;

		~IOEngine()
		{
			Close();			
		}

		public void Close()
		{
			CloseActiveLinks();
			activeLinkHandles_.Wait();
			destroying_ = false;
		}

		public void SetHandler(IOHandler handler)
		{
			handler_ = handler;
		}

		public void AddActiveLinkHandle(Socket handle)
		{
			activeLinkHandles_.Add(handle);
		}

		public void RemoveActiveLinkHandle(Socket handle)
		{
			activeLinkHandles_.Remove(handle);
		}

		public void DoOnConnected(Link link)
		{
			List<Link> activeLinkList = activeLinks_.WriteLock();
			try
			{
				activeLinkList.Add(link);
				if (destroying_)
				{
					link.Close();
				}
			}
			catch (System.Exception ex)
			{
				activeLinks_.WriteUnlock();
				throw ex;
			}

			activeLinks_.WriteUnlock();

			if (handler_ != null)
			{
				handler_.OnOpened(link);
			}
		}

		public void DoOnReceived(Link link)
		{
			if (handler_ != null)
			{
				handler_.OnReceived(link);
			}
		}

		public void DoOnDisconnected(Link link)
		{
			List<Link> activeLinkList = activeLinks_.WriteLock();
			try
			{
				activeLinkList.Remove(link);
				if (destroying_)
				{
					link.Close();
				}
			}
			catch (System.Exception ex)
			{
				activeLinks_.WriteUnlock();
				throw ex;
			}

			activeLinks_.WriteUnlock();

			if (handler_ != null)
			{
				handler_.OnClosed(link);
			}

			link = null;
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
		}

		private bool CloseActiveLinks()
		{
			List<Link> activeLinkList = activeLinks_.ReadLock();
			try
			{
				if (0 < activeLinkList.Count)
				{
					destroying_ = true;
					for (int i = activeLinkList.Count - 1; i >= 0; --i)
					{
						activeLinkList[i].Close();
					}
				}
				else
				{
					activeLinkHandles_.Set();	
				}
			}
			catch (System.Exception ex)
			{
				activeLinks_.ReadUnlock();
				throw ex;
			}

			activeLinks_.ReadUnlock();

			return true;
		}
	}
}