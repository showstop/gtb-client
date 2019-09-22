using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace Yippee.Net {
	
	public class PacketHandler
	{
		public class MessageInfo
		{
			public uint msgid_;
			public Yippee.Net.WaveLink link_;
			public MemoryStream stream_;
		}
		private List<MessageInfo> msgQ_ = new List<MessageInfo>();
		
		private Yippee.Net.Dispatcher dispatcher_ = new Yippee.Net.Dispatcher();
	
		public void Update()
		{
			lock(msgQ_)
			{
				foreach(var m in msgQ_)
				{
					try 
					{
						if (!dispatcher_.Dispatch(m.msgid_, m.link_, m.stream_))
						{
							Debug.Log (string.Format("[PacketHandler::Update()] dispatch failed.{0}", m.msgid_));
							throw new System.Exception();
						}
					}
					catch (SystemException e)
					{
						Debug.Log (string.Format("[PacketHandler::Update()] check protocol handler..Message:{0}, StackTrack:{1}", 
							e.Message, e.StackTrace));

                        continue;
					}
					finally
					{
						m.link_.UnlockReadStream();
					}
				}
				msgQ_.Clear();
			}
		}
		
		public void ProcessReceiveStream(Yippee.Net.WaveLink link)
		{
			try
			{
				MemoryStream stream = link.LockReadStream();
				
				for (; ; )
				{
					uint id = 0;
                    int sessionID = 0;
					byte[] messageBlockStream = null;
					long rollBackPos;
					if (!ExtractMessage(ref link.cipher_, stream, out id, out sessionID, out messageBlockStream, out rollBackPos))
					{
						break;
					}
                    link.SetSessionID(sessionID);
					lock (msgQ_)
					{
						MessageInfo mi = new MessageInfo();
						mi.msgid_ = id;
						mi.link_ = link;
						mi.stream_ = new MemoryStream(messageBlockStream);
						msgQ_.Add(mi);
					}
					
				}
			}
			catch(System.Exception)
			{     
				//link.Close();
			}
			finally
			{
				link.UnlockReadStream();
			}
		}

		public bool AddDispatcher(uint key, Yippee.Net.DispatchFunction func)
		{
			return dispatcher_.Add(key, func);
		}
		
		public bool ExtractMessage(ref AES cipher, Stream stream, out uint id, out int sessionID, out byte[] messageBlockStream, out long rollBackPos)
		{
			id = 0;
            sessionID = 0;
			messageBlockStream = null;

			rollBackPos = stream.Position;
			
            if (stream.Length - stream.Position < Consts.HEADER_SIZE)
                return false;

            BinaryReader reader = new BinaryReader(stream);
			try
			{
                if (Consts.PROTOCOL_SEPERATOR_ID != reader.ReadByte())    // seperator id
                {
                    throw new System.Exception();
                }
				byte flag = reader.ReadByte();	// flag

                sessionID = reader.ReadInt32();
                id = reader.ReadUInt32();
                int length = reader.ReadInt32();

                stream.Seek(rollBackPos, SeekOrigin.Begin);
				if (stream.Length - stream.Position < length)
				{
					throw new System.Exception();
				}

                reader.ReadBytes(Consts.HEADER_SIZE);   // offset recovery.
				if(cipher != null && (length - Consts.HEADER_SIZE) > 0)
				{
					try {
						messageBlockStream = cipher.decryptBytes(reader.ReadBytes((int)length - Consts.HEADER_SIZE));
					}
					catch(SystemException e)
					{
						Debug.LogError(e.ToString());
					}
				}
				else
				{
                	messageBlockStream = reader.ReadBytes((int)length - Consts.HEADER_SIZE);
				}
			}
			catch (System.Exception)
			{
                stream.Flush();
				/*stream.Seek(rollBackPos, SeekOrigin.Begin);
				byte[] remainingData = reader.ReadBytes((int)(stream.Length - rollBackPos));
				stream.Seek(0, SeekOrigin.Begin);
				BinaryWriter writer = new BinaryWriter(stream);
				writer.Write(remainingData);
				stream.Seek(0, SeekOrigin.Begin);
				stream.SetLength(remainingData.Length);
				MemoryStream memoryStream = stream as MemoryStream;
				if (memoryStream != null)
				{
					memoryStream.Capacity = remainingData.Length;
				}*/
				return false;
			}

			return true;
		}
	}
}
