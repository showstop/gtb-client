using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Yippee.Net {

	public class Link
	{
		private Socket handle_;
		private IOEngine engine_;
		private bool closed_ = false;
		private bool onClosedCalled_ = false;
		private ThreadSafeObject<Queue<Buffer>> writeBuffers_ = new ThreadSafeObject<Queue<Buffer>>();
		private ThreadSafeObject<MemoryStream> readStream_ = new ThreadSafeObject<MemoryStream>();
		private ulong recvSeqCurrent_ = 0;
		private ulong recvSeqCounter_ = 0;
		private SortedDictionary<ulong, byte[]> recvSeqBuffer_ = new SortedDictionary<ulong, byte[]>();
		private object data_;
		private IPEndPoint remoteEndPoint_;
		private IPEndPoint localEndPoint_;

		public Link(Socket handle, IOEngine engine)
		{
			handle_ = handle;
			engine_ = engine;

			if (handle_ == null)
			{
				throw new System.Exception();
			}

			if (engine_ == null)
			{
				throw new System.Exception();
			}

			IPEndPoint remoteEndPoint = (IPEndPoint)handle_.RemoteEndPoint;
			remoteEndPoint_ = new IPEndPoint(remoteEndPoint.Address, remoteEndPoint.Port);

			IPEndPoint localEndPoint = (IPEndPoint)handle_.LocalEndPoint;
			localEndPoint_ = new IPEndPoint(localEndPoint.Address, localEndPoint.Port);

            // handle_.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            handle_.NoDelay = true;

        }

		~Link()
		{
			try
			{
				ClearWriteBuffers();
				ClearReadStream();

				InternalClose();

				DoOnDestroy();
			}
			catch (System.Exception)
			{
			}
		}

		public object Data
		{
			get
			{
				return data_;
			}

			set
			{
				data_ = value;
			}
		}

		public void Close()
		{
			InternalClose();
		}

		public IPEndPoint GetLocalEndPoint()
		{
			return localEndPoint_;
		}

		public IPEndPoint GetRemoteEndPoint()
		{
			return remoteEndPoint_;
		}

		public bool Send(byte[] data)
		{
			if (data == null)
			{
				return false;
			}

			if (data.Length == 0)
			{
				return false;
			}

			DoOnSend((byte[])data.Clone());

			return true;
		}

		public bool Recv(uint len)
		{
			lock (handle_)
			{
				if (closed_)
				{
					return false;
				}

				byte[] buffer = new byte[len];

				AsyncResultRecv asyncResult = new AsyncResultRecv(handle_, buffer, recvSeqCounter_++, this);
				if (!engine_.AddReadIO(asyncResult, buffer))
				{
					engine_.PostStatus(0, asyncResult);
					return false;
				}

				return true;							
			}
		}

		Queue<Buffer> LockWriteQueue()
		{
			return writeBuffers_.Lock();
		}

		void UnlockWriteQueue()
		{
			writeBuffers_.Unlock();
		}

		public MemoryStream LockReadStream()
		{
			return readStream_.Lock();
		}

		public void UnlockReadStream()
		{
			readStream_.Unlock();
		}

		public void DoOnOpened()
		{
			engine_.DoOnConnected(this);
		}

		public void DoOnClosed()
		{
			engine_.RemoveActiveLinkHandle(handle_);
		}

		public void DoOnSend(byte[] data)
		{
			Queue<Buffer> bufferQueue = LockWriteQueue();
			try
			{
				bool doSend = bufferQueue.Count == 0;
				Buffer buffer = new Buffer(data, (uint)data.Length, 0);
				bufferQueue.Enqueue(buffer);

				if (doSend)
				{
					if (!InternalSend(buffer))
					{
						bufferQueue.Dequeue();
					}
				}
			}
			finally
			{
				UnlockWriteQueue();
			}
		}

		public void DoOnReceived(byte[] buffer, uint size)
		{
			try
			{
				MemoryStream stream = LockReadStream();
				long orgPosition = stream.Position;
				stream.Seek(0, SeekOrigin.End);
				stream.Write(buffer, 0, (int)size);
				stream.Seek(orgPosition, SeekOrigin.Begin);
			}
			finally
			{
				UnlockReadStream();
			}

			engine_.DoOnReceived(this);
		}

		public void OnLinkOpened()
		{
			lock (this)
			{
				DoOnOpened();
			}
		}

		public void OnLinkClosed()
		{
			lock (this)
			{
				if (!onClosedCalled_)
				{
					onClosedCalled_ = true;
					engine_.DoOnDisconnected(this);

					DoOnClosed();
				}
			}
		}

		public void OnSendCompleted(uint numberOfBytesTransferred)
		{
			if (0 == numberOfBytesTransferred)
			{
				Close();
				return;
			}

			Queue<Buffer> bufferQueue = LockWriteQueue();
			try
			{
				if (0 != bufferQueue.Count)
				{
					Buffer buffer = bufferQueue.Peek();
					buffer.Consume(numberOfBytesTransferred);
					if (buffer.IsEmpty())
					{
						bufferQueue.Dequeue();
						buffer = null;
					}

					if (0 != bufferQueue.Count)
					{
						buffer = bufferQueue.Peek();
						if (!InternalSend(buffer))
						{
							bufferQueue.Dequeue();
						}
					}
				}
			}
			finally
			{
				UnlockWriteQueue();
			}
		}

		public void OnReceiveCompleted(ulong seq, byte[] buffer, uint numberOfBytesTransferred)
		{
			lock (this)
			{
				if (onClosedCalled_)
				{
					return;
				}

				if (0 == numberOfBytesTransferred)
				{
					Close();
					return;
				}

				if (seq == recvSeqCurrent_)
				{
					++recvSeqCurrent_;

					DoOnReceived(buffer, numberOfBytesTransferred);

					if (0 != recvSeqBuffer_.Count)
					{
						List<ulong> keysToRemove = new List<ulong>();
						foreach (KeyValuePair<ulong, byte[]> pair in recvSeqBuffer_)
						{
							if (pair.Key == recvSeqCurrent_)
							{
								++recvSeqCurrent_;
								keysToRemove.Add(pair.Key);
							}
							else
							{
								break;
							}
						}

						keysToRemove.ForEach(key => recvSeqBuffer_.Remove(key));
					}
				}
				else
				{
					recvSeqBuffer_[seq] = buffer;
				}
			}
		}

		private bool InternalClose()
		{
			lock (handle_)
			{
				if (closed_)
				{
					return false;
				}

				closed_ = true;

				engine_.PostStatus(0, new AsyncResultDisconnect(handle_, this));

				handle_.Close();

				return true;
			}
		}

		private bool InternalSend(Buffer buffer)
		{
			if (buffer == null)
			{
				return false;
			}

			if (buffer.IsEmpty())
			{
				return false;
			}

			lock (handle_)
			{
				if (closed_)
				{
					return false;
				}

				AsyncResultSend asyncResult = new AsyncResultSend(handle_, this);
				if (!engine_.AddWriteIO(asyncResult, buffer.Data, buffer.Len))
				{
					engine_.PostStatus(0, asyncResult);
					return false;
				}
			}

			return true;
		}
		
		private bool ClearWriteBuffers()
		{
			Queue<Buffer> writeBuffers = writeBuffers_.Lock();
			try
			{
				writeBuffers.Clear();
			}
			catch (System.Exception)
			{
				writeBuffers_.Unlock();
				return false;
			}

			writeBuffers_.Unlock();

			return true;
		}

		private bool ClearReadStream()
		{
			MemoryStream readStream = readStream_.Lock();
			try
			{
				readStream.Close();
				recvSeqBuffer_.Clear();
			}
			catch (System.Exception)
			{
				readStream_.Unlock();
				return false;
			}

			readStream_.Unlock();

			return true;
		}

		private void DoOnDestroy()
		{
		}

		public void SetNoDelay(bool noDelay)
		{
			handle_.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, noDelay);
		}
	}
}