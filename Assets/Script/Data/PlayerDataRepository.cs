using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;

public class PlayerDataRepository : Singleton<PlayerDataRepository>
{
    public class PlayerInfo
    {
        public Constants.LoginType LoginType;
        public long PlayerNo;
        public string Nick;
        public int NationCode;
        public int Exp;
        public int Trophy;
        public short Status;
        public int TutorialProgress;
        public int TutorialReward;
        public int LoginCount;
        public string CreatedAt;
        public string RecentLoginAt;
    }

    public class CarInfo
    {
        public int VehicleNo;
        public bool IsSelected;
        public short Level;
        public int HoldCard;
        public bool Make;

        public Dictionary<Constants.PARTS_CATEGORY, short> Parts =
            new Dictionary<Constants.PARTS_CATEGORY, short>();
    }

    public class PartsTuningStuff
    {
        public Dictionary<int, int> Stuff = new Dictionary<int, int>();
    }

    public class AbilitySlotUnlockInfo
    {
        public int UnlockLevel;
        public int DiamondPrice;
        public int GoldPrice;
    }

    public class AssetInfo
    {
        public int Gold;
        public int Diamond;
        public int GrandprixTicket;
        public long GrandprixTimestamp;
    }

    public class AbilityInfo
    {
        public int AbilityId;
        public int UnlockLevel;
        public int PriceDiamond;
        public int MaxLevel;
        public List<int> PriceGold = new List<int>();
        public int Level;
    }

    public class VehicleUpgradeInfo
    {
        public Dictionary<Constants.VehicleLevel, int> NeedCard = 
            new Dictionary<Constants.VehicleLevel, int>();
    }

    public class AchievementInfo
    {
        public int AchievementId;
        public Constants.StatKey StatKey;
        public int Goal;
        public Constants.RewardType RewardType;
        public int SpecificRewardId;
        public int RewardAmount;
        public int Progress;
        public bool IsReceiveReward;
    }
    
    public PlayerInfo MyPlayerInfo = new PlayerInfo();
    private List<CarInfo> carList_ = new List<CarInfo>();
    
    public AssetInfo MyAssetInfo = new AssetInfo();
    private List<AbilityInfo> abilities_ = new List<AbilityInfo>();
    private Dictionary<short, int> abilitySlot_ = new Dictionary<short, int>();
    private Dictionary<int, AbilitySlotUnlockInfo> abilitySlotUnlock_ = new Dictionary<int, AbilitySlotUnlockInfo>();
    private Dictionary<int, VehicleUpgradeInfo> vehicleUpgradeInfo_ = new Dictionary<int, VehicleUpgradeInfo>();

    private Dictionary<KeyValuePair<int, int>, PartsTuningStuff> partsTuningStuff_ =
        new Dictionary<KeyValuePair<int, int>, PartsTuningStuff>();
    
    private Dictionary<int, AchievementInfo> achievementInfos_ = new Dictionary<int, AchievementInfo>();
    
    public int SelectCarNo { get; set; }
    public int DisplayCarNo { get; set; }

    private Dictionary<int, int> _stuffInfo = new Dictionary<int, int>();

    public bool AIMatching { get; set; }
    public Setting.SoundVolume BGMVolume { get; set; }
    public Setting.SoundVolume EffectSoundVolume { get; set; }

    private Constants.GameMode _gameMode = Constants.GameMode.QUICK;
    public Constants.GameMode CurrentGameMode
    {
        get { return _gameMode; }
        set { _gameMode = value; }
    }

    public int VehicleCount
    {
        get { return carList_.Count; }
    }

    public Constants.TutorialProgress TutorialProgress { get; set; }

    public bool IsSetInitialData
    {
        get
        {
//            if (null == PlayerProfileInfo ||
//                null == AssetInfo ||
//                0 == _vehicles.Count ||
//                //0 == _specialAbilityList.Count ||
//                //0 == _specialAbilitySlot.Count || 
//                //0 == _achievementInfo.Count ||
//                null == AttendanceInfo ||
//                null == GrandPrixInfo)/*||
//                0 == _grandPrixLeagueRanking.Count ||
//                0 == _grandPrixTotalRanking.Count)*/
//            {
//                return false;
//            }

            return true;
        }
    }

    public void SetPlayerInfo(Dictionary<string, object> info)
    {
        int type = System.Convert.ToInt32(info["login_type"]);
        MyPlayerInfo.LoginType = (Constants.LoginType) type;
        MyPlayerInfo.PlayerNo = System.Convert.ToInt32(info["player_no"]);
        MyPlayerInfo.Nick = System.Convert.ToString(info["nick"]);
        MyPlayerInfo.NationCode = System.Convert.ToInt32(info["nation_code"]);
        MyPlayerInfo.Exp = System.Convert.ToInt32(info["exp"]);
        MyPlayerInfo.Trophy = System.Convert.ToInt32(info["trophy"]);
        MyPlayerInfo.Status = System.Convert.ToInt16(info["status"]);
        MyPlayerInfo.TutorialProgress = System.Convert.ToInt32(info["tutorial_progress"]);
        MyPlayerInfo.TutorialReward = System.Convert.ToInt32(info["tutorial_reward"]);
        MyPlayerInfo.LoginCount = System.Convert.ToInt32(info["login_count"]);
        MyPlayerInfo.CreatedAt = System.Convert.ToString(info["created_at"]);
        MyPlayerInfo.RecentLoginAt = System.Convert.ToString(info["recent_login_at"]);
    }

    public void SetAssetInfo(Dictionary<string, object> info)
    {
        MyAssetInfo.Gold = System.Convert.ToInt32(info["gold"]);
        MyAssetInfo.Diamond = System.Convert.ToInt32(info["diamond"]);
        MyAssetInfo.GrandprixTicket = System.Convert.ToInt32(info["grandprix_ticket"]);

        var stamp = System.Convert.ToString(info["grandprix_timestamp"]);
        var dt = Constants.ConvertToDateTime(stamp);
        MyAssetInfo.GrandprixTimestamp = dt.ToBinary();
    }

    public void SetAbilityInfo(Dictionary<string, object> info)
    {
        var abilityList = info["abilities"] as List<object>;
        for (int i = 0; i < abilityList.Count; i++)
        {
            var abilityUnit = abilityList[i] as Dictionary<string, object>;
            
            AbilityInfo ai = new AbilityInfo();
            ai.AbilityId = System.Convert.ToInt32(abilityUnit["ability_id"]);
            ai.UnlockLevel = System.Convert.ToInt32(abilityUnit["unlock_level"]);
            ai.PriceDiamond = System.Convert.ToInt32(abilityUnit["price_diamond"]);
            ai.MaxLevel = System.Convert.ToInt32(abilityUnit["max_level"]);
            for (int lv = 2; lv <= 5; lv++)
            {
                var key = string.Format("price_gold_lv{0}", lv);
                ai.PriceGold.Add(System.Convert.ToInt32(abilityUnit[key]));
            }
            ai.Level = System.Convert.ToInt32(abilityUnit["level"]);
            abilities_.Add(ai);
        }

        var slots = info["slots"] as Dictionary<string, object>;
        abilitySlot_[1] = System.Convert.ToInt32(slots["slot_1"]);
        abilitySlot_[2] = System.Convert.ToInt32(slots["slot_2"]);
        abilitySlot_[3] = System.Convert.ToInt32(slots["slot_3"]);
    }

    public void UpdateAbilityLevel(int abilityId, int level)
    {
        var target = abilities_.Where(x => x.AbilityId == abilityId).First();
        target.Level = level;
    }

    public void UpdateAbilitySlot(int slotNo, int abilityId)
    {
        abilitySlot_[(short)slotNo] = abilityId;
    }

    public List<AbilityInfo> GetAbilityList()
    {
        return abilities_;
    }

    public AbilityInfo GetAbilityInfo(int abilityId)
    {
        return abilities_.Find(x => x.AbilityId == abilityId);
    }

    public Dictionary<short, int> GetAbilitySlot()
    {
        return abilitySlot_;
    }

    public bool EquippedSpecialAbility(int abilityId)
    {
        return abilitySlot_.ContainsValue(abilityId);
    }

    public AbilitySlotUnlockInfo GetAbilitySlotUnlockInfo(int slotNo)
    {
        return abilitySlotUnlock_[slotNo];
    }

    public void SetAchievementInfos(List<object> infos)
    {
        achievementInfos_.Clear();
        for(int i=0;i<infos.Count;i++)
        {
            var item = infos[i] as Dictionary<string, object>;
            
            AchievementInfo ai = new AchievementInfo();
            ai.AchievementId = System.Convert.ToInt32(item["achievement_id"]);
            ai.StatKey = (Constants.StatKey) System.Convert.ToInt32(item["record_key"]);
            ai.Goal = System.Convert.ToInt32(item["goal"]);
            ai.RewardType = (Constants.RewardType) System.Convert.ToInt32(item["reward_type"]);
            ai.SpecificRewardId = System.Convert.ToInt32(item["specific_reward_id"]);
            ai.RewardAmount = System.Convert.ToInt32(item["reward_amount"]);
            ai.Progress = System.Convert.ToInt32(item["progress"]);
            ai.IsReceiveReward = System.Convert.ToBoolean(item["is_receive_reward"]);
            
            Debug.LogFormat("AchievementId:{0}, Progress:{1}", ai.AchievementId, ai.Progress);
            achievementInfos_.Add(ai.AchievementId, ai);
        }
    }

    public Dictionary<int, AchievementInfo> GetAchievementInfos()
    {
        return achievementInfos_;
    }
    
    public void SetCarList(List<object> infos)
    {
        carList_.Clear();
        for (int i = 0; i < infos.Count; i++)
        {
            var carInfoUnit = infos[i] as Dictionary<string, object>;

            CarInfo ci = new CarInfo();

            ci.VehicleNo = System.Convert.ToInt32(carInfoUnit["vehicle_no"]);
            ci.IsSelected = System.Convert.ToBoolean(carInfoUnit["is_selected"]);
            if (ci.IsSelected) this.SelectCarNo = ci.VehicleNo;
            ci.Level = System.Convert.ToInt16(carInfoUnit["level"]);
            ci.HoldCard = System.Convert.ToInt32(carInfoUnit["hold_card"]);
            ci.Make = System.Convert.ToBoolean(carInfoUnit["make"]);
            
            ci.Parts.Add(Constants.PARTS_CATEGORY.PC_MOTOR, 
                System.Convert.ToInt16(carInfoUnit["motor_lv"]));
            ci.Parts.Add(Constants.PARTS_CATEGORY.PC_BATTERY,
                System.Convert.ToInt16(carInfoUnit["battery_lv"]));
            ci.Parts.Add(Constants.PARTS_CATEGORY.PC_BODY_KIT,
                System.Convert.ToInt16(carInfoUnit["bodykit_lv"]));
            ci.Parts.Add(Constants.PARTS_CATEGORY.PC_SUSPENSION,
                System.Convert.ToInt16(carInfoUnit["suspension_lv"]));
            
            Debug.LogFormat("VehicleNo:{0}, IsSelected:{1}, Level:{2}, HoldCard:{3}",
                ci.VehicleNo, ci.IsSelected, ci.Level, ci.HoldCard);
            carList_.Add(ci);
        }
    }

    public void UpdateCarInfo(Dictionary<string, object> info)
    {
        var vehicleNo = System.Convert.ToInt32(info["vehicle_no"]);

        var idx = carList_.FindIndex(x => x.VehicleNo == vehicleNo);
        if (idx >= 0)
        {
            carList_[idx].Level = System.Convert.ToInt16(info["level"]);
            carList_[idx].HoldCard = System.Convert.ToInt32(info["hold_card"]);
            carList_[idx].Make = System.Convert.ToBoolean(info["make"]);
            
            carList_[idx].Parts[Constants.PARTS_CATEGORY.PC_MOTOR] = System.Convert.ToInt16(info["motor_lv"]);
            carList_[idx].Parts[Constants.PARTS_CATEGORY.PC_BATTERY] = System.Convert.ToInt16(info["battery_lv"]);
            carList_[idx].Parts[Constants.PARTS_CATEGORY.PC_BODY_KIT] = System.Convert.ToInt16(info["bodykit_lv"]);
            carList_[idx].Parts[Constants.PARTS_CATEGORY.PC_SUSPENSION] = System.Convert.ToInt16(info["suspension_lv"]);
        }
    }

    public CarInfo GetCarInfo(int vehicleNo)
    {
        var res = carList_.Where(x => x.VehicleNo == vehicleNo).ToList();
        if (res.Count > 0)
            return res[0];
        
        return null;
    }

    public List<CarInfo> GetCarList()
    {
        return carList_;
    }

    public int GetOwnCarCount()
    {
        return carList_.Where(x => x.Level > 0).ToList().Count;
    }

    public CarInfo GetSelectedCarInfo()
    {
        var res = carList_.Where(x => x.IsSelected).ToList();
        if (res.Count > 0)
            return res[0];    
        
        return null;
    }

    public void SetPlayerIntegratedInfo(Dictionary<string, object> info)
    {
        SetPlayerInfo(info["player_info"] as Dictionary<string, object>);
        SetCarList(info["car_info"] as List<object>);
        SetStuffInfo(info["stuff_info"] as Dictionary<string, object>);
        SetAssetInfo(info["asset_info"] as Dictionary<string, object>);
        SetAbilityInfo(info["ability_info"] as Dictionary<string, object>);
    }

    public void SetGameConfig(Dictionary<string, object> infos)
    {
        var vehicleUpgrade = infos["vehicle_upgrade"] as List<object>;

        for (int i = 0; i < vehicleUpgrade.Count; i++)
        {
            var unit = vehicleUpgrade[i] as Dictionary<string, object>;

            var grade = System.Convert.ToInt32(unit["Grade"]);
            VehicleUpgradeInfo vui = new VehicleUpgradeInfo();
            vui.NeedCard.Add(Constants.VehicleLevel.D_CLASS, System.Convert.ToInt32(unit["D"]));
            vui.NeedCard.Add(Constants.VehicleLevel.C_CLASS, System.Convert.ToInt32(unit["C"]));
            vui.NeedCard.Add(Constants.VehicleLevel.B_CLASS, System.Convert.ToInt32(unit["B"]));
            vui.NeedCard.Add(Constants.VehicleLevel.A_CLASS, System.Convert.ToInt32(unit["A"]));
            vui.NeedCard.Add(Constants.VehicleLevel.S_CLASS, System.Convert.ToInt32(unit["S"]));
            vehicleUpgradeInfo_.Add(grade, vui);
        }

        var abilitySlotUnlock = infos["ability_slot"] as List<object>;

        for (int i = 0; i < abilitySlotUnlock.Count; i++)
        {
            var unit = abilitySlotUnlock[i] as Dictionary<string, object>;
            
            AbilitySlotUnlockInfo asui = new AbilitySlotUnlockInfo();

            var slotNo = System.Convert.ToInt32(unit["slot_no"]);
            asui.UnlockLevel = System.Convert.ToInt32(unit["unlock_level"]);
            asui.DiamondPrice = System.Convert.ToInt32(unit["diamond_price"]);

            abilitySlotUnlock_.Add(slotNo, asui);
        }
    }

    public void SetPartsTuningStuffInfo(List<object> infos)
    {
        for (int i = 0; i < infos.Count; i++)
        {
            var item = infos[i] as Dictionary<string, object>;

            var keyData = item["key"] as List<object>;
            KeyValuePair<int, int> key = new KeyValuePair<int, int>(
                System.Convert.ToInt32(keyData[0]), 
                System.Convert.ToInt32(keyData[1]));
            item.Remove("key");
            
            PartsTuningStuff pts = new PartsTuningStuff();
            foreach (var kv in item)
            {
                pts.Stuff.Add(System.Convert.ToInt32(kv.Key), System.Convert.ToInt32(kv.Value));
                Debug.LogFormat("Stuff Key:{0}, Val:{1}", System.Convert.ToInt32(kv.Key), System.Convert.ToInt32(kv.Value));
            }
            partsTuningStuff_.Add(key, pts);
        }
    }

    public PartsTuningStuff GetPartsTuningStuff(Constants.PARTS_CATEGORY partsId, int partsLevel)
    {
        KeyValuePair<int, int> key = new KeyValuePair<int, int>(
            System.Convert.ToInt32(partsId), 
            partsLevel);

        return partsTuningStuff_[key];
    }

    public int GetVehicleUpgradeInfo(int grade, Constants.VehicleLevel level)
    {
        Debug.LogFormat("grade:{0}, level:{1}", grade, level);
        return vehicleUpgradeInfo_[grade].NeedCard[level];
    }
    

    void Start()
    {
        AIMatching = (PlayerPrefs.GetInt(Setting.KEY_STRING_AIMATCHING, 0) == 1) ? true : false;
        BGMVolume = (Setting.SoundVolume)PlayerPrefs.GetInt(Setting.KEY_STRING_BGM, 0);
        EffectSoundVolume = (Setting.SoundVolume)PlayerPrefs.GetInt(Setting.KEY_STRING_EFFECTSOUND, 0);

        CurrentGameMode = Constants.GameMode.QUICK;
    }

    internal void SetStuffInfo(Dictionary<string, object> aStuffInfo)
    {
        _stuffInfo.Clear();
        foreach (var kv in aStuffInfo)
        {
            _stuffInfo.Add(System.Convert.ToInt32(kv.Key), System.Convert.ToInt32(kv.Value));
        }
    }

    internal int GetStuffCount(int aID)
    {
        if (_stuffInfo.ContainsKey(aID))
        {
            return _stuffInfo[aID];
        }

        return 0;
    }

//    internal void AddVehicle(protocol.vehicle aVehicle)
//    {
//        _vehicles.Add(aVehicle);
//    }
//
//    internal void UpgradeVehicle(protocol.vehicle aVehicle)
//    {
//        protocol.vehicle target = _vehicles.Find(delegate (protocol.vehicle v) { return v.GetVehicleNo() == aVehicle.GetVehicleNo(); });
//        target.CopyFrom(aVehicle);
//    }
//
//    internal protocol.vehicle GetVehicle(int aIndex)
//    {
//        if (0 > aIndex || aIndex >= VehicleCount)
//        {
//            YPLog.LogError("_vehicles list index error!!! length = " + VehicleCount + ", index = " + aIndex);
//            return null;
//        }
//
//        return _vehicles[aIndex];
//    }
//
//    internal void SortVehicles()
//    {
//        _vehicles.Sort(Compare);
//    }
//
//    private int Compare(protocol.vehicle a, protocol.vehicle b)
//    {
//        protocol.vehicle_level levelA = a.GetLevel();
//        protocol.vehicle_level levelB = b.GetLevel();
//
//        if (protocol.vehicle_level.LOCKED == levelA)
//        {
//            if (protocol.vehicle_level.LOCKED == levelB)
//            {
//                return a.GetVehicleNo().CompareTo(b.GetVehicleNo());
//            }
//            else
//            {
//                return -1;
//            }
//        }
//        else
//        {
//            if( protocol.vehicle_level.LOCKED != levelB)
//            {
//                return a.GetVehicleNo().CompareTo(b.GetVehicleNo());
//            }
//            else
//            {
//                return -1;
//            }
//        }
//    }
//
//    internal protocol.vehicle GetSelectCarInfo()
//    {
//        foreach (protocol.vehicle v in _vehicles)
//        {
//            if (SelectCarNo == v.GetVehicleNo())
//            {
//                return v;
//            }
//        }
//
//        return null;
//    }
//
//    internal protocol.vehicle GetCarInfo(int aID)
//    {
//        foreach (protocol.vehicle v in _vehicles)
//        {
//            if (aID == v.GetVehicleNo())
//            {
//                return v;
//            }
//        }
//
//        return null;
//    }
//
//    internal void ClearGrandPrixLeagueInfo()
//    {
//        _grandPrixLeagueRanking.Clear();
//        _grandPrixTotalRanking.Clear();        
//    }
//
//    internal void AddGrandPrixLeagueInfo(protocol.grandprix_rank_type aType, protocol.grandprix_rank_unit aInfo)
//    {
//        if (protocol.grandprix_rank_type.GLOBAL == aType)
//        {
//            _grandPrixTotalRanking.Add(aInfo);
//        }
//        else
//        {
//            _grandPrixLeagueRanking.Add(aInfo);
//        }        
//    }
//
//    internal protocol.grandprix_rank_unit GetGrandPrixRankInfo(protocol.grandprix_rank_type aType, ulong aRank)
//    {
//        if (protocol.grandprix_rank_type.GLOBAL == aType)
//        {
//            return _grandPrixTotalRanking.Find(delegate (protocol.grandprix_rank_unit unit) { return unit.GetRank() == aRank; });
//        }
//        else
//        {
//            return _grandPrixTotalRanking.Find(delegate (protocol.grandprix_rank_unit unit) { return unit.GetRank() == aRank; });
//        }
//    }
//
//    internal void UpdateAchievementInfo(protocol.achievement_info aInfo)
//    {
//        protocol.achievement_info info = _achievementInfo.Find(delegate (protocol.achievement_info ai) { return ai.GetAchievementId() == aInfo.GetAchievementId(); });
//        if (null != info)
//        {
//            info.CopyFrom(aInfo);
//        }
//        else
//        {
//            _achievementInfo.Add(aInfo);
//        }
//    }
//
//    internal protocol.achievement_info GetAchievementInfo(int aIndex)
//    {
//        if( 0 > aIndex || aIndex >= AchievementCount)
//        {
//            YPLog.LogError("_achievementInfo list index error!!! length = " + AchievementCount + ", index = " + aIndex);
//            return null;
//        }
//
//        return _achievementInfo[aIndex];
//    }
//
//    internal void UpdateSpecialAbilityList(protocol.ability[] aList)
//    {
//        for (int index = 0; index < aList.Length; ++index)
//        {
//            _specialAbilityList.Add(aList[index]);
//        }
//
//        _specialAbilityList.Sort(delegate (protocol.ability left, protocol.ability right) { return left.GetAbilityId().CompareTo(right.GetAbilityId()); });
//    }
//
//    internal List<protocol.ability> GetSpecialAbilityList()
//    {
//        return _specialAbilityList;
//    }
//
//    internal protocol.ability GetSpceialAbility(int aID)
//    {
//        return _specialAbilityList.Find(delegate (protocol.ability a) { return a.GetAbilityId() == aID; });
//    }
//
//    internal bool EquippedSpecialAbility(int aID)
//    {   
//        foreach (int value in _specialAbilitySlot.Keys)
//        {
//            if (value == aID)
//            {
//                return true;
//            }
//        }
//
//        return false;
//    }
//
//    internal void UpdateSpecialAbilitySlot(Dictionary<protocol.ability_slot_no, int> aSlot)
//    {
//        _specialAbilitySlot.Clear();
//        _specialAbilitySlot = aSlot;
//    }
//
//    internal Dictionary<protocol.ability_slot_no, int> GetSpecialAbilitySlot()
//    {
//        return _specialAbilitySlot;
//    }
//
//    internal List<int> GetEquippedSpecialAbility()
//    {
//        List<int> list = new List<int>();
//        foreach (int id in _specialAbilitySlot.Values)
//        {
//            if (0 < id)
//            {
//                list.Add(id);
//            }
//        }
//
//        return list;
//    }
//
//    internal protocol.player_profile_info GetMatchPlayerProfile(ulong aPlayerNo)
//    {
//        Dictionary<ulong, protocol.player_profile_info> profiles = MatchNotify.GetProfiles();
//        if (profiles.ContainsKey(aPlayerNo))
//        {
//            return profiles[aPlayerNo];
//        }
//
//        YPLog.PlayServerLogError("[GetMatchPlayerProfile] do not match player no[" + aPlayerNo + "]");
//        YPLog.PlayServerLogError("[GetMatchPlayerProfile] ======= player no list start ============");
//        foreach (ulong playerNo in profiles.Keys)
//        {
//            YPLog.PlayServerLogError("[GetMatchPlayerProfile] player no = " + playerNo);
//        }
//        YPLog.PlayServerLogError("[GetMatchPlayerProfile] ======= player no list end ============");
//
//        return null;
//    }
//
//    internal protocol.vehicle GetMatchPlayerVehicle(ulong aPlayerNo)
//    {
//        Dictionary<ulong, protocol.vehicle> vehicles = MatchNotify.GetVehicles();
//        if (vehicles.ContainsKey(aPlayerNo))
//        {
//            return vehicles[aPlayerNo];
//        }
//
//        YPLog.PlayServerLogError("[GetMatchPlayerVehicle] do not match player no[" + aPlayerNo + "]");
//        YPLog.PlayServerLogError("[GetMatchPlayerVehicle] ======= player no list start ============");
//        foreach (ulong playerNo in vehicles.Keys)
//        {
//            YPLog.PlayServerLogError("[GetMatchPlayerVehicle] player no = " + playerNo);
//        }
//        YPLog.PlayServerLogError("[GetMatchPlayerVehicle] ======= player no list end ============");
//
//        return null;
//    }
//
//    internal protocol.ability_slot GetMatchPlayerEquippedSpecialAbility(ulong aPlayerNo)
//    {
//        Dictionary<ulong, protocol.ability_slot> specialAbility = MatchNotify.GetEquippedAbility();
//        if (specialAbility.ContainsKey(aPlayerNo))
//        {
//            return specialAbility[aPlayerNo];
//        }
//
//        YPLog.PlayServerLogError("[GetMatchPlayerEquippedSpecialAbility] do not match player no[" + aPlayerNo + "]");
//        YPLog.PlayServerLogError("[GetMatchPlayerEquippedSpecialAbility] ======= player no list start ============");
//        foreach (ulong playerNo in specialAbility.Keys)
//        {
//            YPLog.PlayServerLogError("[GetMatchPlayerEquippedSpecialAbility] player no = " + playerNo);
//        }
//        YPLog.PlayServerLogError("[GetMatchPlayerEquippedSpecialAbility] ======= player no list end ============");
//
//        return null;
//    }
//
//    internal protocol.ability GetMatchPlayerSpecialAbility(ulong aPlayerNo, int aID)
//    {
//        Dictionary<ulong, protocol.ability_list> playerAbilityList = MatchNotify.GetAbilities();
//        if (playerAbilityList.ContainsKey(aPlayerNo))
//        {
//            protocol.ability[] abilityList = playerAbilityList[aPlayerNo].GetInfos();
//            for (int index = 0; index < abilityList.Length; ++index)
//            {
//                if (aID == abilityList[index].GetAbilityId())
//                {
//                    return abilityList[index];
//                }
//            }
//
//            YPLog.PlayServerLogError("[GetMatchPlayerSpecialAbility] do not match ability id [" + aID + "]");
//            return null;
//        }
//
//        YPLog.PlayServerLogError("[GetMatchPlayerSpecialAbility] do not match player no[" + aPlayerNo + "]");
//        YPLog.PlayServerLogError("[GetMatchPlayerSpecialAbility] ======= player no list start ============");
//        foreach (ulong playerNo in playerAbilityList.Keys)
//        {
//            YPLog.PlayServerLogError("[GetMatchPlayerSpecialAbility] player no = " + playerNo);
//        }
//        YPLog.PlayServerLogError("[GetMatchPlayerSpecialAbility] ======= player no list end ============");
//
//        return null;
//    }
}