using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;

namespace Yippee.Net {

	public class WaveConnector : IOHandler
	{
		private Connector connector_ = new Connector();
		public PacketHandler packetHandler_ = new PacketHandler();
		private AutoResetEvent connectionEvent_ = new AutoResetEvent(false);
		private WaveLink link_;

		private const int RECV_PENDING_COUNT = 1;
		private const int RECV_BUFFER_SIZE = 1024 * 2;

        public enum ConnectionState
        {
            OFFLINE = 0,
            ONLINE,
            DISCONNECTED_BY_SERVER,
        }
        public ConnectionState connectionState_;
		
		public WaveConnector()
		{
			connector_.SetHandler(this);
            connectionState_ = ConnectionState.OFFLINE;
		}
		
		public bool Connect(String domain, int port, int timeout)
		{
            try
			{
				if (!connector_.Connect(domain, port, timeout))
				{
					throw new System.Exception();
				}

				if (!connectionEvent_.WaitOne(timeout))
				{
					throw new System.Exception();
				}
			}
			catch (System.Exception)
			{
				Close();
				return false;
			}

			return true;
		}

		~WaveConnector()
		{
			Close();
		}

		public bool Connected
		{
			get
			{
				return connector_.Connected;
			}
		}

		public void Close()
		{
			connector_.Close();
			
			if(link_ != null)
				link_.SetSessionKey(null, null);
		}
		
		internal void SetSessionKey(byte[] key, byte[] iv)
		{
			link_.SetSessionKey(key, iv);	
		}

		public WaveLink WaveLink
		{
			get
			{
				return link_;
			}
		}

		public bool AddDispatcher(uint key, DispatchFunction func)
		{
			return packetHandler_.AddDispatcher(key, func);
		}

		internal override void OnOpened(Link link)
		{
			// Call recv function to request asynchronous data receive.
			link.Recv(RECV_BUFFER_SIZE);

			link_ = new WaveLink();
			link_.SetLink(link);

			connectionEvent_.Set();

            connectionState_ = ConnectionState.ONLINE;
		}

		internal override void OnClosed(Link link)
		{
            connector_.Close();

            if (link_ != null)
                link_.SetSessionKey(null, null);
            connectionState_ = ConnectionState.DISCONNECTED_BY_SERVER;
		}

		internal override void OnReceived(Link link)
		{
			link.Recv(RECV_BUFFER_SIZE);

			packetHandler_.ProcessReceiveStream((WaveLink)link.Data);
		}
	}
}
