using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Yippee.Net {
	public class Connector
	{
		private IOEngine engine_ = new IOEngine();
		private bool connected_ = false;
		private AutoResetEvent connectEvent_ = new AutoResetEvent(false);

		public Connector()
		{
		}

		~Connector()
		{
			Close();
		}

		public bool Connected
		{
			get
			{
				return connected_;
			}
		}

		void ConnectTimerCallback(Object stateInfo)
		{
			Console.WriteLine("[Connector] Connection time out");
			connectEvent_.Set();
		}

        void OnConnected(IAsyncResult asyncResult)
        {
            bool connected = true;

            try
            {
                ((Socket)asyncResult.AsyncState).EndConnect(asyncResult);
            }
            catch (System.Exception)
            {
                connected = false;
            }

            connected_ = connected;
            connectEvent_.Set();
        }

        public bool Connect(String domain, int port, int timeout)
		{
#if RELEASE_IPV6_FOR_IOS
            Socket socket = null;

            IPHostEntry hostEntry = Dns.GetHostEntry(domain);
            foreach (var addr in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(addr, port);
                Socket temp = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                temp.Connect(ipe);
                if(temp.Connected)
                {
                    connected_ = true;
                    socket = temp;
                    break;
                }
            }
#else
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.NoDelay = true;
            connectEvent_.Reset();
            Timer connectTimer = new Timer(new TimerCallback(ConnectTimerCallback), null, timeout, Timeout.Infinite);
            using (connectTimer)
            {
                try
                {
                    socket.BeginConnect(domain, port, new AsyncCallback(OnConnected), socket);
                }
                catch (System.Exception)
                {
                    socket.Close();
                    return false;
                }

                connectEvent_.WaitOne();
            }
#endif           
            if (!connected_)
				return false;

			if (!engine_.PostStatus(0, new AsyncResultConnect(socket, engine_)))
			{
				engine_.RemoveActiveLinkHandle(socket);
				socket.Close();
				return false;
			}

			return true;
		}

		public void Close()
		{
			connected_ = false;
			engine_.Close();
		}

		public void SetHandler(IOHandler handler)
		{
			engine_.SetHandler(handler);
		}
	}
}