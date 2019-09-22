using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Yippee.Net;

public class MMSHandler
{
    internal void OnDisconnected()
    {
        //EventManager.Instance.SendGameEvent(GameEventType.LSSessionExpired);
    }

    internal void OnSessionExpiredNotify(WaveLink link, Stream stream)
    {
        // disconnected from lobby server.
        // add several codes for message popup
        //EventManager.Instance.SendGameEvent(GameEventType.LSSessionExpired);
    }

   

}