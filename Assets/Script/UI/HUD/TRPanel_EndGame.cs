using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TRPanel_EndGame : TRPanel
{
    [SerializeField]
    private GameObject[] m_cameraSelectList = new GameObject[3];

    [SerializeField]
    private GameObject[] m_cameraNormalList = new GameObject[3];

	void OnEnable()
	{
		TRStatic.GetHUD().ToggleHUD (false);
	}

	void OnDisable()
	{
		TRStatic.GetHUD().ToggleHUD( true );
	}

    /** 카메라 선택하기 */
    public void SelectCamera(int index)
    {
        for (int i = 0; i < 3; ++i)
        {
            NGUITools.SetActiveSelf(m_cameraSelectList[i], false);
            NGUITools.SetActiveSelf(m_cameraNormalList[i], true);
        }

        NGUITools.SetActiveSelf( m_cameraSelectList[index], true );
        NGUITools.SetActiveSelf( m_cameraNormalList[index], false );

        //TRStatic.GetGameManager().CameraMode(index);
    }

#region EVENT
    public void OnExit()
	{
  //      if (Network.NetworkClient.active || (!TRDataManager.instance.MatchedMultiGame && 0 != TRDataManager.instance.MatchedPlayerNum))
  //      {
		//	YPLog.Log ("OnExit");
  //          TRCarController myCar = TRStatic.GetGameManager().GetMyCarController();
  //          if (null != myCar)
  //          {
		//		YPLog.Log ("Exit2");
				
  //              protocol.player_result_info info = new protocol.player_result_info();
  //              info.SetPlayerUuid(myCar.PlayerUUID);
  //              info.SetGiveupGame(true);

  //              TRSConnector.Instance().GameResultInfoReport(info);
  //          }
  //      }

  //      if (Network.NetworkClient.active)
  //          Network.Disconnect();
        
		//if(true == TRDataManager.instance.IsGuestLogin)
		//{
  //          SceneManager.LoadScene(Constants.SCENE_NAME_GAME_TITLE_FOR_KAKAO);
		//}
		//else
		//{
  //          SceneManager.LoadScene(Constants.SCENE_NAME_MAIN_MENU);
		//}
	}
	
	public void OnContinue()
	{
		ClosePanel();
    }

    public void OnCameraSelect1()
    {
        SelectCamera(0);

        TRGlobalData.instance.CameraIndex = 0;
    }

    public void OnCameraSelect2()
    {
        SelectCamera(1);

        TRGlobalData.instance.CameraIndex = 1;
    }

    public void OnCameraSelect3()
    {
        SelectCamera(2);

        TRGlobalData.instance.CameraIndex = 2;
    }
#endregion
}
