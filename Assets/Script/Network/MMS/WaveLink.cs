using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace Yippee.Net
{
	public class WaveLink
	{
		private Link link_;
        private int sessionID_;
		public AES cipher_ = null;

        public void SetLink(Link link)
		{
			link_ = link;
			link_.Data = this;
		}

        public int SessionID
		{
			get
			{
				return sessionID_;
			}
            set
            {
                sessionID_ = SessionID;
            }
		}

        public void SetSessionID(int sessionID)
        {
            sessionID_ = sessionID;
        }
		
		public void SetSessionKey(byte[] key, byte[] iv)
		{
			if(key == null)
			{
				cipher_ = null;	
			}
			else
			{
				cipher_ = new AES(key, iv);
			}
		}
		
		public bool IsSetSessionKey()
		{
			return cipher_ != null;
		}

        public bool Send(IObjectBase msg)
        {
            MemoryStream stream = new MemoryStream();

            BinaryWriter binaryWriter = new BinaryWriter(stream);
			
			MemoryStream payloadStream = new MemoryStream();
            if (!msg.Serialize(payloadStream))
            {
                return false;
            }
			
			int payloadsize = 0;
			byte[] encrypted = null;
			if(cipher_ != null && payloadStream.Length > 0)
			{
				encrypted = cipher_.encryptBytes(payloadStream.ToArray());
				payloadsize = encrypted.Length;
			}
			else
			{
				payloadsize = msg.Size();
			}
			
            int size = payloadsize + Consts.HEADER_SIZE;
			binaryWriter.Write(Consts.PROTOCOL_SEPERATOR_ID);
			binaryWriter.Write((byte)(cipher_ == null ? 0:1));
            binaryWriter.Write(sessionID_);
            binaryWriter.Write(msg.RuntimeTypeInfo().Id);
            binaryWriter.Write(size);
			
			if(cipher_ != null && payloadStream.Length > 0)
			{
				binaryWriter.Write(encrypted);
			}
			else
			{
				binaryWriter.Write(payloadStream.ToArray());	
			}

            return link_.Send(stream.ToArray());
        }

		public MemoryStream LockReadStream()
		{
			return link_.LockReadStream();
		}

		public void UnlockReadStream()
		{
			link_.UnlockReadStream();
		}

		public IPEndPoint GetRemoteEndPoint()
		{
			return link_.GetRemoteEndPoint();
		}

		public virtual void Close()
		{
			link_.Close();
		}
	}
}
