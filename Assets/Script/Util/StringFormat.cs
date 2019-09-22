using UnityEngine;
using System.Collections;

public class StringFormat : MonoBehaviour 
{
	internal static string CurrentWithMax(int aCurrent, int aMax)
    {
        return string.Format("{0} / {1}", aCurrent, aMax);
    }

    internal static string NumberWithComma(int aNumber)
    {
        return string.Format("{0:#,##0}", aNumber);
    }

    internal static string NumberWithComma(ulong aNumber)
    {
        return string.Format("{0:#,##0}", aNumber);
    }

    internal static string MinuteSecond(long aSeconds)
    {
        long minute = aSeconds / 60;
        long second = aSeconds % 60;

        return string.Format("{0}:{1}", minute.ToString("00"), second.ToString("00"));
    }

    internal static string HourMinuteSecond(long aSeconds)
    {
        long hour = aSeconds / 3600;        
        long minute = (aSeconds % 3600) / 60;
        long second = (aSeconds % 3600) % 60;

        return string.Format("{0}:{1}:{2}", hour.ToString("00"), minute.ToString("00"), second.ToString("00"));
    }

    internal static string CountWithX(int aCount)
    {
        return string.Format("x {0}", aCount);
    }

    internal static string LevelString(int aLevel)
    {
        return string.Format("Lv.{0}", aLevel);
    }

    internal static string GPPointString(int aPoint)
    {
        return string.Format("{0} Point", NumberWithComma(aPoint));
    }

    internal static string MapSceneName(int aMapNo)
    {
        return string.Format("scene_map_{0}", aMapNo);
    }
}