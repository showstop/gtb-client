using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine.Networking;

public class TutorialIngame : GUIComponent
{
    [SerializeField]
    private TutorialGameManager _gm;

    [SerializeField]
    private CarController _player;

    [SerializeField]
    private GameObject _noticePopup;

    [SerializeField]
    private GameObject _nextButton;

    [SerializeField]
    private GameObject _basePopup;

    [SerializeField]
    private GameObject _rightObj;

    [SerializeField]
    private GameObject _leftObj;

    [SerializeField]
    private GameObject _hudPopup;

    [SerializeField]
    private GameObject _skipButton;

    [SerializeField]
    private GameObject _skipView;

    [SerializeField]
    private Text _titleText;

    [SerializeField]
    private Text _noticeText;

    [SerializeField]
    private List<GameObject> _pageList = new List<GameObject>();

    [SerializeField]
    private TextSelector _textSelectorTitle;

    [SerializeField]
    private TextSelector _textSelectorNotice;

    [SerializeField]
    private Image[] _background = new Image[2];

    [SerializeField]
    private Toggle _rightFingerToggle;

    [SerializeField]
    private Toggle _leftFingerToggle;

    [SerializeField]
    private Image[] _rightArrow = new Image[4];

    [SerializeField]
    private Image[] _leftArrow = new Image[4];

    private float _delayTime = 3.0f;
    private bool _rightClick = false;
    private bool _leftClick = false;
    private bool _left = false;
    private Vector3 _rightFingerPos  = new Vector3(-200f, 0, 0);
    private Vector3 _leftFingerPos = new Vector3(200f, 0, 0);

    private float _rightTime = 0.35f;
    private float _leftTime = 0.35f;

    void Start()
    {
        _leftObj.SetActive(false);
        _rightObj.SetActive(false);
        _skipButton.SetActive(false);
    }
    void Update()
    {
        FingerMove();
    }

    public override void OnHandleEvent(GameEventType gameEventType, params object[] args)
    {
        switch (gameEventType)
        {
            case GameEventType.TutorialProgressUpdateAnsOK:
                PlayerDataRepository.Instance.TutorialProgress = (Constants.TutorialProgress)args[0];
                TutorialConstants._tutorialAbility = false;
                _gm.GamePause(false);
                UnityEngine.SceneManagement.SceneManager.LoadScene("mainlobby");
                break;
        }
    }

    internal void Init()
    {
        TutorialConstants._tutorialPlaying = true;
        TutorialConstants.IngameState = TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_START;
        GameManagerLoad();
        _gm.GamePause(true);
        StateSetting(TutorialConstants.IngameState);
        _hudPopup.SetActive(false);
        _skipButton.SetActive(true);
    }


    private void GameManagerLoad()
    {
        GameObject obj = GameObject.Find("TutorialGameManager");
        _gm = obj.GetComponent<TutorialGameManager>();

        _player = _gm.GetPlayerCar();
    }

    internal void BasePopup()
    {
        _basePopup.SetActive(true);
    }

    internal void StateSetting(TutorialConstants.TUTORIAL_INGAME_STATE aIngameState)
    {
        if (aIngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_LANEMOVE_RIGHT_FINGER ||
            aIngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_LANEMOVE_LEFT_FINGER ||
            aIngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_ITEM_SET ||
            aIngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_ITEM_FIRE ||
            aIngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_ABILITY_SET ||
            aIngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_ABILITY_SHOT ||
            aIngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_COLLISION_PLAYING)
        {
            _titleText.text = string.Empty;
            _basePopup.SetActive(false);
            _gm.GamePause(false);
            if(aIngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_LANEMOVE_RIGHT_FINGER)
            {
                _rightObj.SetActive(true);
                _leftObj.SetActive(false);
                _left = false;
                StartCoroutine(ArrowEffect());
            }
            else if(aIngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_LANEMOVE_LEFT_FINGER)
            {
                _rightObj.SetActive(false);
                _leftObj.SetActive(true);
                _left = true;
                StartCoroutine(ArrowEffect());
            }
            return;
        }
        else
        {
            if(aIngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_LANEMOVE_LEFT)
            {
                StopCoroutine(ArrowEffect());
            }
            _basePopup.SetActive(true);
            _nextButton.SetActive(true);
            _leftObj.SetActive(false);
            _rightObj.SetActive(false);
        }

        switch (aIngameState)
        {
            case TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_START:
                _textSelectorNotice.SetText(1);
                break;
            case TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_HUD:
                _textSelectorTitle.SetText(2);
                _textSelectorNotice.SetText(2);
                _hudPopup.SetActive(true);
                break;
            case TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_LANEMOVE_RIGHT:
                _textSelectorTitle.SetText(3);
                _textSelectorNotice.SetText(3);
                _hudPopup.SetActive(false);
                break;
            case TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_LANEMOVE_LEFT:
                _textSelectorTitle.SetText(3);
                _textSelectorNotice.SetText(4);
                break;
            case TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_LANEMOVE_END:
                _textSelectorTitle.SetText(3);
                _textSelectorNotice.SetText(5);
                break;
            case TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_ITEM:
                _textSelectorTitle.SetText(4);
                _textSelectorNotice.SetText(6);
                _gm.ItemboxEnable(true);
                break;
            case TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_ITEM_GET:
                _textSelectorTitle.SetText(4);
                _textSelectorNotice.SetText(7);
                CreateTutorialCar();
                TutorialCarSetting(_player._maxSpeed, _player._accelerate, _player._power, _player._hp);
                break;
            case TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_ITEM_END:
                _textSelectorTitle.SetText(4);
                _textSelectorNotice.SetText(8);
                TutorialCarDestory();
                break;
            case TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_ABILITY:
                _textSelectorTitle.SetText(5);
                _textSelectorNotice.SetText(9);
                _gm.ItemboxEnable(true);
                break;
            case TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_ABILITY_GET:
                _textSelectorTitle.SetText(5);
                _textSelectorNotice.SetText(10);
                CreateTutorialCar();
                TutorialCarSetting(_player._maxSpeed, _player._accelerate, _player._power, _player._hp);
                break;
            case TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_ABILITY_END:
                _textSelectorTitle.SetText(5);
                _textSelectorNotice.SetText(11);
                TutorialCarDestory();
                break;
            case TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_COLLISION:
                _textSelectorTitle.SetText(6);
                _textSelectorNotice.SetText(12);
                CreateTutorialCar();
                TutorialCarSetting(0, 0, 30, 100);
                break;
            case TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_COLLISION_END:
                _textSelectorTitle.SetText(6);
                _textSelectorNotice.SetText(13);
                TutorialCarDestory();
                break;
            case TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_END:
                _textSelectorTitle.SetText(7);
                _textSelectorNotice.SetText(14);
                break;
        }
    }

    internal IEnumerator DelayTimeNextButton()
    {
        if (TutorialConstants.IngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_ITEM_GET)
        {
            yield return null;
        }
        while (_delayTime > 0)
        {
            _delayTime -= Time.deltaTime;
            yield return null;
        }

        _delayTime = 0.5f;
        _nextButton.SetActive(true);
    }

    internal IEnumerator ArrowEffect()
    {
        if (!_left)
        {
            for (int i = 0; i < _rightArrow.Length; i++)
            {
                _rightArrow[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            for (int i = 0; i < _leftArrow.Length; i++)
            {
                _leftArrow[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private void FingerMove()
    {
        if(!_left)
        {
            if (!_rightClick)
            {
                _rightFingerToggle.isOn = false;
                _rightTime -= Time.deltaTime;
                if (_rightTime < 0)
                {
                    _rightClick = true;
                }
            }
            else
            {
                _rightFingerToggle.isOn = true;
                _background[0].gameObject.SetActive(false);
                _rightFingerToggle.transform.Translate(Vector3.right * 6f, Space.World);

                if (_rightFingerToggle.transform.localPosition.x > 120)
                {
                    _rightFingerToggle.isOn = false;
                    _background[0].gameObject.SetActive(true);
                }
                if (_rightFingerToggle.transform.localPosition.x > 200)
                {
                    _rightFingerToggle.transform.localPosition = _rightFingerPos;
                    _rightTime = 0.35f;
                    _rightClick = false;
                }
            }
        }
        else
        {
            if (!_leftClick)
            {
                _leftFingerToggle.isOn = false;
                _leftTime -= Time.deltaTime;
                if (_leftTime < 0)
                {
                    _leftClick = true;
                }
            }
            else
            {
                _leftFingerToggle.isOn = true;
                _background[1].gameObject.SetActive(false);
                _leftFingerToggle.transform.Translate(Vector3.left * 6f, Space.World);

                if (_leftFingerToggle.transform.localPosition.x < -120)
                {
                    _leftFingerToggle.isOn = false;
                    _background[1].gameObject.SetActive(true);
                }
                if (_leftFingerToggle.transform.localPosition.x < -200)
                {
                    _leftFingerToggle.transform.localPosition = _leftFingerPos;
                    _leftTime = 0.35f;
                    _leftClick = false;
                }
            }
        }
    }

    public void NextButtonClick()
    {
        if (TutorialConstants.IngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_END)
        {
            TutorialConstants.IngameState = TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_NULL;
            TutorialConstants.LobbyState = TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_CAR_SELECT;
            
            LobbyRequest.Instance.TutorialProgressUpdateReq(Constants.TutorialProgress.CONTROL_TUTORIAL_END);
            return;
        }

        int count = (int)TutorialConstants.IngameState;
        count++;
        _nextButton.SetActive(false);
        TutorialConstants.IngameState = (TutorialConstants.TUTORIAL_INGAME_STATE)count;
        StateSetting(TutorialConstants.IngameState);

        if (TutorialConstants.IngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_LANEMOVE_RIGHT_FINGER ||
            TutorialConstants.IngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_LANEMOVE_LEFT_FINGER ||
            TutorialConstants.IngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_ITEM_SET ||
            TutorialConstants.IngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_ITEM_FIRE ||
            TutorialConstants.IngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_ABILITY_SET ||
            TutorialConstants.IngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_ABILITY_SHOT ||
            TutorialConstants.IngameState == TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_COLLISION_PLAYING)
        {
            return;
        }

        if (TutorialConstants.IngameState != TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_START)
        {
            StartCoroutine(DelayTimeNextButton());
        }
    }

    public void SkipButtonClick()
    {
        _skipView.SetActive(true);
    }

    public void SkipOkButton()
    {
        LobbyRequest.Instance.TutorialProgressUpdateReq(Constants.TutorialProgress.CONTROL_TUTORIAL_END);
        TutorialConstants.IngameState = TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_NULL;
        TutorialConstants.LobbyState = TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_CAR_SELECT;
        _skipView.SetActive(false);
    }

    public void SkipCancleButton()
    {
        _skipView.SetActive(false);
    }

    internal void CreateTutorialCar()
    {
        for (int i = 0; i < _gm._tutorialCars.Length; i++)
        {
            GameObject go = Instantiate(_gm._tutoCar, Vector3.zero, Quaternion.identity) as GameObject;
            TutoCarController aCar = go.GetComponent<TutoCarController>();
            aCar.SetStartLane(i + 1);
            aCar._tutorialCar = true;
            aCar.name = string.Format("TutorialCar_{0}", i);
            aCar._matchStart = true;
            aCar.gameObject.SetActive(true);
            aCar._isAI = true;
            aCar._swc.InitialF = _player._swc.TF + _player._swc.Spline.DistanceToTF(20f);

            _gm._tutorialCars[i] = aCar;
            NetworkServer.Spawn(go);
        }
    }
    internal void TutorialCarSetting(float aMaxspeed, float aAccelerate, float aPower, int aHp)
    {
        for (int i = 0; i < _gm._tutorialCars.Length; i++)
        {
            _gm._tutorialCars[i].SetStat(aMaxspeed, aAccelerate, aPower, aHp);
            _gm._tutorialCars[i]._swc.Speed = aMaxspeed;
            _gm._tutorialCars[i]._speed = aMaxspeed;
        }
    }
    internal void TutorialCarDestory()
    {
        for (int i = 0; i < _gm._tutorialCars.Length; i++)
        {
            NetworkServer.Destroy(_gm._tutorialCars[i].gameObject);
        }
    }
}
