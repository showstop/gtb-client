using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Yippee.Net {
	public class AsyncResultConnect : AsyncResult
	{
		private IOEngine engine_;

		public AsyncResultConnect(Socket handle, IOEngine engine)
			: base(handle)
		{
			engine_ = engine;
		}

		public override void Process(uint numberOfBytesTransferred)
		{
			Link link = new Link(Handle, engine_);
			link.OnLinkOpened();
		}
	}
	
	public class AsyncResultDisconnect : AsyncResult
	{
		private Link link_;

		public AsyncResultDisconnect(Socket handle, Link link)
			: base(handle)
		{
			link_ = link;
		}

		public override void Process(uint numberOfBytesTransferred)
		{
			link_.OnLinkClosed();
		}
	}
	
	class AsyncResultRecv : AsyncResult
	{
		private byte[] buffer_;
		private ulong ioSeq_;
		private Link link_;

		public AsyncResultRecv(Socket handle, byte[] buffer, ulong ioSeq, Link link)
			: base(handle)
		{
				buffer_ = buffer;
				ioSeq_ = ioSeq;
				link_ = link;
		}

		public override void Process(uint numberOfBytesTransferred)
		{
			link_.OnReceiveCompleted(ioSeq_, buffer_, numberOfBytesTransferred);
		}
	}
	
	class AsyncResultSend : AsyncResult
	{
		private Link link_;

		public AsyncResultSend(Socket handle, Link link)
			: base(handle)
		{
			link_ = link;
		}

		public override void Process(uint numberOfBytesTransferred)
		{
			link_.OnSendCompleted(numberOfBytesTransferred);
		}
	}
}