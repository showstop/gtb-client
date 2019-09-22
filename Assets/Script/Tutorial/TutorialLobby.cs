using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Network;

public class TutorialLobby : GUIComponent
{
    [SerializeField]
    private GameObject _welcomeUser;

    [SerializeField]
    private GameObject _licensePopup;

    [SerializeField]
    private GameObject _skipPopup;

    [SerializeField]
    private GameObject _carSelectPopup;

    [SerializeField]
    private GameObject _noticePopup;

    [SerializeField]
    private GameObject _noticeBG;

    [SerializeField]
    private GameObject _nextButton;

    [SerializeField]
    private GameObject _skipButton;

    [SerializeField]
    private GameObject _blackPanel;

    [SerializeField]
    private GameObject _carInfoCar;

    [SerializeField]
    private GameObject _carInfoCarstat;

    [SerializeField]
    private GameObject _fading;

    [SerializeField]
    private Animator _fadingAni;

    [SerializeField]
    private Text _inputNickName;

    [SerializeField]
    private Text _outputNickName;

    [SerializeField]
    private Text _outputCarName;

    [SerializeField]
    private Text _skipGuideText;

    [SerializeField]
    private Text[] _selectCarName = new Text[3];

    [SerializeField]
    private List<GameObject> _pageList = new List<GameObject>();

    private GameObject _tutorialGameManager;
    public TextSelector _tsNotice;
    public TextSelector _tsWelcomeNotice;
    public TextSelector _tsSkipNotice;
    public TextSelector _tsNextText;

    private Vector3 _nextButtonPos = new Vector3(260.0f, 550.0f, 0);
    private Vector3 _nextButtonCurrentPos = Vector3.zero;

    public override void OnHandleEvent(GameEventType gameEventType, params object[] args)
    {
        switch (gameEventType)
        {
            case GameEventType.TutorialProgressUpdateAnsOK:
                TutorialManager.Instance.TutorialLobbyEnd(false);
                break;
        }
    }

    internal void Init()
    {
        if (PlayerDataRepository.Instance.MyPlayerInfo.Nick != string.Empty 
            && TutorialConstants.IngameState != TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_NULL)
        {
            TutorialConstants.LobbyState = TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_WELCOME;
        }
        else if(PlayerDataRepository.Instance.MyPlayerInfo.Nick != string.Empty 
                && TutorialConstants.IngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_NULL)
        {
            TutorialConstants.LobbyState = TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_CAR_SELECT;
            if (PlayerDataRepository.Instance.SelectCarNo == TutorialConstants.CARID_BUGGY ||
                PlayerDataRepository.Instance.SelectCarNo == TutorialConstants.CARID_MINICOOP ||
                PlayerDataRepository.Instance.SelectCarNo == TutorialConstants.CARID_TURBIN)
            {
                TutorialConstants.LobbyState = TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_MAIN_VIEW;
            }
        }
        else
        { 
            TutorialConstants.LobbyState = TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_START;
        }

        SetToggleShow(TutorialConstants.LobbyState);
        PageListDisable();
    }
   
    internal void SetToggleShow(TutorialConstants.TUTORIAL_LOBBY_STATE aLobbyState)
    {
        AllDisable();
        PageListDisable();
        switch (aLobbyState)
        {
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_START:
                _blackPanel.SetActive(true);
                SetText(aLobbyState);
                _licensePopup.SetActive(true);
                break;

            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_WELCOME:
                _blackPanel.SetActive(true);
                SetText(aLobbyState);
                _welcomeUser.SetActive(true);
                break;

            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_CAR_SELECT:
                _tutorialGameManager = GameObject.Find("TutorialGameManager") as GameObject;
                Destroy(_tutorialGameManager);

                _blackPanel.SetActive(true);
                SetText(aLobbyState);
                _carSelectPopup.SetActive(true);
                break;

            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_MAIN_VIEW:
                SetText(aLobbyState);
                _blackPanel.SetActive(true);
                _noticeBG.SetActive(true);
                _nextButton.SetActive(true);
                _skipButton.SetActive(true);
                break;

            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_TOP_VIEW:
                SetText(aLobbyState);
                PageEnabled(aLobbyState);
                break;

            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_LEFT_ICON:
                SetText(aLobbyState);
                PageEnabled(aLobbyState);
                break;

            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_RIGHT_ICON:
                SetText(aLobbyState);
                PageEnabled(aLobbyState);
                break;

            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_CAR_INFO:
                SetText(aLobbyState);
                PageEnabled(aLobbyState);
                _blackPanel.SetActive(false);
                _carInfoCar.SetActive(true);
                _carInfoCarstat.SetActive(false);
                break;

            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_CAR_INFO_STAT:
                PageEnabled(aLobbyState);
                _carInfoCar.SetActive(false);
                _carInfoCarstat.SetActive(true);
                break;

            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_MATCH_BUTTON:
                SetText(aLobbyState);
                PageEnabled(aLobbyState);
                _blackPanel.SetActive(true);
                break;

            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_PAGE_BUTTON:
                SetText(aLobbyState);
                PageEnabled(aLobbyState);
                break;

            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_END:
                SetText(aLobbyState);
                _noticeBG.SetActive(true);
                break;
        }
    }

    internal void SetText(TutorialConstants.TUTORIAL_LOBBY_STATE aLobbyState)
    {
        switch(aLobbyState)
        {
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_START:
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_WELCOME:
                _tsWelcomeNotice.SetText(1);
                _outputNickName.text = PlayerDataRepository.Instance.MyPlayerInfo.Nick;
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_CAR_SELECT:
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_MAIN_VIEW:
                _tsNotice.SetText(1);
                _tsSkipNotice.SetText(1);
                _tsNextText.SetText(1);
                break; 
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_TOP_VIEW:
                _tsSkipNotice.SetText(1);
                _tsNextText.SetText(1);
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_LEFT_ICON:
                _tsSkipNotice.SetText(1);
                _tsNextText.SetText(1);
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_RIGHT_ICON:
                _tsSkipNotice.SetText(1);
                _tsNextText.SetText(1);
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_CAR_INFO:
                _tsSkipNotice.SetText(1);
                _tsNextText.SetText(1);
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_CAR_INFO_STAT:
                _tsSkipNotice.SetText(1);
                _tsNextText.SetText(1);
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_MATCH_BUTTON:
                _tsSkipNotice.SetText(1);
                _tsNextText.SetText(1);
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_PAGE_BUTTON:
                _tsSkipNotice.SetText(1);
                _tsNextText.SetText(1);
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_END:
                _tsNotice.SetText(2);
                _tsNextText.SetText(2);
                break;
        }
    }

    internal void PageEnabled(TutorialConstants.TUTORIAL_LOBBY_STATE aLobbyState)
    {
        switch(aLobbyState)
        {
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_TOP_VIEW:
                PageListDisable();
                _pageList[0].SetActive(true);
                StartCoroutine(DelayButtonEnable());
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_LEFT_ICON:
                PageListDisable();
                _pageList[1].SetActive(true);
                StartCoroutine(DelayButtonEnable());
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_RIGHT_ICON:
                PageListDisable();
                _pageList[2].SetActive(true);
                StartCoroutine(DelayButtonEnable());
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_CAR_INFO:
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_CAR_INFO_STAT:
                PageListDisable();
                _pageList[3].SetActive(true);
                StartCoroutine(DelayButtonEnable());
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_MATCH_BUTTON:
                PageListDisable();
                _pageList[4].SetActive(true);
                StartCoroutine(DelayButtonEnable());
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_PAGE_BUTTON:
                PageListDisable();
                _pageList[5].SetActive(true);
                StartCoroutine(DelayButtonEnable());
                break;
            case TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_END:
                PageListDisable();
                StartCoroutine(DelayButtonEnable());
                break;
        }
    }

    internal void AllDisable()
    {
        _noticeBG.SetActive(false);
        _welcomeUser.SetActive(false);
        _licensePopup.SetActive(false);
        _skipPopup.SetActive(false);
        _carSelectPopup.SetActive(false);
        _noticePopup.SetActive(false);
        _nextButton.SetActive(false);
        _skipButton.SetActive(false);
    }

    internal void PageListDisable()
    {
        for(int i = 0; i < _pageList.Count; i++)
        {
            _pageList[i].SetActive(false);
        }
    }

    internal IEnumerator DelayButtonEnable()
    {
        float time = 1.0f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        _nextButton.SetActive(true);
        _skipButton.SetActive(true);
        if(TutorialConstants.LobbyState == TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_END)
        {
            _skipButton.SetActive(false);
        }
    }

    public void RegisterButtonClick()
    {
        if(_inputNickName.text == string.Empty)
        {
            return;
        }
        _outputNickName.text = _inputNickName.text;
        TutorialConstants.LobbyState = TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_WELCOME;

        PlayerDataRepository.Instance.MyPlayerInfo.Nick = _inputNickName.text;
        LobbyRequest.Instance.UpdatePlayerNickReq(PlayerDataRepository.Instance.MyPlayerInfo.Nick);
        LobbyRequest.Instance.TutorialProgressUpdateReq(Constants.TutorialProgress.LOBBY_TUTORIAL_BEGIN);

        SetToggleShow(TutorialConstants.LobbyState);
    }
    public void StartButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(TutorialConstants.TUTORIAL_SCENE_INGAME);

        TutorialConstants.LobbyState = TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_CAR_SELECT;
        TutorialConstants.IngameState = TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_START;
        
        LobbyRequest.Instance.TutorialProgressUpdateReq(Constants.TutorialProgress.CONTROL_TUTORIAL_BEGIN);
    }
    public void SkipButtonClick()
    {
        SetText(TutorialConstants.LobbyState);
        _skipPopup.SetActive(true);
    }
    public void SkipOkButtonClick()
    {
        if(TutorialConstants.LobbyState == TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_MAIN_VIEW ||
        TutorialConstants.LobbyState == TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_TOP_VIEW ||
        TutorialConstants.LobbyState == TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_LEFT_ICON ||
        TutorialConstants.LobbyState == TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_RIGHT_ICON ||
        TutorialConstants.LobbyState == TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_CAR_INFO ||
        TutorialConstants.LobbyState == TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_MATCH_BUTTON ||
        TutorialConstants.LobbyState == TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_PAGE_BUTTON)
        {
            TutorialConstants.LobbyState = TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_END;
            
            LobbyRequest.Instance.TutorialProgressUpdateReq(Constants.TutorialProgress.ALL_COMPLETE);
        }
        else
        {
            TutorialConstants.LobbyState = TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_CAR_SELECT;
            TutorialConstants.IngameState = TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_END;
            SetToggleShow(TutorialConstants.LobbyState);
        }
    }
    public void CancelButtonClick()
    {
        _skipPopup.SetActive(false);
        _noticePopup.SetActive(false);
    }
    public void MinicoopButtonClick()
    {
        _noticePopup.SetActive(true);
        _outputCarName.text = _selectCarName[0].text;
        TutorialManager.Instance._firstCarNumber = 1;
    }
    public void BuggyButtonClick()
    {
        _noticePopup.SetActive(true);
        _outputCarName.text = _selectCarName[1].text;
        TutorialManager.Instance._firstCarNumber = 2;
    }
    public void TurbinButtonClick()
    {
        _noticePopup.SetActive(true);
        _outputCarName.text = _selectCarName[2].text;
        TutorialManager.Instance._firstCarNumber = 3;
    }

    public void CarSelectOkButton()
    {
        TutorialConstants.LobbyState = TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_MAIN_VIEW;
        SetToggleShow(TutorialConstants.LobbyState);
        
        LobbyRequest.Instance.FirstCarChoiceReq(TutorialManager.Instance._firstCarNumber);
        LobbyRequest.Instance.TutorialProgressUpdateReq(Constants.TutorialProgress.LOBBY_TUTORIAL_BEGIN);
    }
    
    public void CarSelectCancelButton()
    {
        _noticePopup.SetActive(false);
    }

    public void NextButtonClick()
    {
        if (TutorialConstants.LobbyState == TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_END)
        {
            TutorialConstants.LobbyState = TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_NULL;
            LobbyRequest.Instance.TutorialProgressUpdateReq(Constants.TutorialProgress.ALL_COMPLETE);
            TutorialConstants._tutorialPlaying = false;
            return;
        }
        if (TutorialConstants.LobbyState == TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_MATCH_BUTTON)
        {
            _nextButtonCurrentPos = _nextButton.gameObject.transform.localPosition;
            _nextButton.gameObject.transform.localPosition = _nextButtonPos;
        }
        else if (TutorialConstants.LobbyState == TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_PAGE_BUTTON)
        {
            _nextButton.gameObject.transform.localPosition = _nextButtonCurrentPos;
        }

        int count = (int)TutorialConstants.LobbyState;
        count++;
        TutorialConstants.LobbyState = (TutorialConstants.TUTORIAL_LOBBY_STATE)count;

        SetToggleShow(TutorialConstants.LobbyState);
        PageEnabled(TutorialConstants.LobbyState);
    }
}