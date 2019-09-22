using System;
using UnityEngine;
using System.Collections;

public class Constants
{
#region Game_Version
    public const short GAME_VERSION = 1819;
#endregion

#region MembershipPartner_Site
    public const string MEMBERSHIP_PARTNER_SITE = "http://cafe.naver.com/ypgogosing";
    #endregion

    #region car   

    public const int SUPARK_CAR_ID = 11200100;

    public const string LAYER_NAME_CAR = "Car";
    #endregion    

    #region localization
    public const string LOCALIZATION_KEY_GLOSSARY_LEVEL = "Glossary.Level";
    public const string LOCALIZATION_KEY_CAR_SPECIALABILITY_SHOULDUPGRADE = "Car.SpecialAbility.ShouldUpgrade";
    #endregion

    #region item
    // item property
	public enum property_key
	{
		GRADE = 1,
		PRICE_TYPE,
		GOLD_PRICE,
		CASH_PRICE,
		LIFE_TYPE,

		ACCEL_UP,
		SPEED_UP,
		BOOST_UP,		// not used!!
		POWER_UP,
		HP_UP,
		
		PROP_POINT,
		REQUIRED_LEVEL,
        UNLOCK_KEY,
        UNLOCK_VALUE,
        ABILITY_NO,			// todo change to ABILITY_NO_1
		ABILITY_NO_2,
		ABILITY_NO_3,
		
		RESELL_PRICE = 51,
		ABILITY_COUNT,
        EXCLUSION_ID,
		
		STRENGTHEN_LEVEL = 201,
		
		PARTS_LV_UP_GOLD_PRICE_1 = 211,
		PARTS_LV_UP_GOLD_PRICE_2,
		PARTS_LV_UP_GOLD_PRICE_3,
		PARTS_LV_UP_GOLD_PRICE_4,
		PARTS_LV_UP_GOLD_PRICE_5,

		CHAR_LV_UP_GOLD_PRICE_1 = 231,
		CHAR_LV_UP_GOLD_PRICE_2,
		CHAR_LV_UP_GOLD_PRICE_3,
		CHAR_LV_UP_GOLD_PRICE_4,
		CHAR_LV_UP_GOLD_PRICE_5,

		CHAL_LV_UP_GOLD_PERCENT_1 = 261,
		CHAL_LV_UP_GOLD_PERCENT_2,
		CHAL_LV_UP_GOLD_PERCENT_3,
		CHAL_LV_UP_GOLD_PERCENT_4,
		CHAL_LV_UP_GOLD_PERCENT_5,

		CHAR_LV_UP_CASH_PRICE_1 = 281,
		CHAR_LV_UP_CASH_PRICE_2,
		CHAR_LV_UP_CASH_PRICE_3,
		CHAR_LV_UP_CASH_PRICE_4,
		CHAR_LV_UP_CASH_PRICE_5,

		SALES = 999,
		BEGIN_DYNAMIC_PROPERTY = 1000,

		LEVEL,
		REMAIN_COUNT,
	};

    // item category
    public enum ITEM_CATEGORY
    {
        IC_PARTS = 1,
        IC_ABILITY = 3,
    }
	
	public const int 			TIRE_PARTS_NO_PADDING				= 3000000;
	public const string 		INVALID_PLAYER_UUID					= "";
    // in-game item
    public const int            ITEM_ID_SHIELD                      = 11;
    public const int            ITEM_ID_SPEEDBOOST                  = 8;
    public const int            ITEM_ID_SLOWHAMMER                  = 20;
    public const int            ITEM_ID_LIGHTNINGHAMMER             = 29;
    public const int            BASE_STACK_ITEM_COUNT               = 1;

    // start item
    public const int            MAX_START_ITEM_COUNT                = 4;
#endregion

#region name
    // gameObject name
	public const string 		WAIT_SERVER_RESPONSE_OBJECT 		= "WaitServerResponse";
    public const string         GAME_MANAGER_TAG_NAME               = "GameManager";
    public const string         START_SPLINE_TAG_NAME               = "StartSpline";
    public const string         DEBUG_LOG_TAG_NAME                  = "[Debug] ";
    public const string         WARNING_LOG_TAG_NAME                = "[Warning] ";
    public const string         ERROR_LOG_TAG_NAME                  = "[Error] ";
    public const string         HUD_NAME                            = "HUD";
    public const string         UI_ROOT_NAME                        = "UI Root (2D)";
    public const string         HUD_CENTER_EFFECT_GO_NAME           = "CenterHUDEffect";
    public const string         CAMERA_NAME                         = "Camera";
    public const string         CAR_AVATAR_CAMERA_NAME              = "CarAvator_Camera";
    public const string         FRONT_END_MANAGER_NAME              = "FrontEndManager";
    public const string         ITEM_BOX_SPAWN_MANAGER_NAME         = "TRItemBoxSpawnManager";
    public const string         JUMP_Trigger                        = "JumpTrigger";

    // scene name
    public const string         SCENE_NAME_LOGO_COMPANY             = "scene_logo";
    public const string         SCENE_NAME_GAME_TITLE_FOR_LOCAL     = "scene_gametitle_for_local";
    public const string         SCENE_NAME_GAME_TITLE_FOR_FACEBOOK  = "scene_gametitle_for_facebook";
    public const string         SCENE_NAME_GAME_TITLE_FOR_KAKAO     = "scene_gametitle_for_kakao";
	public const string 		SCENE_NAME_GAME_TITLE_FOR_BAND		= "scene_gametitle_for_band";
    public const string         SCENE_NAME_MAIN_MENU                = "scene_mainmenu";
    public const string         SCENE_NAME_HUD                      = "scene_hud";
    public const string         SCENE_NAME_LOADING                  = "scene_loading";
    public const string         SCENE_NAME_INGAME_PREFIX            = "scene_map_";

    // facebook URL
    public const string         FACEBOOK_PROFILE_PIC_URL_FORMAT     = "http://graph.facebook.com/{0}/picture";
    public const string         FACEBOOK_PROFILE_NAME_URL_FORMAT    = "http://graph.facebook.com/{0}/profile";

    // animation name
    public const string         CAR_MOVE_LEFT_ANIMATION_NAME        = "car_move_left";
    public const string         CAR_MOVE_RIGHT_ANIMATION_NAME       = "car_move_right";

    public const string         ANI_MOVE_IDLE_ANIMATION_NAME        = "Ani_Idle";
    public const string         ANI_MOVE_LEFT_ANIMATION_NAME        = "Ani_move_left";
    public const string         ANI_MOVE_RIGHT_ANIMATION_NAME       = "Ani_move_right";

    // prefab path
    public const string         PREFAB_PATH_PLAYER_CAR              = "Prefabs/Car/PlayerCar";
    public const string         PREFAB_PATH_PLAYER_CAR_RENDER       = "Prefabs/Car/CarAvator";
    public const string         PREFAB_PATH_PLAY_ITEM_MANAGER       = "Prefabs/Item/TRPlayItemManager";
    public const string         PREFAB_PATH_ABILITY_DATABASE        = "Prefabs/Ability/TRAbilityDatabase";
    public const string         PREFAB_PATH_ABILITY                 = "Prefabs/Ability/Abilities";
    public const string         PREFAB_PATH_BOOST_DECAL             = "Prefabs/LevelTrigger/BoostDecal";

    public const string         EMPTY_STRING                        = "";
#endregion

#region game play
    // jump
    public enum JumpState
    {
        None,
        GoingUp,
        GoingDown,
    }
    public const float          JUMP_START_VELOCITY                 = 2.5f;
    public const float          SLOW_HAMMER_JUMP_START_VELOCITY     = 2f;

    // lane
    public enum MoveLaneState
    {
        Stay,
        MoveLeft,
        MoveLeftBack,
        MoveRight,
        MoveRightBack,
    }
    public const int            MIN_LANE_NO                         = 1;
    //public const int        MID_LANE_NO         = 3;
    public const int            MAX_LANE_NO                         = 4;
    public const float          DEFAULT_LANE_WIDTH                  = 0.5f;
    public const float          MOVE_LANE_START_VELOCITY            = 1f;
    public const float          MOVE_LANE_VELOCITY_CHANGE_RATIO     = 2f;

    // player number
    public const int            MAX_PLAYER_NUM                      = 4;

    // collide
    public const float          CAR_COLLIDE_SPEED_PENALTY_RATIO             = -0.2f;
    public const float          OBSTACLE_CAR_COLLIDE_SPEED_PENALTY_RATIO    = -0.5f;
    public const float          COLLIDE_BONUS_MAX_TIME                      = 1f;

    // count down
    public const byte           COUNT_DOWN_MAX                      = 7;

    // decal
    public const byte           DECAL_COUNT_MAX                     = 15;

    // decelerate
    public const float          ACCELERATE                          = 20f;
    public const float          DECELERATE                          = -20f;

    // hp & damage
    public const int            MAX_DAMAGE                          = 100;
    public const float          EXPLOSION_TIME                      = 3f;
    public const int            BASE_HP                             = 100;

    // adjust for sync
    public const float          ADJUST_LOCATION_INTEVAL             = 0.2f;
    public const float          ADJUST_LOCATION_RATIO               = 0.1f;

    // play time
    public const float          BASE_PLAY_TIME                      = 10000000f;

    // play data
    public const int            RANK_FIRST                          = 1;
    public const int            RANK_SECOND                         = 2;
    public const float          OVERWHELMING_GOLD_DISTANCE          = 30f;
    public const float          NARROW_SILVER_TIME                  = 0.5f;

	public const int 			MAX_TIP_DESCRIPTION_COUNT 			= 28;
#endregion

#region front end

	private const int MAX_PLAYER_LEVEL = 100;

	public static int GetVehicleGrade(int vehicleNo)
	{
		var temp = System.Convert.ToString(vehicleNo);
		temp = temp.Substring(2, 1);
		var grade = System.Convert.ToInt32(temp);
		return grade;
	}

	public static int GetNeedExpByLevel(int level)
	{
		return level * (level - 1) * 97;
	}

	public static int GetPlayerLevelByExp(int exp)
	{
		for (int lv = 2; lv < MAX_PLAYER_LEVEL; lv++)
		{
			var needExp = GetNeedExpByLevel(lv);
			if (exp < needExp)
				return (lv-1);
		}

		return 1;
	}

	public static float GetPlayerExpRatio(int exp)
	{
		var curLevel = GetPlayerLevelByExp(exp);
		var curLvNeedExp = GetNeedExpByLevel(curLevel);
		var nextLvNeedExp = GetNeedExpByLevel(curLevel + 1);

		return (float)(exp - curLvNeedExp) / (float)(nextLvNeedExp - curLvNeedExp);
	}

	public enum LoginType
	{
		GUEST = 1,
		GOOGLE,
		GAMECENTER,
		FACEBOOK,
		
	}
	
    public enum TutorialProgress
    {
        INITIALIZE = 0,
        CONTROL_TUTORIAL_BEGIN,
        CONTROL_TUTORIAL_END,
        LOBBY_TUTORIAL_BEGIN,
        LOBBY_TUTORIAL_END,
        ALL_COMPLETE ,
    };	// enum TutorialProgress

    public enum GameMode
    {
        QUICK = 1,
        GRANDPRIX = 2,
    };	// enum GameMode

    public enum RewardType
    {
        GOLD = 1,
        DIAMOND = 2,
        QUICK_MATCH_TICKET = 3,
        GRANDPRIX_TICKET = 4,
        VEHICLE_SPECIFIC_CARD = 10,
        VEHICLE_SELECT_CARD = 11,
        VEHICLE_PICK_CARD = 12,
        STUFF = 13,
    };	// enum RewardType

    public enum StatKey
    {
        LOGIN_COUNT = 101,
        FACEBOOK_POSTING = 102,
        FACEBOOK_LIKE = 103,
        FACEBOOK_FRIEND_INVITE = 104,
        ACHIEVEMENT_COUNT = 105,
        ACQ_GOLD_MEDAL = 1001,
        ACQ_SILVER_MEDAL = 1002,
        ACQ_BRONZE_MEDAL = 1003,
        RUNNING_DISTANCE = 1004,
        ACQ_GAME_MONEY = 1006,
        PLAY_COUNT = 1007,
        USE_ITEM_COUNT = 1008,
        KNOCK_COUNT = 1009,
        OVERWHELM_FIRST = 1010,
        NARROW_SECOND = 1011,
        CONTINUOUS_FIRST = 1012,
        CONTINUOUS_LAST = 1013,
        NO_DEATH_GAME = 1014,
        FRIEND_COUNT = 1015,
        COLLECT_ACHIEVEMENT = 1016,
        BUMP_ATTACK = 1017,
        DEATH = 1018,
        KILL = 1019,
        TOTAL_PLAY_SEC = 1020,
        USE_INGAME_ITEM = 1021,
        ACQ_INGAME_ITEM = 1022,
        QUICK_MATCH_PLAY_COUNT = 1023,
        QUICK_MATCH_PLAY_SEC = 1024,
        GRANDPRIX_PLAY_COUNT = 1025,
        GRANDPRIX_PLAY_SEC = 1026,
        UPGRADE_VEHICLE = 1027,
        PARTS_TUNING = 1028,
        ACQ_NORMAL_STUFF = 1029,
        ACQ_RARE_STUFF = 1030,
        USE_GACHA_COUNT = 1031,
        OPEN_LUCKY_BOX = 1032,
        WATCH_VIDEO_AD_COUNT = 1033,
        DEAL_DAMAGE = 1034,
        TAKE_DAMAGE = 1035,
        DEFENSE_USING_BRAND_NEW = 11101,
        TIME_REWIND = 11102,
        USE_FROST = 11103,
        USE_JACK_BOX = 11104,
        USE_EMERGENCY = 11105,
        USE_ARTISAN_HANDS = 11106,
        USE_SIREN_BOOST = 11107,
        USE_WATER_BOMB = 11108,
        USE_DESTROYER = 11109,
        ONE_SHOT_KILL = 11110,
        USE_BARRICADE = 11111,
        USE_RUN_AWAY = 11112,
        DAMAGE_CHAINING_USING_LIGHTENING_CHAIN = 11113,
        DEFENSE_USING_SPEEDING = 11114,
    };	// enum record_data_key

	public enum VehicleLevel
	{
		LOCKED = 0,
		D_CLASS = 1,
		C_CLASS,
		B_CLASS,
		A_CLASS,
		S_CLASS,
	}
	
    public enum player_state
    {
        PS_OFFLINE = 0,
        PS_ONLINE = 1,
        PS_PLAYING = 2,
    };

    public enum mail_type
    {
        EVENT = 1,
        GAME_TICKET = 2,
        GIFT = 3,
        REWARD = 4,
    };

    public enum buy_type
    {
        GOLD = 1,
        CASH = 2,
    };
	
	public enum life_type
	{
		INFINITE = 0,
		COUNT = 1,
	};

    public enum change_type
    {
        AUTO = 0,
        MANUAL = 1,
    };

	public static DateTime ConvertToDateTime(string str)
	{
		var dtInfo = str.Split('T');
		var dateUnits = dtInfo[0].Split('-');
		var timeUnits = dtInfo[1].Split(':');

		DateTime dt = new DateTime(
			Convert.ToInt32(dateUnits[0]),
			Convert.ToInt32(dateUnits[1]),
			Convert.ToInt32(dateUnits[2]),
			Convert.ToInt32(timeUnits[0]),
			Convert.ToInt32(timeUnits[1]),
			Convert.ToInt32(timeUnits[2])
			);
		return dt;
	}

    // parts category
    public enum PARTS_CATEGORY
    {
	    PC_MOTOR = 1,
	    PC_BATTERY,
        PC_BODY_KIT,
        PC_SUSPENSION,
    }

    public enum char_slot
    {
        CHAR_SLOT_1 = 5,
        CHAR_SLOT_2,
        CHAR_SLOT_3,
        CHAR_SLOT_4,
        CHAR_SLOT_5,
        CHAR_SLOT_6,
		
    };

    public enum ABILITY_CATEGORY
    {
        PASSIVE = 1,
        ACTIVE = 2,
		FUNCTIONAL = 3,
    };

    // currently don't use.
	public enum ABILITY_GRADE
	{   
        GRADE_NONE = 0,
		GRADE_C,
		GRADE_B,
		GRADE_A,
		GRADE_S,
	};

    public enum AbilitySummaryInfo
    {
        // +{0}
        Speed,
        Accelerate,
        Power,
        HP,
        ItemDamage,
        IncreaseCollideDamage,
        DecreaseCollideDamage,
        Stacker,
        // {00.00}
        RecoverHPThroughMoveDistance,
        GetGameMoneyThroughMoveDistance,
        // {0}%
        IncreaseSlowBonusTime,
        DecreaseSlowBonusTime,        
        IncreaseEXP,
        DrcreaseHPRecoveryTime,
        MoveLaneSpeed,
        MAX,
    };

    // ability
    public const int                MAX_CAR_ABILITY_SLOT_COUNT          = 3;
    public const int                MAX_EQUIP_ABILITY_SLOT_COUNT        = 6;
    public const int                BRAND_NEW                           = 32000030;
    public const int                STRONG_WILL                         = 32000100;

    public const int                ABILITY_BASE_ID                     = 30000000;
    public const int                ABILITY_SLOW_FIELD_ID               = 32100600;    
    public const int                STARTITEM_BASE_ID                   = 33000000;
    

    // game ticket
    public const sbyte              DEFAULT_GAME_TICKET_COUNT           = 5;
    public const sbyte              MAX_GAME_TICKET_COUNT               = 100;
    public const int                ISSUE_GAME_TICKET_TIME              = 60 * 10;		// 10 mins

    public const int                DEFAULT_WAITING_SEC_INGAME_ITEM_POPUP = 5;

    // status
    public const int                MAX_STATUS_NUM                      = 3;

    // level up
    public const int                MAX_LEVEL_UP                        = 5;

    // login retry count
    public const int                MAX_LOGIN_RETRY_COUNT               = 5;

    // sticker
    public const int                STICKER_BASE_ID                     = 12000000;
    public const int                STICKER_EMPTY_ID                    = 12100000;
    public const int                STICKER_PLAY_COUNT_FIVE_ID          = 12101500;

    public const int                DIAMOND_TO_KRW		                = 100;
    public const int                DIAMOND_TO_USD		                = 10;
    public const int                DIAMOND_TO_GOLD                     = 500;
    public const int                DIAMOND_TO_BATTERY                  = 2;

    // enchant
    public const int                ENCHANT_PRICE_RATIO_GRADE_C         = 14;
    public const int                ENCHANT_PRICE_RATIO_GRADE_B         = 15;
    public const int                ENCHANT_PRICE_RATIO_GRADE_A         = 16;
    public const int                MAX_ENCHANT_LEVEL                   = 6;

    // second slot open
    public const int                SECOND_ABILITY_SLOT_FAKE_ID         = 10000002;
#endregion
}