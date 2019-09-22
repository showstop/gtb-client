using UnityEngine;
using System.Collections;

public class TRPanel_UserInfo : TRPanel
{
    [SerializeField]
    private TRHUD_UserInfo[] m_userInfos = new TRHUD_UserInfo[4];

    public void UpdateUserInfo(int aUserIndex, string aPlayerUUID, int aGearScore, bool aConnected)
    {
        //YPLog.Log("UpdateUserInfo, aUserIndex = " + aUserIndex + ", aPlayerUUID = " + aPlayerUUID + ", aGearScore = " + aGearScore + ", aConnected = " + aConnected);

        if (null == m_userInfos[aUserIndex])
            return;

        m_userInfos[aUserIndex].UpdateUserInfo(aPlayerUUID, aGearScore, aConnected);
    }

    public void SetReady(int aUserIndex)
    {
        if (null == m_userInfos[aUserIndex])
            return;

        m_userInfos[aUserIndex].SetReady();
    }
}
