using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TRPanel_GameResult : TRPanel 
{
    [SerializeField]
    private TRUnit_GameResultRank[]     m_playerGameResultRanks = new TRUnit_GameResultRank[4];

    [SerializeField]
    private UILabel                     m_exp;

    [SerializeField]
    private UILabel                     m_gameMoney;

    [SerializeField]
    private UISprite                    m_myRank;

    [SerializeField]
    private UILabel                     m_resultDesc;

    [SerializeField]
    private GameObject                  m_breakDownGameMoneyIcon;

    [SerializeField]
    private GameObject                  m_abilityGameMoneyIcon;

    [SerializeField]
    private GameObject                  m_abilityEXPIcon;

    private CarController             m_myCar;

    public float                        m_autoCloseTime = 10.0f;

    void OnEnable()
    {
        //m_myCar = TRStatic.GetGameManager().GetMyCarController();

        SetGameResultRank();
        SetGameReward();
        SetIcons();

        Invoke("AutoClose", m_autoCloseTime);
    }

    void SetGameResultRank()
    {
   //     TRStatic.GetGameManager().ClientSortRank(true);
   //     for (int index = 0; index < m_playerGameResultRanks.Length; ++index)
   //     {
			//TRCarController car = TRStatic.GetGameManager().GetSortedPlayer(index);
   //         m_playerGameResultRanks[index].SetGameResult(car.PlayerUUID, car.GearScore, car.Rank, car.PlayGameTime, car.Mine);            
   //     }

   //     string fileName = string.Format("icon_rank_{0}", m_myCar.Rank);
   //     m_myRank.spriteName = fileName;

   //     string desc = "";
   //     switch (m_myCar.Rank)
   //     {
   //         case 1:
   //         case 2:
   //         case 3:
   //                 desc = Localization.instance.Get("@Medal_" + m_myCar.Rank.ToString()) + " ";
   //                 desc += Localization.instance.Get("@GetMedal");
   //                 break;
   //         case 4:
   //                 desc = Localization.instance.Get("@GetMedalFail");
   //                 break;
   //         default: 
   //                 break;                    
   //     }

   //     m_resultDesc.text = desc;
    }

    void SetGameReward()
    {
        m_exp.text = "1";
        m_gameMoney.text = "1";

        // TO DO : change value, EXP & GM add list up.
        StartCoroutine("ChangeValue");
    }
    
    IEnumerator ChangeValue()
    {
        yield return new WaitForSeconds(0.1f);

        //m_myCar.EffectSound.PlayScoreIncreaseSound();

        //int current = System.Convert.ToInt32(m_gameMoney.text);
        //while (current != m_myCar.GameMoney)
        //{
        //    yield return null;

        //    current += Random.Range(1, TRStatic.GAMEMONEY_BREAK_DOWN * 2);
        //    if (current > m_myCar.GameMoney)
        //        current = m_myCar.GameMoney;

        //    m_gameMoney.text = current.ToString();

        //    int exp = System.Convert.ToInt32(m_exp.text) + Random.Range(1, TRStatic.GAMEMONEY_BREAK_DOWN);
        //    if (exp > m_myCar.EXP || current == m_myCar.GameMoney)
        //        exp = m_myCar.EXP;

        //    m_exp.text = exp.ToString();
        //}

        //m_myCar.EffectSound.StopRunningSound();
    }

    private void SetIcons()
    {
        //if (0 < m_myCar.BreakDownCount)
        //{
        //    m_breakDownGameMoneyIcon.SetActive(true);
        //}

        //int[] abilityIDs = new int[1];
        //if (m_myCar.GetAbilityRelatedWithGameMoney(ref abilityIDs))
        //{
        //    m_abilityGameMoneyIcon.SetActive(true);
        //}

        //if (m_myCar.GetAbilityRelatedWithEXP(ref abilityIDs))
        //{
        //    m_abilityEXPIcon.SetActive(true);
        //}
    }

    private void AutoClose()
    {
        ClosePanel();
        
        UnityEngine.Network.Disconnect();

        System.GC.Collect();
        Resources.UnloadUnusedAssets();	

		//if(true == TRDataManager.instance.IsGuestLogin)
		//{
		//	SceneManager.LoadScene(Constants.SCENE_NAME_GAME_TITLE_FOR_KAKAO);
		//}
		//else
		//{
  //          SceneManager.LoadScene(Constants.SCENE_NAME_MAIN_MENU);
		//}
    }

#region EVENT
    public void OnOKBtnClick()
    {
        CancelInvoke("AutoClose");

        YPLog.Log(this + "OnOKBtnClick()!");

		ClosePanel();

        UnityEngine.Network.Disconnect();

        System.GC.Collect();
        Resources.UnloadUnusedAssets();	

		//if(true == TRDataManager.instance.IsGuestLogin)
		//{
  //          SceneManager.LoadScene(Constants.SCENE_NAME_GAME_TITLE_FOR_KAKAO);
		//}
		//else
		//{
  //          SceneManager.LoadScene(Constants.SCENE_NAME_MAIN_MENU);
		//}
    }

    public void OnBoastBtnClick()
    {
        YPLog.Log(this + "OnBoastBtnClick()!");
    }
#endregion
}
