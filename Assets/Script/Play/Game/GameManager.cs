using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;

public class RegisterPlayerMessage : MessageBase
{
    public ulong _playerNo;
}

public class GameManager : NetworkManager, IEventListener
{
    [SerializeField]
    private GameObject _aiPrefab;

    [SerializeField]
    private int _goalLapCount = 0;

    [SerializeField]
    private int[] _testCarAbilityID = new int[Constants.MAX_CAR_ABILITY_SLOT_COUNT];

    [SerializeField]
    private int[] _testCarAbilityLevel = new int[Constants.MAX_CAR_ABILITY_SLOT_COUNT];
    
    [SerializeField]
    protected List<ItemBoxSpawner> _itemBoxSpawners = new List<ItemBoxSpawner>();

    [SerializeField]
    private ItemManager _itemManager;

    [SerializeField]
    private int[] _playerVehicleID = new int[Constants.MAX_PLAYER_NUM];

    [SerializeField]
    private GameObject _abilityDB;

    //private Dictionary<ulong, protocol.player_requisite_info> _playerInfo = new Dictionary<ulong, protocol.player_requisite_info>();

    private const float CONNECTION_WAIT_TIME = 10f;
    public int _matchPlayerCount = 1;
    private int _playerReadyCount = 0;

    public bool _startMatch = false;
    private int _endMatchPlayerCount = 0;

    //private protocol.internal_send_game_play_info_req _playerInfo = new protocol.internal_send_game_play_info_req();

    private Dictionary<NetworkConnection, CarController> _players = new Dictionary<NetworkConnection, CarController>();
    private CarController[] _playerRank = new CarController[Constants.MAX_PLAYER_NUM];
    private int _registerPlayerCount = 0;
    protected int _countDown = -1;

    const short PlayerMsg = 1004;
    static public RuntimePlatform platform;
    
    private List<ItemSchoolzone> _schoolZoneInfo = new List<ItemSchoolzone>();
    protected float _playerWaitTime = 3.0f;

    void Awake()
    {
#if UNITY_EDITOR
        // TODO : temp remove 
//        if ("" == PlayerDataRepository.Instance.MatchNotify.GetPsAddr().GetDomain())
//        {
//            // for editor local play..
//            StartCoroutine(AssetBundleLoader.Instance.LoadAssetBundle(_abilityDB));
//            if(TutorialConstants._tutorialPlaying)
//            {
//                StopCoroutine(AssetBundleLoader.Instance.LoadAssetBundle(_abilityDB));
//            }
//
//            NetworkManagerHUD hud = GetComponent<NetworkManagerHUD>();
//            hud.showGUI = true;
//        }
        
            StartCoroutine(AssetBundleLoader.Instance.LoadAssetBundle(_abilityDB));
        _playerVehicleID[0] = 11300300;
        
#endif
    }

    void Start()
    {
#if UNITY_EDITOR
        // TODO : temp remove 
//        if ("" != PlayerDataRepository.Instance.MatchNotify.GetPsAddr().GetDomain())
//        {
//            networkAddress = PlayerDataRepository.Instance.MatchNotify.GetPsAddr().GetDomain();
//            networkPort = PlayerDataRepository.Instance.MatchNotify.GetPsAddr().GetPort();
//#if RELEASE_IPV6_FOR_IOS
//            System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry("domain");
//            networkAddress = hostEntry.AddressList[0].ToString();
//#endif      
//
//            StartClient();
//        }
#else         
//        if (PSLauncherConnector.Instance.PlayServer)
//        {
//            EventManager.Instance.AddEventListener(this);
//            StartCoroutine(AssetBundleLoader.Instance.LoadAssetBundle(_abilityDB));
//
//            PSLauncherConnector.Instance.ConnectToPSLauncher();
//            PSLauncherConnector.Instance.SendRegisterPSReq(PSLauncherConnector.Instance.ClientListenPort);
//
//            // TO DO : set ip address
//            //networkAddress = "";
//            networkPort = PSLauncherConnector.Instance.ClientListenPort;
//            StartServer();
//        }
//        else
//        {
//            // client side
//
//            // TO DO : get network info
//            networkAddress = PlayerDataRepository.Instance.MatchNotify.GetPsAddr().GetDomain();
//            networkPort  = PlayerDataRepository.Instance.MatchNotify.GetPsAddr().GetPort();
//        
//#if RELEASE_IPV6_FOR_IOS
//            System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry("domain");
//            networkAddress = hostEntry.AddressList[0].ToString();
//#endif
//            YPLog.Log("ip = " + networkAddress + ", port = " + networkPort);
//            StartClient();
//        }
#endif
    }

    public void OnHandleEvent(GameEventType gameEventType, params System.Object[] args)
    {
        // TODO : temp remove 
//        switch (gameEventType)
//        {
//            case GameEventType.ReceivePlayerInfo:
//                Debug.Log("[GameManager::OnHandleEvent] --- ReceivePlayerInfo ---");
//
//                protocol.internal_start_game_req req = (protocol.internal_start_game_req)args[0];
//                _playerInfo = req.GetPlayerInfos();
//                Debug.Log("[GameManager::OnHandleEvent] --- ReceivePlayerInfo ---2");
//
//                //var res = StartServer();
//                //Debug.Log("[GameManager::OnHandleEvent] --- StartPlayServer --- res= " + res);
//                PSLauncherConnector.Instance.StartGameAns(1);
//
//                break;
//
//            case GameEventType.InternalPlayerEndGameReportAns:
//                protocol.internal_player_end_game_report_ans ans = (protocol.internal_player_end_game_report_ans)args[0];
//                for (int index = 0; index < _playerRank.Length; ++index)
//                {
//                    ulong playerNo = System.Convert.ToUInt64(_playerRank[index]._playerNo);
//                    if (playerNo == ans.GetPlayerNo())
//                    {
//                        _playerRank[index].SendEndMatchToClient(ans.GetFirstAcquiredStuffId(), ans.GetSecondAcquiredStuffId());
//                        break;
//                    }
//                }
//
//                break;
//
//            case GameEventType.KillProcess:
//                StartCoroutine(KillServerProcess());
//                break;
//        }
    }

    private IEnumerator KillServerProcess()
    {
        yield return new WaitForSeconds(5f);
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }

    internal bool IsTargetableItem(int aItemID)
    {
        return _itemManager.IsTargetableItem(aItemID);
    }

    public override NetworkClient StartHost()
    {
        return base.StartHost();
    }

    #region server
    public override void OnStartServer()
    {
        YPLog.Trace();

        base.OnStartServer();
        NetworkServer.RegisterHandler(PlayerMsg, OnAddPlayer);
    }

    public override void OnStartHost()
    {
        YPLog.Trace();
        YPLog.Log("server addr = " + networkAddress + ", port = " + networkPort + ", numPlayers = " + numPlayers);

        base.OnStartHost();
    }    

    public override void OnServerConnect(NetworkConnection conn)
    {
        YPLog.Trace();

        base.OnServerConnect(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        YPLog.Trace();

        base.OnServerDisconnect(conn);
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        YPLog.Trace();

        base.OnServerReady(conn);
    }

    public override void OnServerError(NetworkConnection conn, int errorCode)
    {
        YPLog.Trace();

        base.OnServerError(conn, errorCode);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        YPLog.Trace();
        YPLog.Log("player ID = " + playerControllerId + ", connection ID = " + conn.connectionId + ", connection = " + conn);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, UnityEngine.Networking.PlayerController player)
    {
        YPLog.Trace();

        base.OnServerRemovePlayer(conn, player);
    }

    public void UpdatePlayerReady()
    {
        ++_playerReadyCount;
        if (1 == _playerReadyCount)
        {
            StartCoroutine(AutoStartMatch(_playerWaitTime));
        }

        if (!_startMatch && _playerReadyCount == _matchPlayerCount)
        {
            StartCoroutine(StartMatch());
        }
    }

    private IEnumerator AutoStartMatch(float aTime)
    {
        yield return new WaitForSeconds(aTime);

        if (-1 == _countDown)
        {   
            StartCoroutine(StartMatch());
        }
    }

    private IEnumerator StartMatch()
    {
        //NetworkServer.SpawnObjects();
        AddAIPlayers();
        _countDown = 5;

        while (0 < _countDown)
        {
            foreach (KeyValuePair<NetworkConnection, CarController> kv in _players)
            {
                kv.Value.RpcCountDown(_countDown);
            }
            yield return new WaitForSeconds(1f);
            --_countDown;
        }

        _startMatch = true;
        for (int index = 0; index < _playerRank.Length; ++index)
        {
            if (null != _playerRank[index])
            {
                _playerRank[index]._matchStart = true;
                _playerRank[index].RpcStartMatch();
                StartCoroutine(_playerRank[index].DecideMove());
            }
        }        

        foreach (ItemBoxSpawner spawner in _itemBoxSpawners)
        {
            spawner.StartMatch();
        }

        //System.Array.Resize<CarController>(ref _playerRank, _players.Count);
        StartCoroutine(SortRank());
    }

    internal CarController GetPlayerCar()
    {
        CarController carPlayer = _playerRank[0];
        return carPlayer; 
    }

    internal CarController GetPlayerWithRank(int aRank)
    {
        int index = aRank - 1;
        if (index < 0 || index >= _playerRank.Length)
        {
            return null;
        }
        return _playerRank[index];
    }

    internal int PlayerEndMatch(CarController aCar)
    {
        //YPLog.PlayServerLog("============== PlayerEndMatch, aCar = " + aCar + ", endMatchPlayerCount = " + _endMatchPlayerCount);
        //YPLog.PlayServerLog("============== playerRank Length = " + _playerRank.Length);

        _playerRank[_endMatchPlayerCount] = aCar;
        ++_endMatchPlayerCount;
        
        // TODO : temp remove 
//        if (_endMatchPlayerCount == numPlayers && PSLauncherConnector.Instance.PlayServer)
//        {
//            PSLauncherConnector.Instance.EndGameReq(PSLauncherConnector.Instance.ClientListenPort);
//        }

        return _endMatchPlayerCount;
    }    

    private void OnAddPlayer(NetworkMessage aMsg)
    {
        var message = aMsg.ReadMessage<RegisterPlayerMessage>();
        if (0 == message._playerNo)
        {
            message._playerNo = (ulong)numPlayers;
        }

        var player = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        NetworkServer.AddPlayerForConnection(aMsg.conn, player, (short)0);
        
        YPLog.PlayServerLog("[OnAddPlayer] PlayerNO = " + message._playerNo + ", numPlayers = " + numPlayers + ", connection ID = " + aMsg.conn.connectionId + ", connection = " + aMsg.conn);

        CarController cc = player.GetComponent<CarController>();
        if (null != cc)
        {
            if (message._playerNo == 0)
            {
                cc.SetParts(_playerVehicleID[0]);
            }
            SetCarInfo(cc, message._playerNo, numPlayers, false);            
            _players.Add(aMsg.conn, cc);
        }
    }

    private void AddAIPlayers()
    {
        // TODO : temp remove 

        YPLog.Trace();
        YPLog.Log("match count = " + _matchPlayerCount + ", connected = " + numPlayers + ", ready Count = " + _playerReadyCount);
//        if (0 != _playerInfo.Count)
//        {
//            int aiCount = 0;
//            foreach (KeyValuePair<ulong, protocol.player_requisite_info> kv in _playerInfo)
//            {
//                bool connected = false;
//                foreach (CarController cc in _players.Values)
//                {
//                    if (cc._playerNo == kv.Value.GetProfile().GetPlayerNo().ToString())
//                    {
//                        connected = true;
//                        break;
//                    }
//                }
//
//                if (kv.Value.GetIsAi() || !connected)
//                {
//                    ++aiCount;
//                    var player = GameObject.Instantiate(_aiPrefab, Vector3.zero, Quaternion.identity) as GameObject;
//                    NetworkServer.Spawn(player);
//
//                    CarController cc = player.GetComponent<CarController>();
//                    if (null != cc)
//                    {
//                        SetCarInfo(cc, kv.Value.GetProfile().GetPlayerNo(), numPlayers + aiCount, true);
//                    }
//                }
//            }
//        }
//        else
//        {
//            for (int i = 0; i < Constants.MAX_PLAYER_NUM; i++)
//            {
//                var vehicleId = AssetBundleLoader.Instance.GetRandomCarID();
//                _playerVehicleID[i] = vehicleId;
//
//                YPLog.Log(string.Format("PlayerNo={0}, VehicleId={1}", i, vehicleId));
//            }
//
//            for (int index = numPlayers; index < _matchPlayerCount; ++index)
//            {
//                var player = GameObject.Instantiate(_aiPrefab, Vector3.zero, Quaternion.identity) as GameObject;
//                NetworkServer.Spawn(player);
//
//                CarController cc = player.GetComponent<CarController>();
//                if (null != cc)
//                {
//                    SetCarInfo(cc, (ulong)(index + 1), (index + 1), true);
//                }
//            }
//        }
    }

    internal void SetCarInfo(CarController aCar, ulong aPlayerNo, int aStartLane, bool aIsAI)
    {
        YPLog.PlayServerLog("car = " + aCar + ", playerNo = " + aPlayerNo + ", startLane = " + aStartLane + ", ai = " + aIsAI);

        aCar.GM = this;
        aCar.name = aIsAI ? string.Format("AI_{0}", aPlayerNo) : string.Format("Player_{0}", numPlayers);
        aCar._playerNo = aPlayerNo.ToString();
        aCar.SetTrackInfo(aStartLane, _goalLapCount);
        aCar._isAI = aIsAI;
        
        // TODO : temp remove 
//        if (_playerInfo.ContainsKey(aPlayerNo))
//        {   
//            protocol.vehicle car = _playerInfo[aPlayerNo].GetSelectedCar();
//            aCar.SetParts(car.GetVehicleNo());
//            YPLog.PlayServerLog("vehicle No = " + car.GetVehicleNo());
//
//            float speed = 0f;
//            float acceleration = 0f;
//            float power = 0f;
//            float hp = 0f;
//            protocol.vehicle_parts[] parts = car.GetParts();
//            for (int index = 0; index < parts.Length; ++index)
//            {
//                switch (parts[index].GetPartsId())
//                {
//                    case protocol.vehicle_parts_id.MOTOR:
//                        speed = Calculator.InGameStat(CarStat.Speed, car.GetStat().GetSpd(), car.GetLevel(), parts[index].GetLevel());
//                        break;
//
//                    case protocol.vehicle_parts_id.SUSPENSION:
//                        acceleration = Calculator.InGameStat(CarStat.Acceleration, car.GetStat().GetAcc(), car.GetLevel(), parts[index].GetLevel());
//                        break;
//
//                    case protocol.vehicle_parts_id.BODY_KIT:
//                        power = Calculator.InGameStat(CarStat.Power, car.GetStat().GetPow(), car.GetLevel(), parts[index].GetLevel());
//                        break;
//
//                    case protocol.vehicle_parts_id.BATTERY:
//                        hp = Calculator.InGameStat(CarStat.HP, car.GetStat().GetHp(), car.GetLevel(), parts[index].GetLevel());
//                        break;
//                }
//            }
//
//            Debug.Log("[SetCarInfo] speed = " + speed + ", acceleration = " + acceleration + ", power = " + power + ", hp = " + hp);
//            aCar.SetStat(speed, acceleration, power, (int)hp);
//            aCar.SetCarAbility(car.GetSkills());
//            
//            aCar.SetEquipAbility(_playerInfo[aPlayerNo].GetEquippedAbility(), _playerInfo[aPlayerNo].GetAbilities());
//            aCar.SetStartItem(_playerInfo[aPlayerNo].GetMatchItems());
//
//            aCar.ApplyStartGameAbility();   
//        }
//        else
//        {
//            if (PSLauncherConnector.Instance.PlayServer)
//            {
//                YPLog.PlayServerLogError("[SetCarInfo] do not match player no[" + aPlayerNo + "]");
//                YPLog.PlayServerLogError("[SetCarInfo] ======= player no list start ============");
//                foreach (ulong playerNo in _playerInfo.Keys)
//                {
//                    YPLog.PlayServerLogError("[SetCarInfo] player no = " + playerNo);
//                }
//                YPLog.PlayServerLogError("[SetCarInfo] ======= player no list end ============");
//            }
//            else
//            {
//                aCar.SetParts(_playerVehicleID[aStartLane-1]);
//                aCar.SetStat(Calculator.BASE_IN_GAME_SPEED, Calculator.BASE_IN_GAME_ACCELERATION, Calculator.BASE_IN_GAME_POWER, Calculator.BASE_IN_GAME_HP);
//
//                protocol.vehicle_skill[] skills = new protocol.vehicle_skill[0];
//                for (int index = 0; index < Constants.MAX_CAR_ABILITY_SLOT_COUNT; ++index)
//                {
//                    if (0 >= _testCarAbilityID[index])
//                    {
//                        continue;
//                    }
//
//                    if (0 >= _testCarAbilityLevel[index])
//                    {
//                        _testCarAbilityLevel[index] = 1;
//                    }
//
//                    protocol.vehicle_skill skill = new protocol.vehicle_skill();
//                    skill.SetSkillId(_testCarAbilityID[index]);
//                    skill.SetSkillLv((short)_testCarAbilityLevel[index]);
//
//                    System.Array.Resize(ref skills, skills.Length + 1);
//                    skills[skills.Length - 1] = skill;
//                }
//                aCar.SetCarAbility(skills);
//
//             
//                aCar.ApplyStartGameAbility();
//            }
//        }

        _playerRank[aStartLane - 1] = aCar;        
    }

    internal void GiveItem(CarController aCar, int aRank)
    {
        _itemManager.GiveItem(aCar, aRank);
    }

    internal void GiveSpecifiedItem(CarController aCar, int aItemID)
    {
        _itemManager.GiveSpecifiedItem(aCar, aItemID);
    }

    internal CarController GetRandomTarget(CarController aAttacker)
    {
        if (_playerRank.Length <= 1)
        {
            return aAttacker;
        }

        while (true)
        {
            int index = Random.Range(0, _playerRank.Length);
            if (_playerRank[index] != aAttacker)
            {
                return _playerRank[index];
            }
        }
    }

    internal CarController GetAITarget(CarController aAttacker)
    {
        for (int index = 0; index < _playerRank.Length; ++index)
        {
            if (_playerRank[index]._matchStart && !_playerRank[index]._isAI)
            {
                return _playerRank[index];
            }
        }

        return GetRandomTarget(aAttacker);
    }

    internal CarController GetNearestCar(CarController aCar)
    {
        float diff = 1f;
        CarController nearestCar = null;
        for (int index = 0; index < _playerRank.Length; ++index)
        {
            if (aCar == _playerRank[index])
            {
                continue;
            }

            float checkDiff = Mathf.Abs(_playerRank[index]._swc.TF - aCar._swc.TF);
            if (checkDiff < diff)
            {
                diff = checkDiff;
                nearestCar = _playerRank[index];
            }
        }

        return nearestCar;
    }

    internal void AddSchoolZoneInfo(ItemSchoolzone aItem)
    {
        _schoolZoneInfo.Add(aItem);
    }

    internal void RemoveSchoolZoneInfo(ItemSchoolzone aItem)
    {
        _schoolZoneInfo.Remove(aItem);
    }

    internal void CheckSchoolZone(CarController aCar, bool aLeft, int aLaneNO)
    {
        int laneNO = aCar._laneNO;
        if (aLeft)
        {
            if (laneNO != aCar._swc.Spline.StartLaneNO)
            {
                --laneNO;
            }
        }
        else
        {
            int maxLaneNO = aCar._swc.Spline.StartLaneNO + aCar._swc.Spline.LaneCount - 1;
            if (laneNO != maxLaneNO)
            {
                ++laneNO;
            }
        }

        for (int index = 0; index < _schoolZoneInfo.Count; ++index)
        {
            _schoolZoneInfo[index].ApplyEffectDuringChangeLane(aCar, laneNO);
        }
    }
#endregion

#region client
    public override void OnStartClient(NetworkClient client)
    {
        YPLog.Trace();

        base.OnStartClient(client);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        YPLog.Trace();

        base.OnClientConnect(conn);

        RegisterPlayerMessage msg = new RegisterPlayerMessage();
#if UNITY_EDITOR
        // TODO : temp remove 
//        if ("" != PlayerDataRepository.Instance.MatchNotify.GetPsAddr().GetDomain())
//        {
//            msg._playerNo = PlayerDataRepository.Instance.PlayerProfileInfo.GetPlayerNo();
//        }
#elif UNITY_IOS || UNITY_ANDROID
        msg._playerNo = (ulong)PlayerDataRepository.Instance.MyPlayerInfo.PlayerNo;
#endif
        client.Send(PlayerMsg, msg);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        YPLog.Trace();

        base.OnClientDisconnect(conn);
    }

    public override void OnClientNotReady(NetworkConnection conn)
    {
        YPLog.Trace();

        base.OnClientNotReady(conn);
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        base.OnClientError(conn, errorCode);
    }    

    internal void RegisterCar(CarController aCar)
    {
        YPLog.Trace();
        YPLog.Log("_registerPlayerCount = " + _registerPlayerCount);
        
        _playerRank[_registerPlayerCount] = aCar;
        ++_registerPlayerCount;
    }

    //internal void ClientStartMatch()
    //{
    //    //System.Array.Resize<CarController>(ref _playerRank, _registerPlayerCount);
        
    //    _startMatch = true;
    //    //StartCoroutine(SortRank());
    //}

    private IEnumerator SortRank()
    {
        //YPLog.Log("size = " + _playerRank.Length);

        while (_startMatch)
        {
            System.Array.Sort(_playerRank, delegate (CarController a, CarController b) { return a._moveDistance.CompareTo(b._moveDistance); });
            for (int index = 0; index < _playerRank.Length; ++index)
            {
                _playerRank[index].ChangeRank(_playerRank.Length - index);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    internal CarController GetCarWithRank(int aRank)
    {
        int index = aRank - 1;
        if (0 > index || index >= _playerRank.Length)
        {
            return null;
        }

        return _playerRank[index];
    }

    internal CarController FindCarWithPlayerUUID(string aPlayerUUID)
    {
        for (int index = 0; index < _playerRank.Length; ++index)
        {
            if (aPlayerUUID == _playerRank[index]._playerNo)
            {
                return _playerRank[index];
            }
        }

        return null;
    }

    internal CarController GetLocalPlayer()
    {
        for (int index = 0; index < _playerRank.Length; ++index)
        {
            if (null != _playerRank[index] && _playerRank[index].isLocalPlayer)
            {
                return _playerRank[index];
            }
        }

        return null;
    }

    internal List<CarController> GetOtherPlayers(CarController aOwn)
    {
        List<CarController> players = new List<CarController>();
        for (int index = 0; index < _playerRank.Length; ++index)
        {
            if (null == _playerRank[index])
            {
                continue;
            }
            
            if (aOwn != _playerRank[index])
            {
                players.Add(_playerRank[index]);
            }
        }

        return players;
    }

    internal CarController[] GetSortPlayers()
    {
        System.Array.Sort(_playerRank, delegate (CarController a, CarController b) { return a._finalRank.CompareTo(b._finalRank); });
        return _playerRank;
    }
    #endregion
    internal void GamePause(bool aShow)
    {
        if (aShow)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
    internal IEnumerator DelayTime()
    {
        while (TutorialConstants.TUTORIAL_ITEM_DELAYTIME > 0)
        {
            TutorialConstants.TUTORIAL_ITEM_DELAYTIME -= Time.deltaTime;
            yield return null;
        }
        TutorialManager.Instance._ingameHUD.StateSetting(TutorialConstants.IngameState);
        TutorialConstants.TUTORIAL_ITEM_DELAYTIME = 0.5f;
        GamePause(true);
    }
}