using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Yippee.Net {
	public abstract class AsyncResult
	{
		private Socket handle_;

		public AsyncResult(Socket handle)
		{
			handle_ = handle;
		}

		public Socket Handle
		{
			get
			{
				return handle_;
			}
		}

		public abstract void Process(uint numberOfBytesTransferred);
	}

	public class AsyncIOEngine
	{
		public AsyncIOEngine()
		{
		}

		public bool PostStatus(uint numberOfBytesTransferred, AsyncResult asyncResult)
		{
			Object[] args = new Object[2];
			args[0] = numberOfBytesTransferred;
			args[1] = asyncResult;

			ThreadPool.QueueUserWorkItem(new WaitCallback(OnStatusPosted), args);
			return true;
		}

		public bool AddReadIO(AsyncResult asyncResult, byte[] data)
		{
			if (asyncResult == null)
			{
				return false;
			}

			if (asyncResult.Handle == null)
			{
				return false;
			}

			if (0 >= data.Length)
			{
				return false;
			}

			try
			{
				asyncResult.Handle.BeginReceive(data, 0, data.Length, 0, new AsyncCallback(OnReceiveCompleted), asyncResult);
			}
			catch (System.Exception)
			{
				return false;
			}

			return true;
		}

		public bool AddWriteIO(AsyncResult asyncResult, byte[] data, uint len)
		{
			if (asyncResult == null)
			{
				return false;
			}

			if (asyncResult.Handle == null)
			{
				return false;
			}

			if (0 >= data.Length)
			{
				return false;
			}

			try
			{
				asyncResult.Handle.BeginSend(data, 0, (int)len, 0, new AsyncCallback(OnSendCompleted), asyncResult);
			}
			catch (System.Exception ex)
			{
				return false;
			}

			return true;
		}

		private static void OnIOCompleted(uint numberOfBytesTransferred, AsyncResult asyncResult)
		{
			try
			{
				asyncResult.Process(numberOfBytesTransferred);
			}
			catch (System.Exception ex)
			{
			}
		}

		private static void OnStatusPosted(Object context)
		{
			Object[] args = (Object[])context;
			OnIOCompleted((uint)args[0], (AsyncResult)args[1]);
		}

		private static void OnReceiveCompleted(IAsyncResult context)
		{
			AsyncResult asyncResult = (AsyncResult)context.AsyncState;
			try
			{
				OnIOCompleted((uint)asyncResult.Handle.EndReceive(context), asyncResult);
				
			}
			catch (System.Exception)
			{
				asyncResult.Process(0);
			}
		}

		private static void OnSendCompleted(IAsyncResult context)
		{
			try
			{
				AsyncResult asyncResult = (AsyncResult)context.AsyncState;
				OnIOCompleted((uint)asyncResult.Handle.EndSend(context), asyncResult);
			}
			catch (System.Exception ex)
			{
			}
		}
	}
}