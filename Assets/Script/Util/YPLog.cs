using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class YPLog : MonoBehaviour
{
	public static void Log(object aLog, bool aShowAtReleaseBuild = false)
    {
        if(!Debug.isDebugBuild && !aShowAtReleaseBuild)
        {
            return;
        }

        Debug.Log(Constants.DEBUG_LOG_TAG_NAME + aLog);
    }

    public static void LogWarning(object aLog, bool aShowAtReleaseBuild = false)
    {
        if (!Debug.isDebugBuild && !aShowAtReleaseBuild)
        {
            return;
        }

        Debug.LogWarning(Constants.WARNING_LOG_TAG_NAME + aLog);
    }

    public static void LogError(object aLog, bool aShowAtReleaseBuild = false)
    {
        if (!Debug.isDebugBuild && !aShowAtReleaseBuild)
        {
            return;
        }

        Debug.LogError(Constants.ERROR_LOG_TAG_NAME + aLog);
    }

    internal static void PlayServerLog(object aLog)
    {
        if (!NetworkServer.active)
        {
            return;
        }

        Log(aLog, true);
    }

    internal static void PlayServerLogWarning(object aLog)
    {
        if (!NetworkServer.active)
        {
            return;
        }

        LogWarning(aLog, true);
    }

    internal static void PlayServerLogError(object aLog)
    {
        if (!NetworkServer.active)
        {
            return;
        }

        LogError(aLog, true);
    }

    internal static void Trace()
    {
        if (!Debug.isDebugBuild)
        {
            return;
        }

        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
        System.Diagnostics.StackFrame sf = st.GetFrame(1);
        Debug.Log(sf.GetMethod().DeclaringType.ToString() + "::" + sf.GetMethod().Name);
    }
}