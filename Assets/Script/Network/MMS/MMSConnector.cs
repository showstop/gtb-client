using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;


public partial class MMSConnector : Singleton<MMSConnector>
{
	public int connectionTimeout_ = 2000;

	private MMSHandler message_handler_ = new MMSHandler();
    private Yippee.Net.WaveConnector connector_ = new Yippee.Net.WaveConnector();
	public bool isLogin_ = false;

    private string serverDomain_;
	private int serverPort_ = 10050;

    public bool Connected { get { return connector_.Connected;  } }

	// Use this for initialization
	void Awake()
	{
		RegisterHandler();
		//InvokeRepeating("HeartbeatSend", 2.0f, 30.0f);
		//DontDestroyOnLoad(this);
	}
	
	void Update()
	{
		connector_.packetHandler_.Update();
        if (connector_.connectionState_ == Yippee.Net.WaveConnector.ConnectionState.DISCONNECTED_BY_SERVER)
        {
            message_handler_.OnDisconnected();
            connector_.connectionState_ = Yippee.Net.WaveConnector.ConnectionState.OFFLINE;
        }
	}

	internal void SetMMSAddr(string domain, int port)
	{
        serverDomain_ = domain;
		serverPort_ = port;
	}

	internal bool ConnectToMMS()
	{
		if(false == connector_.Connected) {

			bool ret = connector_.Connect(serverDomain_, serverPort_, connectionTimeout_);
			return ret;
		}
		return true;
	}

	
	private void RegisterHandler()
	{

    }

    internal void Disconnect()
	{
		connector_.Close();
		isLogin_ = false;
	}
	
	public void SetSessionKey(byte[] key, byte[] iv)
	{
        connector_.SetSessionKey(key, iv);
	}

	private bool Send(Yippee.Net.IObjectBase msg)
	{
		try 
		{
			if(false == ConnectToMMS())
				throw new System.Exception();
			
           if(connector_.WaveLink.Send(msg))
				return true;
		}
		catch(System.Exception)
		{
			return false;
		}
		return false;
	}

}
