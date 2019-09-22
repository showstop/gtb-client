using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TRHUD : MonoBehaviour 
{
    [SerializeField]
    private TRPanel_UserInfo m_userInfoPanel = null;

    [SerializeField]
    private TRPanel_UseItem m_useItemPanel = null;

    [SerializeField]
    private TRPanel_HUDEffect m_hudEffectPanel = null;

    [SerializeField]
    private TRPanel_EndGame m_pausePopup = null;

    [SerializeField]
    private TRPanel_GameResult m_gameResultPopup = null;

    [SerializeField]
    private UIButton                m_leftButton            = null;

    [SerializeField]
    private UIButton                m_rightButton           = null;    

    public GameObject[]             m_HUDEffects;
    private bool                    m_showFinalLap          = false;

    [SerializeField]
    private TRHUD_Navigator _navigator;

    [SerializeField]
    private TRPanel_PlayerStatus _playerStatusPanel;

    [SerializeField]
    private TRHUD_Abilities _ability;

    [SerializeField]
    private TRHUD_Warning _warning = null;
    private bool _showWarningHUD = false;

    //[SerializeField]
    //private HUDChangeCar[] _changeCar = new HUDChangeCar[3];
    //private int _activeCarID;
    //[SerializeField]
    //private float _changeCarCooltime;

    private CarController           _playerCar;

	// Use this for initialization
	void Start () 
    {
        /*
        InitSummary(null, TRStatic.GetGameManager().m_goalLapCount);

        TRStatic.GetGameManager().SetupHUD();
        if (TRStatic.GetGameManager().IsLocalGame && 0 == TRDataManager.instance.MatchedPlayerNum)
            HideUserInfo();
        */ 

        ToggleHUD(false);
        //InitializeCamera();
	}

    void InitializeCamera()
    {
        int cameraIndex = TRGlobalData.instance.CameraIndex;
        switch (cameraIndex)
        {
            case 0:
                {
                    if ( null != m_pausePopup ) m_pausePopup.SelectCamera(0);
                }
                break;
            case 1:
                {
                    if (null != m_pausePopup) m_pausePopup.SelectCamera(1);
                }
                break;
            case 2:
                {
                    if (null != m_pausePopup) m_pausePopup.SelectCamera(2);
                }
                break;
        }
    }

#if (UNITY_STANDALONE_WIN || UNITY_EDITOR)    
	void Update () 
    {
        ProcessKey();
	}

    void ProcessKey()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            OnRightButtonPress();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            OnLeftButtonPress();
        }
    }
#endif

    public void SetupHUD(CarController aPlayer)
    {
        //UpdateUserInfo(aPlayer.LaneNO - 1, aPlayer.PlayerUUID, aPlayer.GearScore, aPlayer.Connected);
        //SetupPlayerIndicator(aPlayer);
        //SetupPlayerStatus(aPlayer);
        //if (aPlayer.Mine)
        //{
        //    //SetPlayerMine(aPlayer.PlayerUUID);
        //    //aPlayer.SetHUDAbilities();
        //    //ShowWarningHUD(aPlayer.ReceiveDangerAlarm);
        //}
    }
	
    public void OnLeftButtonPress()
    {   
        //_playerCar.MoveLane(true);
    }

    public void OnRightButtonPress()
    {
        //_playerCar.MoveLane(false);
    }

    public void UpdateCountDown(int aCountDown)
    {
        m_hudEffectPanel.ShowCountDown(aCountDown);

        //if (3 >= aCountDown)
        //{
        //    TRCarController myCar = TRStatic.GetGameManager().GetMyCarController();
        //    if (null != myCar)
        //    {
        //        myCar.EffectSound.PlayCountDownSound(false);
        //    }
        //}
    }

    /*
    public void UpdateTimer(float time)
    {
        m_information.CurrentTime(time);
    }
    */ 
	
	/** 아이템 아이콘이 갱신되었을때 호출됨 */
    public void UpdateItemIcon(int aKeyItemID, int[] aItemKeys)
    {
        if (null != m_useItemPanel)
            m_useItemPanel.SetItem(aKeyItemID, aItemKeys);
    }

    public void StolenItem()
    {
        if (null != m_useItemPanel)
            m_useItemPanel.StolenItem();
    }

    internal void UpdateJamming(bool aApply)
    {
        if (null == m_useItemPanel)
        {
            return;
        }

        YPLog.Trace();
        YPLog.Log("apply = " + aApply);
        m_useItemPanel.UpdateJamming(aApply);
    }

    public void SetUseItemAuto(bool aAuto)
    {
        m_useItemPanel.UseItemAuto = aAuto;
    }

    void ShowCountDownEffect()
    {
        SpawnHUDEffect(0);
    }

    public void ShowFinalLapEffect()
    {
        if (m_showFinalLap)
            return;

        SpawnHUDEffect(1);
        m_showFinalLap = true;
    }

    public void EndGame(int aPlace)
    {
        _playerStatusPanel.ClosePanel();
        SpawnHUDEffect(aPlace + 1);
        Invoke("OpenGameResult", 2f);
    }

    void SpawnHUDEffect(int index)
    {
        if (index < 0 || index >= m_HUDEffects.Length)
            return;

        GameObject go = Instantiate(m_HUDEffects[index], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        GameObject cameraTarget = GameObject.Find(Constants.HUD_CENTER_EFFECT_GO_NAME);
        go.layer = cameraTarget.layer;
        go.transform.parent = cameraTarget.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.Euler(new Vector3(180f, 0, 0));
    }

    public void OpenGameResult()
    {
        ToggleHUD(false);
        m_gameResultPopup.OpenPanel();
    }

    public void ToggleHUD(bool aShow)
    {
        YPLog.Trace();
        YPLog.Log("show = " + aShow);

        m_leftButton.gameObject.SetActive(aShow);
        m_rightButton.gameObject.SetActive(aShow);
        m_useItemPanel.gameObject.SetActive(aShow);

        _navigator.gameObject.SetActive(aShow);
        _ability.gameObject.SetActive(aShow);

        if (_showWarningHUD)
        {
            _warning.gameObject.SetActive(aShow);
        }
        else
        {
            _warning.gameObject.SetActive(false);
        }
    }

    internal void UpdateWarning(int aLineIndex, int aUpdateCount)
    {
        //if (null == _warning)
        //    return;

        _warning.UpdateWarning(aLineIndex, aUpdateCount);
    }

    internal void AddDangerItem(int aLaneNO, Item aItem)
    {
        _warning.AddDangerItem(aLaneNO, aItem);
    }

    internal void RemoveDangerItem(int aLaneNO, Item aItem)
    {
        _warning.RemoveDangerItem(aLaneNO, aItem);
    }

    internal void ShowWarningHUD(bool aShow, int aLaneCount)
    {
        YPLog.Log("TRHUD::ShowWarningHUD, aShow = " + aShow);

        _showWarningHUD = aShow;
        _warning.Activate(aLaneCount);
        _warning.gameObject.SetActive(aShow);
    }

#region User Info
    public void UpdateUserInfo(int aUserIndex, string aPlayerUUID, int aGearScore, bool aConnected)
    {
        m_userInfoPanel.UpdateUserInfo(aUserIndex, aPlayerUUID, aGearScore, aConnected);
    }

    public void SetReady(int aUserIndex)
    {
        m_userInfoPanel.SetReady(aUserIndex);
    }

    public void HideUserInfo()
    {
        m_userInfoPanel.ClosePanel();
    }
#endregion

#region Navigator
    //public void InitSummary(CurvySpline aSpline, int aGoalLapCount)
    //{
    //    _navigator.InitSummary(aSpline, aGoalLapCount);
    //}

    //public void SetupPlayerIndicator(TRCarController aPlayer)
    //{
    //    if (null != _navigator)
    //        _navigator.SetupPlayerIndicator(aPlayer);
    //}

    //public void SetPlayerMine(string aPlayerUUID)
    //{
    //    if (null != _navigator)
    //        _navigator.SetPlayerMine(aPlayerUUID);

    //    //_playerStatusPanel.ShowMyCar(aPlayerUUID);
    //}

    //internal void UpdateNavigatorRank(int aRank)    
#endregion

#region Player Status
    //public void SetupPlayerStatus(TRCarController aPlayer)
    //{
    //    _playerStatusPanel.SetupPlayerStatus(aPlayer);
    //}
#endregion

#region Pause Events
	public void OnPauseButtonClick()
	{
		if ( null != m_pausePopup )
			m_pausePopup.OpenPanel();
	}
#endregion    

#region ability
    internal void SetAbilities(List<int> aAbilityIDs)
    {
        _ability.SetAbilities(aAbilityIDs);
    }

    internal void UpdateAbilityGauge(int aAbilityID, float aGauge)
    {
        if (null == _ability)
        {
            YPLog.LogError("TRHUD::_abilities is null. please setup!");
            return;
        }

        _ability.UpdateAbilityGauge(aAbilityID, aGauge);
    }

    internal void AbilityActivated(int aAbilityID)
    {
        if (null == _ability)
        {
            YPLog.LogError("TRHUD::_abilities is null. please setup!");
            return;
        }

        _ability.AbilityActivated(aAbilityID);
    }
#endregion

    internal void RegisterCar(CarController aCar)
    {
        YPLog.Trace();
        YPLog.Log("car = " + aCar);

        _navigator.SetupIndicator(aCar);
        _playerStatusPanel.SetupPlayerStatus(aCar);

        if (aCar.isLocalPlayer)
        {
            _playerCar = aCar;
            m_useItemPanel.SetPlayerCar(aCar);
            SetAbilities(_playerCar.GetCarAbilityID());
            //SetMatchCar(_playerCar.GetCarID());
            ShowWarningHUD(_playerCar._dangerAlarm, _playerCar._swc.Spline.LaneCount);
        }
    }

    internal void UpdateRank(int aRank)
    {
        _navigator.UpdateRank(aRank);
    }

    internal void UpdateLapCount(int aCount)
    {
        _navigator.UpdateLapCount(aCount);
    }

    //public void ChangeCar(int aCarID)
    //{
    //    if (_activeCarID == aCarID)
    //    {
    //        YPLog.Log("same Car!!! current id = " + _activeCarID + ", change id = " + aCarID);
    //        return;
    //    }

    //    _playerCar.CmdChangeCar(aCarID);
    //    for (int index = 0; index < _changeCar.Length; ++index)
    //    {
    //        StartCoroutine(_changeCar[index].Cooltime(_changeCarCooltime));
    //    }
    //}

    //internal void SetMatchCar(List<int> aCarIDs)
    //{
    //    _activeCarID = aCarIDs[0];
    //    for (int index = 0; index < aCarIDs.Count; ++index)
    //    {
    //        _changeCar[index].SetChangeCar(aCarIDs[index]);
    //    }
    //}

    //internal void SetActiveCar(int aCarID)
    //{
    //    _activeCarID = aCarID;
    //}
}