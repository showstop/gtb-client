using UnityEngine;
using System.Collections;
using SimpleJSON;

public class TRUnit_GameResultRank : MonoBehaviour 
{
    [SerializeField]
    private UILabel         m_playerID;

    [SerializeField]
    private UILabel         m_playerGearScore;

    [SerializeField]
    private UILabel         m_playerPlayTime;

    [SerializeField]
    private UITexture       m_playerAvatar;
	public Texture PlayerAvatar
	{
		set	{	if ( null != value )	{	m_playerAvatar.mainTexture = value;	}	}
	}

    [SerializeField]
    private UISprite        m_rankSprite;

    [SerializeField]
    private GameObject      m_playerMeBG;

    internal void SetGameResult(string aPlayerUUID, int aGearScore, int aRank, float aPlayTime, bool aMine)
    {   
        m_playerGearScore.text = aGearScore.ToString();
        m_playerPlayTime.text = TRStatic.GetTimeString(aPlayTime);

        if (Constants.BASE_PLAY_TIME != aPlayTime)
        {
            string fileName = string.Format("icon_rank_{0}", aRank);
            m_rankSprite.spriteName = fileName;
            m_rankSprite.gameObject.SetActive(true);
        }

        if (aMine)
            m_playerMeBG.SetActive(true);

        // maybe useless, because image & name is already cached at loading scene.
   //     protocol.integrated_info info = TRDataManager.instance.GetMatchedPlayerInfo(aPlayerUUID);
   //     if (null != info)
   //     {
			//PlayerAvatar = TRPlatformWrapper.Instance.GetPlayerImage(aPlayerUUID, delegate(Texture texture) { PlayerAvatar = texture; }, info.GetInfo().GetPlayerProfileUrl(), info.GetInfo().GetIsProfileOpen());
   //         //m_playerID.text = TRPlatformWrapper.Instance.GetPlayerName(aPlayerUUID, delegate(string buffer) { m_playerID.text = buffer; }, info.GetInfo().GetPlayerNickname());
			//m_playerID.text = info.GetInfo().GetPlayerNickname();
   //     }
   //     else
   //     {
   //         PlayerAvatar = TRPlatformWrapper.Instance.GetPlayerImage(aPlayerUUID, delegate(Texture texture) { PlayerAvatar = texture; }, Constants.EMPTY_STRING, true);
   //         m_playerID.text = TRPlatformWrapper.Instance.GetPlayerName(aPlayerUUID, delegate(string buffer) { m_playerID.text = buffer; }, Constants.EMPTY_STRING);
   //     }
    }
}
