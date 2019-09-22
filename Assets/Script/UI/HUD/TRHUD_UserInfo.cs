using UnityEngine;
using System.Collections;
using SimpleJSON;

public class TRHUD_UserInfo : MonoBehaviour 
{
    [SerializeField]
    private UITexture m_avatar;
	public Texture Avatar
	{
		set	{	if ( null != value )	{	m_avatar.mainTexture = value;	}	}
	}

    [SerializeField]
    private UILabel m_ID;

    [SerializeField]
    private UILabel m_gearScore;

    [SerializeField]
    private UISprite m_ready;

    public void UpdateUserInfo(string aPlayerUUID, int aGearScore, bool aConnected)
    {
        YPLog.Log("UpdateUserInfo, aPlayerUUID = " + aPlayerUUID + ", aGearScore = " + aGearScore + ", aConnected = " + aConnected);
        gameObject.SetActive(true);
        
        m_gearScore.text = aGearScore.ToString();
        if (aConnected)
            SetReady();

        // maybe useless, because image & name is already cached at loading scene.
   //     protocol.integrated_info info = TRDataManager.instance.GetMatchedPlayerInfo(aPlayerUUID);
   //     if (null == info)
   //     {
   //         Avatar = TRPlatformWrapper.Instance.GetPlayerImage(aPlayerUUID, delegate(Texture texture) { Avatar = texture; }, info.GetInfo().GetPlayerProfileUrl());
   //         m_ID.text = TRPlatformWrapper.Instance.GetPlayerName(aPlayerUUID, delegate(string buffer) { m_ID.text = buffer; }, Constants.EMPTY_STRING);            
			
   //     }
   //     else
   //     {
			//Avatar = TRPlatformWrapper.Instance.GetPlayerImage(aPlayerUUID, delegate(Texture texture) { Avatar = texture; }, Constants.EMPTY_STRING, info.GetInfo().GetIsProfileOpen());
   //         m_ID.text = TRPlatformWrapper.Instance.GetPlayerName(aPlayerUUID, delegate(string buffer) { m_ID.text = buffer; }, Constants.EMPTY_STRING);
   //     }
    }

    public void SetReady()
    {
        m_ready.gameObject.SetActive(true);
    }
}
