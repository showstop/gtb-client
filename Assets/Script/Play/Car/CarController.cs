using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SpeedBonus
{
    public float            _currentTime;
    public float            _bonusTime;
    public float            _bonusSpeed;    
};

public struct ApplyAbility
{
    public int _id;
    public int _level;
};

public class ApplyAbilities : SyncListStruct<ApplyAbility>
{
}

public class CarController : NetworkBehaviour
{
    [SyncVar]
    public string _playerNo;

    [SyncVar(hook = "ChangeBody")]
    private int _body;
    [SyncVar(hook = "ChangeTire")]
    private int _tire;
    private bool _bodyCreate = false;
    private bool _tireCreate = false;

    [SerializeField]
    public SplineWalkerCon _swc;

    [SerializeField]
    private CarRenderer _renderer;
    public Transform CarTransform { get { return _renderer.gameObject.transform; } }
    public GameObject ItemTop { get { return _renderer.ItemTop; } }
    public GameObject ItemBack { get { return _renderer.ItemBack; } }
    public Vector3 NamePosition { get { return ItemBack.transform.position + new Vector3(0f, -0.1f, 0f); } }
    public BoxCollider Collider { get { return _renderer._collider; } }

    public GameManager GM { get; set; }
    private TRCamera _camera;
    //private TRHUD _hud = null;
    //public TRHUD _hud = null;
    public HUD _hud = null;

    [SerializeField]
    private CarSound _sound;
    [SerializeField]
    private CarFX _fx;

    public bool _matchStart = false;
    private float _matchStartTime = 0f;

    private SyncListInt _startItem = new SyncListInt();
    private ApplyAbilities _carAbility = new ApplyAbilities();
    private ApplyAbilities _equipAbility = new ApplyAbilities();

    [SyncVar]
    public float _maxSpeed;
    public float _speed;
    [SyncVar]
    public float _accelerate;
    public float _power;
    [SyncVar]
    public int _maxHP;
    [SyncVar(hook = "ChangeHP")]
    public int _hp;
    [SyncVar]
    public float _explosionTime = Constants.EXPLOSION_TIME;

    [SyncVar(hook = "SetStartLane")]
    private int _startLane;
    public int _laneNO;
    private Constants.MoveLaneState _moveLaneState = Constants.MoveLaneState.Stay;
    private float _moveLaneStartVelocity = Constants.MOVE_LANE_START_VELOCITY;
    private float _moveLaneVelocityChangeRatio = Constants.MOVE_LANE_VELOCITY_CHANGE_RATIO;
    private float _moveLaneCurrentVelocity = 0f;
    private float _moveLaneOffset = 0f;
    private float _moveLaneCurrentOffset = 0f;
    public Constants.JumpState _jumpState;
    private float _jumpVelocity = Constants.JUMP_START_VELOCITY;
    private float _jumpVelocityChangeRatio = 6f;
    private float _jumpOffset = 0f;
    private bool _changeRotationDuringJump = false;
    private Quaternion _oldLocalRotation;

    private List<CarController> _collideCars = new List<CarController>();
    private Dictionary<uint, SpeedBonus> _collideSpeedBonus = new Dictionary<uint, SpeedBonus>();
    private List<SpeedBonus> _speedBonus = new List<SpeedBonus>();
    public SpeedBonus _rollingBooster = new SpeedBonus();
    public float _maxRollingBoosterBonusTime;
    public float _IncreaseRollingBoosterBonusTime;
    public ItemRollingBooster _rollingBoosterItem;

    private Dictionary<short, int> _playData = new Dictionary<short, int>();
    public float _moveDistance = 0f;
    private float _playGameTime = 0f;
    [SyncVar]
    public int _rank = 1;
    [SyncVar]
    public int _finalRank = 4;
    public int _gameMoney = 0;
    public int _exp = 0;
    [SyncVar]
    public float _lapTime;

    [SyncVar]
    public int _goalLapCount = 0;
    public int _lapCount = 0;
    private float _lapStartTime = 0f;
    private float _bestLapTime = 0f;

    protected Dictionary<int, Item> _stackItems = new Dictionary<int, Item>();
    private int _getItemCount = 0;
    private int _maxStackItemCount = 1;
    [SyncVar(hook = "UpdateItemIcon")]
    private int _currentItemID;
    private SyncListInt _itemIDs = new SyncListInt();
    public bool CanStackItem { get { return _stackItems.Count < _maxStackItemCount; } }
    public bool _updateItemUseButton;
    private int _useItemID;

    public bool _isAI = false;
    private Constants.MoveLaneState _lastMove = Constants.MoveLaneState.Stay;

    public int _damageShield;
    [SyncVar(hook = "ChargeShield")]
    public bool _chargeShield;
    private GameObject _chargeShieldFX;
    [SyncVar(hook = "StrongWillShield")]
    public int _strongWillShield = 0;
    public bool _speedingShield = false;

    [SyncVar]
    public bool _dangerAlarm = false;

    [SyncVar]
    public bool _runawayActivated = false;
    private float _runawayBonusSpeedTime;

    [SyncVar(hook = "Jamming")]
    public bool _jamming = false;

    [SyncVar]
    public bool _rewindActivated = false;
    [SyncVar]
    public float _rewindPercentage;

    [SyncVar(hook = "OneWay")]
    public bool _oneWay = false;

    public bool _sirenBoost = false;

    //Touth Silde Move
    private Vector2 _touchPos;
    private float _distanceX = 100f;
    private float _startPos = 0;

    void Start()
    {
        YPLog.Trace();
        YPLog.Log("playerNo = " + _playerNo + ", _startLane = " + _startLane + ", body = " + _body + ", _tire = " + _tire, true);
        YPLog.Log("server = " + NetworkServer.active + ", client = " + NetworkClient.active + ", host = " + NetworkServer.localClientActive + ", localPlayer = " + isLocalPlayer, true);

        _rollingBooster._currentTime = -1f;
        if (isLocalPlayer)
        {
            _startItem.Callback = StartItemChanged;
            _carAbility.Callback = CarAbilityChanged;
            _equipAbility.Callback = EquipAbilityChanged;
        }
        else
        {
            SetStartLane(_startLane);   
                    
            ChangeBody(_body);
            ChangeTire(_tire);
        }

        if (NetworkClient.active)
        {
            StartCoroutine(RegisterCarToHUD());

            GM = GameObject.FindWithTag(Constants.GAME_MANAGER_TAG_NAME).GetComponent<GameManager>();
            GM.RegisterCar(this);

            name = string.Format("Player_{0}", _playerNo);
        }
    }


    private void StartItemChanged(SyncListInt.Operation op, int itemIndex)
    {
        YPLog.Trace();
        for (int index = 0; index < _startItem.Count; ++index)
        {
            YPLog.Log("index = " + index + ", id = " + _startItem[index]);
        }
    }

    private void CarAbilityChanged(SyncListStruct<ApplyAbility>.Operation op, int itemIndex)
    {
        if (null != _hud)
        {
            _hud.SetAbilities(GetCarAbilityID());
        }
    }

    private void EquipAbilityChanged(SyncListStruct<ApplyAbility>.Operation op, int itemIndex)
    {
        YPLog.Trace();
    }

    private void CarChanged(SyncListInt.Operation op, int itemIndex)
    {
        YPLog.Trace();
    }

    void Update()
    {
        if (!_matchStart)
        {
            return;
        }        

        UpdateSpeed();
        UpdateLane();
        UpdateJump();
        
        MouseTouchMove();
        TouchSlideMove();

        _hud.IngamePlayTime();
    }

    protected void MouseTouchMove()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _startPos = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            float absPos = Mathf.Abs(_startPos - Input.mousePosition.x);

            if (absPos > _distanceX)
            {
                float posValue = Mathf.Sign(_startPos - Input.mousePosition.x);
                if (posValue < 0)
                {
                    MoveLane(false);
                }
                else if (posValue > 0)
                {
                    MoveLane(true);
                }
            }
        }
    }

    protected void TouchSlideMove()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            _touchPos = Input.GetTouch(0).position;
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _startPos = _touchPos.x;
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                float absPos = Mathf.Abs(_startPos - _touchPos.x);
                if(absPos > _distanceX)
                {
                    float posValue = Mathf.Sign(_startPos - _touchPos.x);
                    if(posValue < 0)
                    {
                        MoveLane(false);
                    }
                    else if(posValue > 0)
                    {
                        MoveLane(true);
                    }
                }
            }
        }
    }

    void LateUpdate()
    {
        if (!_matchStart)
        {
            return;
        }

        UpdateSpline();
    }

    internal void SetTrackInfo(int aStartLane, int aGoalLapCount)
    {
        _startLane = aStartLane;
        _goalLapCount = aGoalLapCount;
    }

    internal void SetStartLane(int aLane)
    {
        YPLog.Trace();
        YPLog.Log("_startLane = " + _startLane + ", aLane = " + aLane);

        _laneNO = _startLane = aLane;

        GameObject startSplineGO = GameObject.FindWithTag(Constants.START_SPLINE_TAG_NAME);
        CurvySpline startSpline = startSplineGO.GetComponent<CurvySpline>();

        _swc.Spline = startSpline;
        _renderer.transform.localPosition = new Vector3(startSpline.GetLaneOffsetX(_startLane), startSpline.LaneOffsetY, 0f);
        _renderer.transform.localRotation = Quaternion.Euler(Vector3.zero);

        if (NetworkClient.active && isLocalPlayer)
        {
            SceneManager.LoadSceneAsync("hud", LoadSceneMode.Additive);
            SetCamera();
        }
    }

    internal void SetParts(int aID)
    {
        _body = aID;
        _tire = aID;

        ChangeBody(_body);
        ChangeTire(_tire);
    }

    private void ChangeBody(int aBody)
    {
        YPLog.Log("[ChangeBody] body = " + aBody);
        _body = aBody;
        if (_body > 0 && _bodyCreate == false)
        {
            _renderer.SpawnBody(aBody);
            _bodyCreate = true;
        }
    }

    private void ChangeTire(int aTire)
    {
        _tire = aTire;
        if (_tire > 0 && _tireCreate == false)
        {
            StartCoroutine(_renderer.DelayChangeTire(aTire));
            _tireCreate = true;
        }
    }

    internal void SetStat(float aMaxSpeed, float aAccelerate, float aPower, int aMaxHP)
    {
        _maxSpeed = aMaxSpeed;
        _accelerate = aAccelerate;
        _power = aPower;
        _maxHP = _hp = aMaxHP;
    }

//    internal void SetStartItem(protocol.match_item_info aInfo)
//    {
//        foreach (int id in aInfo.GetInfos().Keys)
//        {
//            _startItem.Add(id);
//        }
//    }

    [ClientRpc]
    public void RpcCountDown(int aCountDown)
    {
        //YPLog.Trace();
        //YPLog.Log("countDown = " + aCountDown + ", localPlayer = " + isLocalPlayer);
        _hud.SetLapCount(GM.GetPlayerCar());
        if (!isLocalPlayer)
        {
            return;
        }

        if (5 == aCountDown)
        {
            _camera.PlayPlayGameCamera();
            //hud work
            //_hud.ToggleHUD(true);
            _hud.TextChange();

        }
        else
        {
            //hud work
            //_hud.UpdateCountDown(aCountDown);
            if (aCountDown == 3)
            {
                StartCoroutine(_hud.CountDownImage(0, 1.0f));
            }

            if (3 >= aCountDown)
            {
                _sound.PlayCountDownSound(false);
            }
        }
    }

    [ClientRpc]
    public void RpcStartMatch()
    {
        YPLog.Log("player NO = " + _playerNo + ", isLocalPlayer = " + isLocalPlayer);

        _renderer.StopAnimation();
        _sound.PlayRunningSound();
        _matchStart = true;

        if (!isLocalPlayer)
        {
            return;
        }

        //GM.ClientStartMatch();
        _sound.PlayCountDownSound(true);
        _matchStartTime = Time.time;
    }

    [Command]
    public void CmdEndMatch(float aPlayGameTime, float aMoveDistance)
    {
//        _finalRank = GM.PlayerEndMatch(this);
//        _playGameTime = aPlayGameTime;
//
//        UpdateRankPlayData();
//        UpdatePlayData((short)protocol.record_data_key.TOTAL_PLAY_SEC, (int)aPlayGameTime);
//        UpdatePlayData((short)protocol.record_data_key.RUNNING_DISTANCE, (int)aMoveDistance);
//        CalcGameMoneyNExp();
//        ApplyEndGameAbility();
//        
//        SendGameResultInfo();        
    }

    internal void SendEndMatchToClient(int aFirstReward, int aSecondReward)
    {
        RpcEndMatch(_finalRank, _playGameTime, _exp, _gameMoney, aFirstReward, aSecondReward);
    }

    [ClientRpc]
    public void RpcEndMatch(int aRank, float aLapTime, int aExp, int aGameMoney, int aFirstReward, int aSecondReward)
    {
        _finalRank = aRank;
        _lapTime = aLapTime;
        _exp = aExp;
        _gameMoney = aGameMoney;

        if (isLocalPlayer)
        {   
            _hud.EndGame(this, aFirstReward, aSecondReward);
        }
    }

    #region camera
    private void SetCamera()
    {
        _camera = TRStatic.GetTRCamera();
        if (null == _camera)
        {
            YPLog.LogError("can't find camera!");
            return;
        }

        _camera.m_SynchronizeCar = gameObject.transform;
        _camera.PlayJoinGameCamera(this);
    }

    [Command]
    public void CmdEndJoinCameraMoving()
    {
        //YPLog.Log("Command :: End Joing Camera Moving.");
        GM.UpdatePlayerReady();
    }

    internal void ShakeCamera(float aDuration)
    {
        _camera.PlayCameraShake(aDuration);
    }
    #endregion

    #region move
    protected void UpdateSpeed()
    {
        float maxSpeedBonus = 0f;
        CalcMaxSpeedBonus(ref maxSpeedBonus);
        float limitSpeed = _maxSpeed + maxSpeedBonus;
        if (limitSpeed < 0f)
        {
            limitSpeed = 0f;
        }

        if (_speed > limitSpeed)
        {
            _speed += Constants.DECELERATE * Time.deltaTime;
        }
        else
        {
            _speed += _accelerate * Time.deltaTime;
            if (_speed > limitSpeed)
            {
                _speed = limitSpeed;
            }
        }

        if (_hp <= 0 && !_runawayActivated)
        {
            _speed = 0f;
        }

        if (_hp > 0 && _rewindActivated)
        {
            _speed = (_maxSpeed + maxSpeedBonus) * _rewindPercentage;
            _rewindActivated = false;

            YPLog.Trace();
            YPLog.Log("rewind activated! speed = " + _speed + ", maxSpeed = " + _maxSpeed);

            if (isLocalPlayer)
            {
                _hud.AbilityActivated(32102100);
            }
        }

        if (0f > _speed)
        {
            _speed = 0f;
        }

        _swc.Speed = _speed;
    }

    private void CalcMaxSpeedBonus(ref float oMaxSpeed)
    {
        // collide
        foreach (KeyValuePair<uint, SpeedBonus> kv in _collideSpeedBonus)
        {
            SpeedBonus bonus = kv.Value;
            if (0f == bonus._bonusTime)
            {
                continue;
            }

            bonus._currentTime += Time.deltaTime;
            if (bonus._bonusTime < bonus._currentTime)
            {
                bonus._currentTime = 0f;
                bonus._bonusTime = 0f;
                bonus._bonusSpeed = 0f;
            }
            else
            {
                oMaxSpeed += bonus._bonusSpeed;
            }
        }

        for (int index = _speedBonus.Count - 1; index >= 0; --index)
        {
            _speedBonus[index]._currentTime += Time.deltaTime;
            if (_speedBonus[index]._bonusTime < _speedBonus[index]._currentTime)
            {
                _speedBonus.RemoveAt(index);
            }
            else
            {
                oMaxSpeed += _speedBonus[index]._bonusSpeed;
            }
        }

        if (-1f != _rollingBooster._currentTime)
        {
            _rollingBooster._currentTime += Time.deltaTime;
            if (_rollingBooster._bonusTime < _rollingBooster._currentTime)
            {
                _rollingBooster._currentTime = -1f;
            }
            else
            {
                oMaxSpeed += _rollingBooster._bonusSpeed;
            }
        }

        //// adjust
        //if (0f != m_adjustSpeedBonus.bonusTime)
        //{
        //    m_adjustSpeedBonus.currentTime += Time.deltaTime;
        //    if (m_adjustSpeedBonus.currentTime > m_adjustSpeedBonus.bonusTime)
        //    {
        //        m_adjustSpeedBonus.bonusTime = 0f;
        //    }
        //    else
        //    {
        //        oMaxSpeed += m_adjustSpeedBonus.bonusSpeed;
        //        oAccelerate += m_adjustSpeedBonus.bonusAccelerate;
        //    }
        //}

        //if (_runawayActivated)
        //{
        //    oMaxSpeed += _runawayBonusSpeed;
        //}
    }

    void UpdateSpline()
    {
        _moveDistance += _swc.MoveDistance;

        if (_swc.ClampTF && _goalLapCount > _lapCount)
        {   
            ++_lapCount;
            ApplyLapCountAbility();
            
            if (isLocalPlayer)
            {
                float lapTime = Time.time - _lapStartTime;
                if (0f == _bestLapTime || _bestLapTime > lapTime)
                {
                    _bestLapTime = lapTime;
                }

                if (_goalLapCount == _lapCount)
                {
                    _matchStart = false;
                    _sound.StopRunningSound();
                    _camera.StopBGM();

                    _playGameTime = Time.time - _matchStartTime;
                    YPLog.Log("_playGameTime = " + _playGameTime);
                    CmdEndMatch(_playGameTime, _moveDistance);

                    _matchStart = false;
                }
                else
                {
                    _hud.UpdateLapCount(_lapCount);
                    if (_goalLapCount == _lapCount + 1)
                    {
                        //hud work
                        StartCoroutine(_hud.ShowFinalLapEffect());
                        _hud.TextChange();
                    }
                }

                _lapStartTime = Time.time;
            }
            else if (_isAI && NetworkServer.active)
            {
                if (_goalLapCount == _lapCount)
                {
                    _matchStart = false;
                    _sound.StopRunningSound();

                    _lapTime = Time.time - _matchStartTime;
                    _finalRank = GM.PlayerEndMatch(this);
                }
            }
        }

        // TO DO : split spline
        /*
        if (SWC.Spline.tag == Constants.START_SPLINE_TAG_NAME)
            DoUpdateLap();
        */

        //m_currentSpline = SWC.Spline;
        //CarTransform.localPosition = new Vector3(m_currentSpline.GetLaneOffsetX(LaneNO), CarTransform.localPosition.y, CarTransform.localPosition.z);
    }

    internal IEnumerator DecideMove()
    {
        //YPLog.Trace();
        //YPLog.Log("ai = " + _isAI + ", server = " + NetworkServer.active + ", matchStart = " + _matchStart);
        if (!_isAI || !NetworkServer.active)
        {
            yield break;
        }

        while (!_matchStart)
        {
            yield return null;
        }

        _matchStartTime = Time.time;
        yield return new WaitForSeconds(1.5f);
        while (_matchStart)
        {
            yield return new WaitForSeconds(Random.Range(0.7f, 2f));
            if (!_oneWay && Constants.MoveLaneState.Stay == _moveLaneState && Constants.JumpState.None == _jumpState)
            {
                Constants.MoveLaneState nextMove = Constants.MoveLaneState.Stay;
                CarController near = GM.GetNearestCar(this);

                if (null != near && 0.005f > Mathf.Abs(near._swc.TF - _swc.TF))
                {
                    if (near._laneNO > _laneNO)
                    {
                        nextMove = Constants.MoveLaneState.MoveRight;
                    }
                    else if (near._laneNO < _laneNO)
                    {
                        nextMove = Constants.MoveLaneState.MoveLeft;
                    }
                    else
                    {
                        nextMove = _lastMove;
                    }
                }
                else
                {
                    if (Constants.MIN_LANE_NO == _laneNO)
                    {
                        nextMove = Constants.MoveLaneState.MoveRight;
                    }
                    else if (_swc.Spline.LaneCount == _laneNO)
                    {
                        nextMove = Constants.MoveLaneState.MoveLeft;
                    }
                    else if (Constants.MoveLaneState.MoveLeft == _lastMove)
                    {
                        nextMove = Constants.MoveLaneState.MoveLeft;
                    }
                    else if (Constants.MoveLaneState.MoveRight == _lastMove)
                    {
                        nextMove = Constants.MoveLaneState.MoveRight;
                    }
                    else
                    {
                        nextMove = _lastMove;
                    }
                }

                //YPLog.Log("[decide move] last = " + _lastMove + ", next = " + nextMove);

                bool left = (Constants.MoveLaneState.MoveLeft == nextMove);
                bool canMove = true;
                if (left)
                {
                    if (_laneNO == _swc.Spline.StartLaneNO)
                    {
                        canMove = false;
                    }
                }
                else
                {
                    int maxLaneNO = _swc.Spline.StartLaneNO + _swc.Spline.LaneCount - 1;
                    if (_laneNO == maxLaneNO)
                    {
                        canMove = false;
                    }
                }

                _lastMove = nextMove;
                if (canMove)
                {
                    if (_jamming)
                    {
                        left = !left;
                    }

                    GM.CheckSchoolZone(this, left, _laneNO);

                    ChangeLane(left, _laneNO);
                    RpcChangeLane(left, _laneNO);
                }
            }
        }
    }

    protected void MoveLane(bool aLeft)
    {
        if (_oneWay)
        {
            return;
        }

        if (Constants.MoveLaneState.Stay != _moveLaneState)
        {
            return;
        }

        if (Constants.JumpState.None != _jumpState)
        {
            return;
        }

        if (aLeft)
        {
            if (_laneNO == _swc.Spline.StartLaneNO)
            {
                YPLog.Log("can't move left anymore, current lane = " + _laneNO + ", left end lane = " + _swc.Spline.StartLaneNO);
                return;
            }
        }
        else
        {
            int maxLaneNO = _swc.Spline.StartLaneNO + _swc.Spline.LaneCount - 1;
            if (_laneNO == maxLaneNO)
            {
                YPLog.Log("can't move right anymore, current lane = " + _laneNO + ", right end lane = " + maxLaneNO);
                return;
            }
        }
        CmdChangeLane(aLeft, _laneNO);
    }

    [Command]
    public void CmdChangeLane(bool aLeft, int aLaneNO)
    {
        YPLog.Trace();
        YPLog.Log("local Client Active = " + NetworkServer.localClientActive);
        if (_jamming)
        {
            aLeft = !aLeft;
        }

        GM.CheckSchoolZone(this, aLeft, aLaneNO);

        ChangeLane(aLeft, aLaneNO);
        RpcChangeLane(aLeft, aLaneNO);
    }

    [ClientRpc]
    public void RpcChangeLane(bool aLeft, int aLaneNO)
    {
        if (NetworkServer.localClientActive)
        {
            return;
        }
        ChangeLane(aLeft, aLaneNO);
    }

    protected virtual void ChangeLane(bool aLeft, int aLaneNO)
    {
        //YPLog.Trace();
        //YPLog.Log("current = " + _laneNO + ", param = " + aLaneNO);

        if (aLeft)
        {
            if (_laneNO == _swc.Spline.StartLaneNO)
            {
                YPLog.Log("can't move left anymore, current lane = " + _laneNO + ", left end lane = " + _swc.Spline.StartLaneNO);
                return;
            }

            --_laneNO;
            _moveLaneState = Constants.MoveLaneState.MoveLeft;
            _moveLaneOffset = _swc.Spline.LaneWidth;
            _moveLaneCurrentVelocity = _moveLaneStartVelocity;
            //_renderer.PlayAnimation(Constants.CAR_MOVE_LEFT_ANIMATION_NAME);

            Animator anim = _renderer.GetComponent<Animator>();
            anim.speed = 1.5f;
            anim.Play(Constants.ANI_MOVE_LEFT_ANIMATION_NAME);
        }
        else
        {
            int maxLaneNO = _swc.Spline.StartLaneNO + _swc.Spline.LaneCount - 1;
            if (_laneNO == maxLaneNO)
            {
                YPLog.Log("can't move right anymore, current lane = " + _laneNO + ", right end lane = " + maxLaneNO);
                return;
            }

            ++_laneNO;
            _moveLaneState = Constants.MoveLaneState.MoveRight;
            _moveLaneOffset = _swc.Spline.LaneWidth;
            _moveLaneCurrentVelocity = _moveLaneStartVelocity;
            //_renderer.PlayAnimation(Constants.CAR_MOVE_RIGHT_ANIMATION_NAME);
            
            Animator anim = _renderer.GetComponent<Animator>();
            anim.speed = 1.5f;
            anim.Play(Constants.ANI_MOVE_RIGHT_ANIMATION_NAME);
        }
        _swc.AdditionalTags = _laneNO.ToString();
    }

    protected void UpdateLane()
    {
        if (Constants.MoveLaneState.Stay == _moveLaneState)
        {
            return;
        }

        int moveDir = 1;
        if (Constants.MoveLaneState.MoveLeft == _moveLaneState || Constants.MoveLaneState.MoveLeftBack == _moveLaneState)
        {
            moveDir = -1;
        }

        if (Mathf.Abs(_moveLaneCurrentOffset) >= _moveLaneOffset * 0.6f)
        {
            _moveLaneCurrentVelocity -= _moveLaneVelocityChangeRatio * Time.deltaTime;
        }
        else
        {
            _moveLaneCurrentVelocity += _moveLaneVelocityChangeRatio * Time.deltaTime;
        }

        float moveOffset = _moveLaneCurrentVelocity * Time.deltaTime * moveDir;
        _moveLaneCurrentOffset += moveOffset;

        //YPLog.Log("state = " + _moveLaneState + ", dir = " + moveDir + ", velocity = " + _moveLaneCurrentVelocity + ", offset = " + _moveLaneCurrentOffset);
        //YPLog.Log("tick move offset = " + moveOffset);

        _renderer.transform.localPosition += new Vector3(moveOffset, 0f, 0f);
        if (Mathf.Abs(_moveLaneCurrentOffset) >= _moveLaneOffset)
        {
            _moveLaneState = Constants.MoveLaneState.Stay;
            _moveLaneOffset = 0f;
            _moveLaneCurrentVelocity = 0f;
            _moveLaneCurrentOffset = 0f;
            _renderer.transform.localPosition = new Vector3(_swc.Spline.GetLaneOffsetX(_laneNO), _renderer.transform.localPosition.y, _renderer.transform.localPosition.z);

            //_moveLaneCurrentOffset = 0f;
            //_moveLaneCurrentVelocity = 0f;
            //_moveLaneOffset = 0f;
            //MoveLaneState = Constants.MoveLaneState.Stay;
            //m_pushed = false;
        }
    }

    [ClientRpc]
    public void RpcBackToPrevLane()
    {
        YPLog.Trace();
        YPLog.Log("playerNo = " + _playerNo + ", localPlayer = " + isLocalPlayer + ", moveLaneState = " + _moveLaneState, true);
        //YPLog.Log("server = " + NetworkServer.active + ", client = " + NetworkClient.active + ", host = " + NetworkServer.localClientActive, true);

        if (NetworkServer.localClientActive)
        {
            return;
        }

        BackToPrevLane();
    }

    private void BackToPrevLane()
    {
        if (Constants.MoveLaneState.Stay == _moveLaneState ||
            Constants.MoveLaneState.MoveLeftBack == _moveLaneState ||
            Constants.MoveLaneState.MoveRightBack == _moveLaneState)
        {
            return;
        }

        if (Constants.MoveLaneState.MoveRight == _moveLaneState)
        {
            --_laneNO;
            _moveLaneState = Constants.MoveLaneState.MoveLeftBack;
            _renderer.PlayAnimation(Constants.CAR_MOVE_LEFT_ANIMATION_NAME);
        }
        else if (Constants.MoveLaneState.MoveLeft == _moveLaneState)
        {
            ++_laneNO;
            _moveLaneState = Constants.MoveLaneState.MoveRightBack;
            _renderer.PlayAnimation(Constants.CAR_MOVE_RIGHT_ANIMATION_NAME);
        }

        _moveLaneOffset = Mathf.Abs(_moveLaneCurrentOffset);
        _moveLaneCurrentOffset = 0f;
        _moveLaneCurrentVelocity = _moveLaneStartVelocity;
    }

    protected void UpdateJump()
    {
        if (Constants.JumpState.None == _jumpState)
        {
            return;
        }

        if (_changeRotationDuringJump)
        {
            CarTransform.localRotation = Quaternion.Euler(CarTransform.localRotation.eulerAngles + new Vector3(0, 720f * Time.deltaTime, 0f));
        }

        if (Constants.JumpState.GoingUp == _jumpState)
        {
            _jumpVelocity -= _jumpVelocityChangeRatio * Time.deltaTime;
            _jumpOffset += _jumpVelocity * Time.deltaTime;

            if (_jumpVelocity <= 0f)
            {
                _jumpState = Constants.JumpState.GoingDown;
            }
        }
        else if (Constants.JumpState.GoingDown == _jumpState)
        {
            _jumpVelocity += _jumpVelocityChangeRatio * Time.deltaTime;
            _jumpOffset -= _jumpVelocity * Time.deltaTime;
        }

        //YPLog.Log("jumpOffset = " + m_jumpOffset);

        // floor check
        if (0f >= _jumpOffset)
        {
            _jumpState = Constants.JumpState.None;
            _jumpOffset = _swc.Spline.LaneOffsetY;
            _changeRotationDuringJump = false;
            CarTransform.localRotation = _oldLocalRotation;

            //_fx.PlayLandingEffectParticle(this);
            _sound.PlayLandingSound();
        }

        CarTransform.localPosition = new Vector3(_renderer.transform.localPosition.x, _jumpOffset, _renderer.transform.localPosition.z);
    }

    internal void StartJump(bool aChangeRotation, float aStartVelocity = Constants.JUMP_START_VELOCITY)
    {
        if (Constants.JumpState.None != _jumpState)
        {
            YPLog.Log("already jump!!!");
            return;
        }

        _jumpVelocity = aStartVelocity;
        _jumpState = Constants.JumpState.GoingUp;
        _changeRotationDuringJump = aChangeRotation;
        _oldLocalRotation = CarTransform.localRotation;
    }

    [Command]
    public void CmdStartJump(bool aChangeRotation, float aStartVelocity)
    {
        StartJump(aChangeRotation, aStartVelocity);
        RpcStartJump(aChangeRotation, aStartVelocity);
    }

    [ClientRpc]
    public void RpcStartJump(bool aChangeRotation, float aStartVelocity)
    {
        if (NetworkServer.localClientActive)
        {
            return;
        }

        StartJump(aChangeRotation, aStartVelocity);
    }
#endregion

#region Collide
    internal virtual void Collide(CarController aOtherCar)
    {
        // only check at server
        if (!NetworkServer.active)
        {
            return;
        }

        if (!_collideCars.Contains(aOtherCar))
        {
            aOtherCar.UpdateCollideCar(this, true);
            return;
        }

        YPLog.Trace();
        YPLog.Log("this = " + this + ", other = " + aOtherCar, true);

        if (0f < _rollingBooster._currentTime || 0f < aOtherCar._rollingBooster._currentTime)
        {
            RollingBoosterCollide(aOtherCar);
        }
        else
        {
            if ((_moveLaneState > Constants.MoveLaneState.Stay) || (aOtherCar._moveLaneState > Constants.MoveLaneState.Stay))
            {
                SideCollide(aOtherCar);
            }
            else
            {
                FrontRearCollide(aOtherCar);
            }
        }
    }

    /*
     * no speed bonus both.
     */
    private void RollingBoosterCollide(CarController aOtherCar)
    {
        YPLog.Trace();

        BackToPrevLane();
        RpcBackToPrevLane();

        aOtherCar.BackToPrevLane();
        aOtherCar.RpcBackToPrevLane();

        if (0f < _rollingBooster._currentTime)
        {
            IncreaseRollingBoosterTime(_IncreaseRollingBoosterBonusTime);
            RpcIncreaseRollingBoosterTime(_IncreaseRollingBoosterBonusTime);

            _rollingBoosterItem.IncreaseRollingBoosterApplyTime(_IncreaseRollingBoosterBonusTime);
        }

        if (0f < aOtherCar._rollingBooster._currentTime)
        {
            aOtherCar.IncreaseRollingBoosterTime(aOtherCar._IncreaseRollingBoosterBonusTime);
            aOtherCar.RpcIncreaseRollingBoosterTime(aOtherCar._IncreaseRollingBoosterBonusTime);

            aOtherCar._rollingBoosterItem.IncreaseRollingBoosterApplyTime(aOtherCar._IncreaseRollingBoosterBonusTime);
        }

        if (aOtherCar._power < _power)
        {
            UpdateCollideHP(aOtherCar, this);
        }
        else if (aOtherCar._power > _power)
        {
            UpdateCollideHP(this, aOtherCar);
        }
    }

    private void SideCollide(CarController aOtherCar)
    {
        UpdatePlayData((short)Constants.StatKey.BUMP_ATTACK, 1);
        BackToPrevLane();
        RpcBackToPrevLane();

        aOtherCar.UpdatePlayData((short)Constants.StatKey.BUMP_ATTACK, 1);
        aOtherCar.BackToPrevLane();
        aOtherCar.RpcBackToPrevLane();

        //YPLog.Log("this car, net ID = " + netId + ", controllerID = " + playerControllerId, true);
        //YPLog.Log("max Speed = " + _maxSpeed + ", current speed = " + _speed + ", accelerate = " + _accelerate + ", power = " + _power, true);

        //YPLog.Log("other car, net ID = " + aOtherCar.netId + ", controllerID = " + aOtherCar.playerControllerId, true);
        //YPLog.Log("max Speed = " + aOtherCar._maxSpeed + ", current speed = " + aOtherCar._speed + ", accelerate = " + aOtherCar._accelerate + ", power = " + aOtherCar._power, true);

        if (aOtherCar._power == _power)
        {
            if (_speed != aOtherCar._speed)
            {
                // 1. current speed
                if (aOtherCar._speed < _speed)
                {
                    float speedPenalty = aOtherCar._speed * Constants.CAR_COLLIDE_SPEED_PENALTY_RATIO;
                    aOtherCar.ApplyCollideResult(netId.Value, speedPenalty, false, false);
                    aOtherCar.RpcApplyCollideResult(netId.Value, speedPenalty, false, false);
                }
                else
                {
                    float speedPenalty = _speed * Constants.CAR_COLLIDE_SPEED_PENALTY_RATIO;
                    ApplyCollideResult(aOtherCar.netId.Value, speedPenalty, false, false);
                    RpcApplyCollideResult(aOtherCar.netId.Value, speedPenalty, false, false);
                }
            }
            else
            {
                if (_accelerate != aOtherCar._accelerate)
                {
                    // 2. accelerate
                    if (aOtherCar._accelerate < _accelerate)
                    {
                        float speedPenalty = aOtherCar._speed * Constants.CAR_COLLIDE_SPEED_PENALTY_RATIO;
                        aOtherCar.ApplyCollideResult(netId.Value, speedPenalty, false, false);
                        aOtherCar.RpcApplyCollideResult(netId.Value, speedPenalty, false, false);
                    }
                    else
                    {
                        float speedPenalty = _speed * Constants.CAR_COLLIDE_SPEED_PENALTY_RATIO;
                        ApplyCollideResult(aOtherCar.netId.Value, speedPenalty, false, false);
                        RpcApplyCollideResult(aOtherCar.netId.Value, speedPenalty, false, false);
                    }
                }
                else
                {
                    // 3. max speed
                    if (aOtherCar._maxSpeed < _maxSpeed)
                    {
                        float speedPenalty = aOtherCar._speed * Constants.CAR_COLLIDE_SPEED_PENALTY_RATIO;
                        aOtherCar.ApplyCollideResult(netId.Value, speedPenalty, false, false);
                        aOtherCar.RpcApplyCollideResult(netId.Value, speedPenalty, false, false);
                    }
                    else if (aOtherCar._maxSpeed > _maxSpeed)
                    {
                        float speedPenalty = _speed * Constants.CAR_COLLIDE_SPEED_PENALTY_RATIO;
                        ApplyCollideResult(aOtherCar.netId.Value, speedPenalty, false, false);
                        RpcApplyCollideResult(aOtherCar.netId.Value, speedPenalty, false, false);
                    }
                }
            }
        }
        else if (aOtherCar._power < _power)
        {
            UpdateCollideHP(aOtherCar, this);

            float penalty = aOtherCar._speed * Constants.CAR_COLLIDE_SPEED_PENALTY_RATIO;
            aOtherCar.ApplyCollideResult(netId.Value, penalty, false, false);
            aOtherCar.RpcApplyCollideResult(netId.Value, penalty, false, false);
        }
        else
        {
            UpdateCollideHP(this, aOtherCar);

            float penalty = _speed * Constants.CAR_COLLIDE_SPEED_PENALTY_RATIO;
            ApplyCollideResult(aOtherCar.netId.Value, penalty, false, false);
            RpcApplyCollideResult(aOtherCar.netId.Value, penalty, false, false);
        }

        UpdateCollideCar(aOtherCar, false);
    }

    /*
     * front car(A), rear car(B)
     * if power of A is bigger than power of B : A get +speed bonus, B get -speed bonus
     * if power of B is bigger than power of A : A get -speed bonus with jump, B pass through with +speed bonus
     */
    private void FrontRearCollide(CarController aOtherCar)
    {
        UpdatePlayData((short)Constants.StatKey.BUMP_ATTACK, 1);
        aOtherCar.UpdatePlayData((short)Constants.StatKey.BUMP_ATTACK, 1);

        if (aOtherCar._swc.TF < _swc.TF)
        {
            float speedDiff = Mathf.Abs(aOtherCar._speed - _speed);
            if (aOtherCar._power <= _power)
            {
                UpdateCollideHP(aOtherCar, this);
                ApplyCollideResult(aOtherCar.netId.Value, speedDiff, false, true);
                aOtherCar.ApplyCollideResult(netId.Value, speedDiff * -1f, false, true);
                RpcApplyCollideResult(aOtherCar.netId.Value, speedDiff, false, true);
                aOtherCar.RpcApplyCollideResult(netId.Value, speedDiff * -1f, false, true);
            }
            else
            {
                UpdateCollideHP(this, aOtherCar);
                ApplyCollideResult(aOtherCar.netId.Value, speedDiff * -1f, true, true);
                aOtherCar.ApplyCollideResult(netId.Value, speedDiff, false, true);
                RpcApplyCollideResult(aOtherCar.netId.Value, speedDiff * -1f, true, true);
                aOtherCar.RpcApplyCollideResult(netId.Value, speedDiff, false, true);
            }
        }
        else if (aOtherCar._swc.TF > _swc.TF)
        {
            float speedDiff = Mathf.Abs(_speed - aOtherCar._speed);
            if (aOtherCar._power <= _power)
            {
                UpdateCollideHP(aOtherCar, this);
                ApplyCollideResult(aOtherCar.netId.Value, speedDiff, false, true);
                aOtherCar.ApplyCollideResult(netId.Value, speedDiff * -1f, true, true);
                RpcApplyCollideResult(aOtherCar.netId.Value, speedDiff, false, true);
                aOtherCar.RpcApplyCollideResult(netId.Value, speedDiff * -1f, true, true);
            }
            else
            {
                UpdateCollideHP(this, aOtherCar);
                ApplyCollideResult(aOtherCar.netId.Value, speedDiff * -1f, false, true);
                aOtherCar.ApplyCollideResult(netId.Value, speedDiff, false, true);
                RpcApplyCollideResult(aOtherCar.netId.Value, speedDiff * -1f, false, true);
                aOtherCar.RpcApplyCollideResult(netId.Value, speedDiff, false, true);
            }
        }

        UpdateCollideCar(aOtherCar, false);
    }

    private void UpdateCollideCar(CarController aCollideCar, bool aAdd)
    {
        if (aAdd)
        {
            _collideCars.Add(aCollideCar);
        }
        else
        {
            _collideCars.Remove(aCollideCar);
        }
    }

    [ClientRpc]
    public void RpcApplyCollideResult(uint aPlayerID, float aMaxSpeedBonus, bool aJump, bool aImmediately)
    {
        YPLog.Trace();
        YPLog.Log("==== [RpcApplayCollideResult], server = " + NetworkServer.active + ", client = " + NetworkClient.active + ", host = " + NetworkServer.localClientActive);
        if (NetworkServer.localClientActive)
        {
            return;
        }

        ApplyCollideResult(aPlayerID, aMaxSpeedBonus, aJump, aImmediately);
    }

    private void ApplyCollideResult(uint aPlayerID, float aMaxSpeedBonus, bool aJump, bool aImmediately)
    {
        YPLog.Trace();
        YPLog.Log("id = " + aPlayerID + ", speedBonus = " + aMaxSpeedBonus);

        SpeedBonus bonus = null;
        if (_collideSpeedBonus.TryGetValue(aPlayerID, out bonus))
        {
            bonus._bonusSpeed = aMaxSpeedBonus;
            bonus._bonusTime = Constants.COLLIDE_BONUS_MAX_TIME;
            bonus._currentTime = 0f;
        }
        else
        {
            bonus = new SpeedBonus();
            bonus._bonusSpeed = aMaxSpeedBonus;
            bonus._bonusTime = Constants.COLLIDE_BONUS_MAX_TIME;
            bonus._currentTime = 0f;

            _collideSpeedBonus.Add(aPlayerID, bonus);
        }

        if (aJump)
        {
            StartJump(false);
        }

        //if (aImmediately)
        //{
        _speed += aMaxSpeedBonus;
        //}
    }
#endregion

#region hp
    private void UpdateCollideHP(CarController aSmallPowerCar, CarController aBigPowerCar)
    {
        float diff = aSmallPowerCar._power - aBigPowerCar._power;
        aSmallPowerCar.UpdateHP((int)diff, aBigPowerCar, true);
    }

    internal void UpdateHP(int aHP, CarController aAttackCar, bool aCollide)
    {
        YPLog.Trace();
        YPLog.Log("damage = " + aHP + ", attack = " + aAttackCar + ", this = " + this);
        YPLog.Log("current hp = " + _hp);

        if (0 > aHP)
        {
            aAttackCar.ChangeDamage(ref aHP, aCollide, true);
            ChangeDamage(ref aHP, aCollide, false);
        }

        if (0 > aHP && 0 == _hp)
        {
            return;
        }

        bool fullHP = (_maxHP == _hp);
        _hp += aHP;
        if (0 >= _hp)
        {
            _hp = 0;

            UpdatePlayData((short)Constants.StatKey.DEATH, 1);
            ApplyZeroHPAbility();

            if (null != aAttackCar)
            {
                aAttackCar.UpdatePlayData((short)Constants.StatKey.KILL, 1);
                if (0 > aHP && fullHP)
                {
                    aAttackCar.UpdatePlayData((short)Constants.StatKey.ONE_SHOT_KILL, 1);
                }
            }

            StartCoroutine(HPRecovery());
        }
        else if (_maxHP < _hp)
        {
            _hp = _maxHP;
        }
    }

    private IEnumerator HPRecovery()
    {
        if (_runawayActivated)
        {
            yield return new WaitForSeconds(_runawayBonusSpeedTime);
            _runawayActivated = false;
        }

        yield return new WaitForSeconds(_explosionTime);
        UpdateHP(_maxHP, null, false);
    }

    private void ChangeHP(int aHP)
    {
        _hp = aHP;
        if (0 == _hp)
        {
            _fx.PlayExplosionFX(_body);
        }
    }
#endregion

#region ability
//    internal void SetCarAbility(protocol.vehicle_skill[] aSkill)
//    {
//        for (int index = 0; index < aSkill.Length; ++index)
//        {
//            ApplyAbility ability;
//            ability._id = aSkill[index].GetSkillId();
//            ability._level = aSkill[index].GetSkillLv();
//            _carAbility.Add(ability);
//        }
//    }
//
//    internal void SetEquipAbility(protocol.ability_slot aEquipped, protocol.ability_list aList)
//    {
//        Dictionary<protocol.ability_slot_no, int> equipped = aEquipped.GetSlots();
//        protocol.ability[] abilityList = aList.GetInfos();
//        foreach (int id in equipped.Values)
//        {
//            for (int index = 0; index < abilityList.Length; ++index)
//            {
//                if (id == abilityList[index].GetAbilityId())
//                {
//                    ApplyAbility ability;
//                    ability._id = id;
//                    ability._level = abilityList[index].GetLevel();
//                    _equipAbility.Add(ability);
//                }
//            }
//        }
//    }

    internal int ChangeItem(int aItemID, ref int oAbilityID, ref int oApplyLevel)
    {
        for (int index = 0; index < _carAbility.Count; ++index)
        {
            int id = AbilityDatabase.Instance.ChangeItem(_carAbility[index]._id, _carAbility[index]._level, aItemID, this);
            if (-1 != id)
            {
                oAbilityID = _carAbility[index]._id;
                oApplyLevel = _carAbility[index]._level;
                return id;
            }
        }

        return -1;
    }

    private void ChangeDamage(ref int oDamage, bool aCollide, bool aAttack)
    {
        for (int index = 0; index < _carAbility.Count; ++index)
        {
            AbilityDatabase.Instance.ChangeDamage(_carAbility[index]._id, _carAbility[index]._level, ref oDamage, aCollide, aAttack, this);
        }

        for (int index = 0; index < _equipAbility.Count; ++index)
        {
            AbilityDatabase.Instance.ChangeDamage(_equipAbility[index]._id, _equipAbility[index]._level, ref oDamage, aCollide, aAttack, this);
        }

        //for (int index = 0; index < _startItem.Count; ++index)
        //{
        //    AbilityDatabase.Instance.ChangeDamage(_startItem[index], 1, ref oDamage, aCollide, aAttack, this);
        //}
    }

    internal float ChangeSlowTime(float aTime, bool aAttack)
    {
        float applyTime = aTime;
        for (int index = 0; index < _equipAbility.Count; ++index)
        {
            applyTime = AbilityDatabase.Instance.ChangeSlowTime(_equipAbility[index]._id, _equipAbility[index]._level, applyTime, aAttack);
        }

        return applyTime;
    }

    internal void GetItemBox()
    {
        for (int index = 0; index < _carAbility.Count; ++index)
        {
            AbilityDatabase.Instance.GetItemBox(_carAbility[index]._id, _carAbility[index]._level, this);
        }
    }

    private void ApplyZeroHPAbility()
    {
        // barricade
        for (int index = 0; index < _carAbility.Count; ++index)
        {
            AbilityDatabase.Instance.ApplyAbilityAtZeroHP(_carAbility[index]._id, _carAbility[index]._level, this);
        }
    }

    private void ApplyLapCountAbility()
    {
        // last spurt
        for (int index = 0; index < _startItem.Count; ++index)
        {
            AbilityDatabase.Instance.ApplyAbilityAtLapCount(_startItem[index], 1, this);
        }

        // lap boost
        for (int index = 0; index < _carAbility.Count; ++index)
        {
            AbilityDatabase.Instance.ApplyAbilityAtLapCount(_carAbility[index]._id, _carAbility[index]._level, this);
        }
    }

    private void ApplyEndGameAbility()
    {
        //for (int index = 0; index < Constants.MAX_CAR_ABILITY_SLOT_COUNT; ++index)
        //{
        //    TRAbilityManager.Instance.ApplyAbilityAtEndGame(m_carAbilityID[index], m_carAbilityLevel[index], this);
        //}

        //for (int index = 0; index < Constants.MAX_EQUIP_ABILITY_SLOT_COUNT; ++index)
        //{
        //    TRAbilityManager.Instance.ApplyAbilityAtEndGame(m_equipAbilityID[index], m_equipAbilityLevel[index], this);
        //}

        //for (int index = 0; index < m_startItemID.Length; ++index)
        //{
        //    TRAbilityManager.Instance.ApplyAbilityAtEndGame(m_startItemID[index], 0, this);
        //}
    }

    internal void ApplyStartGameAbility()
    {
        for (int index = 0; index < _carAbility.Count; ++index)
        {
            AbilityDatabase.Instance.ApplyAbilityAtStartGame(_carAbility[index]._id, _carAbility[index]._level, this);
        }

        for (int index = 0; index < _equipAbility.Count; ++index)
        {
            AbilityDatabase.Instance.ApplyAbilityAtStartGame(_equipAbility[index]._id, _equipAbility[index]._level, this);
        }

        for (int index = 0; index < _startItem.Count; ++index)
        {
            AbilityDatabase.Instance.ApplyAbilityAtStartGame(_startItem[index], 1, this);
        }
    }

    internal void ApplyAbilityBonusSpeed(float aSpeed, float aTime, GameObject aApplyFX, AudioClip aApplySound)
    {
        SpeedBonus bonus = new SpeedBonus();
        bonus._bonusSpeed = aSpeed;
        bonus._bonusTime = aTime;
        bonus._currentTime = 0f;
        _speedBonus.Add(bonus);

        _fx.PlayEffectParticle(aApplyFX, aTime);
        _sound.PlaySoundOneShot(aApplySound);
    }

    [ClientRpc]
    public void RpcApplyAbilityBonusSpeed(float aSpeed, float aTime, int aID)
    {
        if (NetworkServer.localClientActive)
        {
            return;
        }

        GameObject fx = AbilityDatabase.Instance.GetApplyFX(aID);
        AudioClip sound = AbilityDatabase.Instance.GetApplySound(aID);
        ApplyAbilityBonusSpeed(aSpeed, aTime, fx, sound);
    }

    internal void ApplyRollingBooster(float aSpeed, float aTime, float aMaxTime, float aIncreaseTime)
    {
        _rollingBooster._currentTime = 0f;
        _rollingBooster._bonusTime = aTime;
        _rollingBooster._bonusSpeed = aSpeed;

        _maxRollingBoosterBonusTime = aMaxTime;
        _IncreaseRollingBoosterBonusTime = aIncreaseTime;
    }

    [ClientRpc]
    public void RpcApplyRollingBooster(float aSpeed, float aTime, float aMaxTime, float aIncreaseTime)
    {
        ApplyRollingBooster(aSpeed, aTime, aMaxTime, aIncreaseTime);
    }

    internal void IncreaseRollingBoosterTime(float aTime)
    {
        if (-1f == _rollingBooster._currentTime)
        {
            return;
        }

        if (_maxRollingBoosterBonusTime <= _rollingBooster._bonusTime)
        {
            return;
        }

        _rollingBooster._bonusTime += aTime;
    }

    [ClientRpc]
    public void RpcIncreaseRollingBoosterTime(float aTime)
    {
        IncreaseRollingBoosterTime(aTime);
    }

    [ClientRpc]
    public void RpcAbilityActivated(int aID, float aGauge, bool aShowFX)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        _hud.UpdateAbilityGauge(aID, aGauge);
        if (1f == aGauge)
        {
            _hud.AbilityActivated(aID);
        }

        if (aShowFX)
        {
            GameObject fx = AbilityDatabase.Instance.GetApplyFX(aID);
            AudioClip sound = AbilityDatabase.Instance.GetApplySound(aID);

            _fx.PlayEffectParticle(fx);
            _sound.PlaySoundOneShot(sound);
        }
    }

    internal void ActivatedRunawayTrain(float aTime)
    {
        _runawayActivated = true;
        _runawayBonusSpeedTime = aTime;
    }
#endregion

#region item
    [Command]
    public void CmdGetItem(int aRank, NetworkInstanceId aNetID)
    {
        YPLog.Trace();
        YPLog.Log("playerUUID = " + _playerNo + ", rank = " + aRank);

        GameObject go = NetworkServer.FindLocalObject(aNetID);
        ItemBox itemBox = go.GetComponent<ItemBox>();
        itemBox.ToggleActive(false);

        GM.GiveItem(this, aRank);
    }

    internal virtual bool GetItem(Item aItem)
    {
        if (_isAI)
        {
            StartCoroutine(UseItemAuto(1f));
        }

        ++_getItemCount;
        int keyItemID = _getItemCount * 100 + aItem._itemID;
        _itemIDs.Add(keyItemID);
        _stackItems.Add(keyItemID, aItem);

        YPLog.Trace();
        YPLog.Log("itemID = " + aItem._itemID + ", keyItemID = " + keyItemID);
        if (1 == _stackItems.Count)
        {
            _currentItemID = keyItemID;
        }

        return (_stackItems.Count == 1);
    }

    private void UpdateItemIcon(int aCurrentItemID)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        //YPLog.Trace();
        //YPLog.Log("current item id = " + _currentItemID);
        //for (int index = 0; index < _itemIDs.Count; ++index)
        //{
        //    YPLog.Log("item index = " + index + ", itemID = " + _currentItemID);
        //}
        _currentItemID = aCurrentItemID;
        int[] itemKeys = new int[_itemIDs.Count];
        _itemIDs.CopyTo(itemKeys, 0);

        _hud.UpdateItemIcon(_currentItemID, itemKeys);
    }

    private IEnumerator UseItemAuto(float aDelayTime)
    {
        float delay = Random.Range(0.5f, aDelayTime);
        yield return new WaitForSeconds(delay);

        //while (Jamming)
        //{
        //    yield return null;
        //}

        Item useItem = _stackItems[_currentItemID];
        if (GM.IsTargetableItem(useItem._itemID))
        {
            CarController target = GM.GetAITarget(this);
            UseItemToTarget(target._playerNo);
        }
        else
        {
            UseItem();
            ItemMultiFire multiFire = useItem as ItemMultiFire;
            if (null != multiFire)
            {
                while (multiFire.CanFire())
                {
                    yield return new WaitForSeconds(delay / 4f);
                    //while (Jamming)
                    //{
                    //    yield return null;
                    //}

                    UseItem();
                }
            }
        }
    }

    [Command]
    public void CmdUseItem()
    {
        UseItem();
    }

    private void UseItem()
    {
        YPLog.Trace();
        YPLog.Log("current item ID = " + _currentItemID);

        if (!_stackItems.ContainsKey(_currentItemID))
        {
            YPLog.Log("never happen this!!");
            foreach (KeyValuePair<int, Item> kv in _stackItems)
            {
                YPLog.Log("item key ID = " + kv.Key);
            }

            return;
        }

        if (_stackItems[_currentItemID]._stolenByMagnet)
        {
            YPLog.Log("item is stolen by magnet now! can't use!");
            return;
        }

        _useItemID = _currentItemID;
        _stackItems[_useItemID].Use();
    }

    [Command]
    public void CmdUseItemToTarget(string aTargetPlayerUUID)
    {
        UseItemToTarget(aTargetPlayerUUID);
    }

    private void UseItemToTarget(string aTargetPlayerUUID)
    {
        if (!_stackItems.ContainsKey(_currentItemID))
        {
            YPLog.Log("never happen this!!");
            foreach (KeyValuePair<int, Item> kv in _stackItems)
            {
                YPLog.Log("item key ID = " + kv.Key);
            }

            return;
        }

        if (_stackItems[_currentItemID]._stolenByMagnet)
        {
            YPLog.Log("item is stolen by magnet now! can't use!");
            return;
        }

        CarController target = null;
        if (Constants.EMPTY_STRING == aTargetPlayerUUID)
        {
            target = GM.GetRandomTarget(this);
        }
        else
        {
            target = GM.FindCarWithPlayerUUID(aTargetPlayerUUID);
        }
        _useItemID = _currentItemID;
        _stackItems[_useItemID].UseToTarget(target);
    }

    internal void RemoveItem()
    {
        _stackItems.Remove(_useItemID);
        _itemIDs.Remove(_useItemID);

        _useItemID = -1;
        _currentItemID = -1;
    }

    internal void ForceRemoveItem()
    {
        Item item = _stackItems[_currentItemID];
        _stackItems.Remove(_currentItemID);
        _itemIDs.Remove(_currentItemID);

        _useItemID = -1;
        _currentItemID = -1;

        NetworkServer.Destroy(item.gameObject);
    }

    internal void UseItemFX(GameObject aFX, AudioClip aClip)
    {
        _fx.PlayEffectParticle(aFX);
        _sound.PlaySoundOneShot(aClip);
    }

    internal void ApplyItemFX(GameObject aFX, AudioClip aClip, float aApplyTime)
    {
        _sound.PlaySoundOneShot(aClip);
        if (0f != aApplyTime)
        {
            _fx.PlayEffectParticle(aFX, aApplyTime);
        }
        else
        {
            _fx.PlayEffectParticle(aFX);
        }
    }

    internal void GetItemFX()
    {
        _fx.PlayGetItemFX();
    }

    [ClientRpc]
    public void RpcApplyItemBonusSpeed(float aSpeed, float aTime, bool aImmediately)
    {
        if (NetworkServer.localClientActive)
        {
            return;
        }

        ApplyItemBonusSpeed(aSpeed, aTime, aImmediately);
    }

    internal void ApplyItemBonusSpeed(float aSpeed, float aTime, bool aImmediately)
    {
        //YPLog.Trace();
        //YPLog.Log("this = " + this + ", speed = " + aSpeed + ", time = " + aTime + ", imme = " + aImmediately);

        SpeedBonus bonus = new SpeedBonus();
        bonus._bonusSpeed = aSpeed;
        bonus._bonusTime = aTime;
        bonus._currentTime = 0f;

        _speedBonus.Add(bonus);
        
        if (aImmediately)
        {   
            _speed += aSpeed;
            if (0 > _speed)
            {
                _speed = 0f;
            }
        }
    }

    internal void ShowItemRange(float aRange, int aColorIndex, bool aShow)
    {
        _fx.ShowItemRangeFX(aRange, aColorIndex, aShow);
    }

    internal void StolenItemByMagnet(ItemMagnet aMagnet)
    {
        if (-1 == _currentItemID)
        {
            return;
        }

        if (!_stackItems.ContainsKey(_currentItemID))
        {
            return;
        }

        if (_stackItems[_currentItemID]._using)
        {
            return;
        }

        _useItemID = _currentItemID;
        _currentItemID = -1;

        Item stolenItem = _stackItems[_useItemID];
        stolenItem.StolenByMagnet(aMagnet);
    }

    [Command]
    public virtual void CmdCollideItem(NetworkInstanceId aNetID)
    {
        Item item = NetworkServer.FindLocalObject(aNetID).GetComponent<Item>();
        if (null != item)
        {
            item.Collide(this);
        }
    }

    [Command]
    public void CmdCollideItemFireUnit(NetworkInstanceId aNetID)
    {
        ItemFireUnit unit = NetworkServer.FindLocalObject(aNetID).GetComponent<ItemFireUnit>();
        if (null != unit)
        {
            unit.Collide(this);
        }
    }

    private void ChargeShield(bool aShield)
    {
        YPLog.Trace();

        _chargeShield = aShield;

        if (_chargeShield)
        {
            GameObject fx = AbilityDatabase.Instance.GetApplyFX(Constants.BRAND_NEW);
            _chargeShieldFX = _fx.PlayEffectParticle(fx);
        }
        else
        {
            Destroy(_chargeShieldFX);
            _chargeShieldFX = null;
        }
    }

    private void StrongWillShield(int aShield)
    {
        _strongWillShield = aShield;
        if (0 < _strongWillShield)
        {
            GameObject fx = AbilityDatabase.Instance.GetApplyFX(Constants.STRONG_WILL);
            _fx.PlayStrongWillFX(true, fx);
        }
        else
        {
            _fx.PlayStrongWillFX(false, null);
        }
    }

    internal bool DefenceDamage(ref int oDamage)
    {
        if (_sirenBoost)
        {
            return true;
        }

        // damage shield (03.Shield, 52.Fluid)
        int rest = 0;
        if (0 < _damageShield)
        {
            rest = _damageShield + oDamage;
            if (0 < rest)
            {
                _damageShield = rest;
                return true;
            }
            else
            {
                _damageShield = 0;
                oDamage = rest;

                ItemShield shieldItem = _stackItems[_currentItemID] as ItemShield;
                if (null != shieldItem)
                {
                    shieldItem.Defence();
                }
            }
        }

        // strong will        
        rest = _strongWillShield + oDamage;
        if (0 < rest)
        {
            _strongWillShield = rest;
            return true;
        }
        else
        {
            _strongWillShield = 0;
            oDamage = rest;

            return false;
        }
    }

    internal bool DefenceSpeeding()
    {
        return _speedingShield;
    }

    internal bool ShieldDefence()
    {
        if (_chargeShield)
        {
            _chargeShield = false;
            return true;
        }

        return false;
    }

    private void Jamming(bool aJamming)
    {
        YPLog.Trace();
        YPLog.Log("jamming = " + aJamming);

        _jamming = aJamming;
        _fx.PlayJammingFX(_jamming);
    }

    internal void ApplyPoison(int aDamage, int aCount, float aInterval, CarController aAttacker)
    {
        StartCoroutine(Poison(aDamage, aCount, aInterval, aAttacker));
    }

    private IEnumerator Poison(int aDamage, int aCount, float aInterval, CarController aAttacker)
    {
        YPLog.Log("this = " + this + ", attacker = " + aAttacker + ", damage = " + aDamage + ", count = " + aCount + ", interval = " + aInterval);

        for (int index = 0; index < aCount; ++index)
        {
            UpdateHP(aDamage, aAttacker, false);
            yield return new WaitForSeconds(aInterval);
        }
    }

    private void OneWay(bool aOneWay)
    {
        YPLog.Trace();
        YPLog.Log("_oneWay= " + aOneWay);

        _oneWay = aOneWay;
        _fx.PlayOneWayFX(_oneWay);
    }    

    [ClientRpc]
    public void RpcUpset(bool aStart, float aRotationSpeed)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        _camera.Upset(aStart, aRotationSpeed);
    }

    [ClientRpc]
    public void RpcPacifism(bool aShow)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        _hud.UpdateJamming(aShow);
    }

#endregion

#region play data
    private void CalcGameMoneyNExp()
    {
        if (!_playData.ContainsKey((short)Constants.StatKey.KILL))
        {
            UpdatePlayData((short)Constants.StatKey.KILL, 0);
        }

        switch (_finalRank)
        {
            case 1: _exp += TRStatic.EXP_FIRST;     _gameMoney += TRStatic.GAMEMONEY_FIRST;     break;
            case 2: _exp += TRStatic.EXP_SECOND;    _gameMoney += TRStatic.GAMEMONEY_SECOND;    break;
            case 3: _exp += TRStatic.EXP_THIRD;     _gameMoney += TRStatic.GAMEMONEY_THIRD;     break;
            case 4: _exp += TRStatic.EXP_FOURTH;    _gameMoney += TRStatic.GAMEMONEY_FOURTH;    break;
            default: break;
        }

        _gameMoney += _playData[(short)Constants.StatKey.KILL] * TRStatic.GAMEMONEY_BREAK_DOWN;
    }

    internal void UpdatePlayData(short aKey, int aValue)
    {
        if (_playData.ContainsKey(aKey))
        {
            _playData[aKey] += aValue;
        }
        else
        {
            _playData.Add(aKey, aValue);            
        }
    }

    private void UpdateRankPlayData()
    {
        if (_isAI)
        {
            return;
        }

        if (Constants.RANK_FIRST == _finalRank)
        {
            CarController second = GM.GetPlayerWithRank(Constants.RANK_SECOND);
            if (null != second)
            {
                float distance = _moveDistance - second._moveDistance;
                if (Constants.OVERWHELMING_GOLD_DISTANCE <= distance)
                {
                    UpdatePlayData((short)Constants.StatKey.OVERWHELM_FIRST, 1);
                }
            }
        }
        else if (Constants.RANK_SECOND == _finalRank)
        {
            CarController winner = GM.GetPlayerWithRank(Constants.RANK_FIRST);
            float timeDiff = _playGameTime - winner._playGameTime;
            if (Constants.NARROW_SILVER_TIME >= timeDiff)
            {
                UpdatePlayData((short)Constants.StatKey.NARROW_SECOND, 1);
            }
        }
    }

    private void SendGameResultInfo()
    {
//        if (!PSLauncherConnector.Instance.PlayServer)
//        {
//            return;
//        }
//        
//        protocol.internal_player_end_game_report_req result = new protocol.internal_player_end_game_report_req();
//        result.SetPlayerNo(System.Convert.ToUInt64(_playerNo));
//        result.SetGiveupGame(false);
//        result.SetFinalRank((sbyte)_finalRank);
//        result.SetIncGameMoney((short)_gameMoney);
//        result.SetIncPlayerExp((short)_exp);
//        foreach (KeyValuePair<short, int> kv in _playData)
//        {
//            result.SetPlayData((Constants.StatKey)kv.Key, kv.Value);
//        }
//
//        PSLauncherConnector.Instance.PlayerEndGameReportReq(result);        
    }
#endregion

#region HUD
    protected IEnumerator RegisterCarToHUD()
    {
        YPLog.Trace();
        YPLog.Log("register car to hud!!");

        while (null == _hud)
        {
            GameObject go = GameObject.Find(Constants.HUD_NAME);
            if (null != go)
            {
                //_hud = go.GetComponent<TRHUD>();
                _hud = go.GetComponent<HUD>();
            }

            yield return new WaitForSeconds(1f);
        }

        _hud.RegisterCar(this);        
    }

    internal string GetLapText()
    {
        if (_lapCount == _goalLapCount - 2)
        {
            return "Final Lap";
        }
        else if (_lapCount > _goalLapCount - 2)
        {
            return "Finish";
        }
        else
        {
            return string.Format("{0} / {1}", _lapCount + 2, _goalLapCount);
        }
    }

    internal List<int> GetCarAbilityID()
    {
        List<int> abilities = new List<int>();
        for (int index = 0; index < _carAbility.Count; ++index)
        {
            if (0 >= _carAbility[index]._id)
            {
                continue;
            }

            abilities.Add(_carAbility[index]._id);
        }

        return abilities;
    }

    internal void ChangeRank(int aRank)
    {
        if (_rank == aRank)
        {
            return;
        }

        _rank = aRank;
        //hud work
        //_hud.UpdateRank(_rank);
    }

    private void FinalRank(int aRank)
    {
        _sound.PlayFinalRankSound(aRank);
        //hud work
        //_hud.EndGame(aRank);
    }

    internal void AddDangerItem(int aLaneNO, Item aItem)
    {
        //hud work
        //_hud.AddDangerItem(aLaneNO, aItem);
    }

    internal void RemoveDangerItem(int aLaneNO, Item aItem)
    {
        //hud work
        //_hud.RemoveDangerItem(aLaneNO, aItem);
    }
    #endregion

}