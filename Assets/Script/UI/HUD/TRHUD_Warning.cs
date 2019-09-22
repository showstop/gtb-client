using UnityEngine;
using System.Collections;

public class TRHUD_Warning : MonoBehaviour 
{
    [SerializeField]
    private DangerAlarmManager _fourLane;

    [SerializeField]
    private DangerAlarmManager _fiveLane;

    private DangerAlarmManager _activated = null;

	[SerializeField]
	private TRHUDUnit_Warning[] m_warnings_4 = new TRHUDUnit_Warning[4];

    [SerializeField]
    private TRHUDUnit_Warning[] m_warnings_5 = new TRHUDUnit_Warning[5];

    private int m_maxLine = 4;

    internal void Activate(int aLaneCount)
    {
        if (4 == aLaneCount)
        {
            _activated = _fourLane;
        }
        else if (5 == aLaneCount)
        {
            _activated = _fourLane;
        }
        else
        {
            YPLog.LogError("check spline lane count[" + aLaneCount + "]!!!");
            return;
        }

        _activated.gameObject.SetActive(true);
    }

    internal void AddDangerItem(int aLaneNO, Item aItem)
    {
        _activated.AddItem(aLaneNO, aItem);
    }

    internal void RemoveDangerItem(int aLaneNO, Item aItem)
    {
        _activated.RemoveItem(aLaneNO, aItem);
    }

    internal void UpdateWarning(int aLineIndex, int aUpdateCount)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (4 == m_maxLine)
        {
            UpdateWarning_4(aLineIndex, aUpdateCount);
        }
        else if (5 == m_maxLine)
        {
            UpdateWarning_5(aLineIndex, aUpdateCount);
        }
    }

    private void UpdateWarning_4(int aLineIndex, int aUpdateCount)
    {
        if (0 > aLineIndex || m_warnings_4.Length <= aLineIndex)
        {
            return;
        }

        if (null == m_warnings_4[aLineIndex])
        {
            return;
        }

        m_warnings_4[aLineIndex].UpdateWarning(aUpdateCount);
    }

    private void UpdateWarning_5(int aLineIndex, int aUpdateCount)
    {
        if (0 > aLineIndex || m_warnings_5.Length <= aLineIndex)
        {
            return;
        }

        if (null == m_warnings_5[aLineIndex])
        {
            return;
        }

        m_warnings_5[aLineIndex].UpdateWarning(aUpdateCount);
    }
}