using UnityEngine;
using System.Collections;

public class PSLobby : MonoBehaviour
{
	void Start ()
    {
        YPLog.PlayServerLog("PSLobby::Start");
        //PSLauncherConnector.Instance.Initialize();
    }
}