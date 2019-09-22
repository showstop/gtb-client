using UnityEngine;
using System.Collections;
//using NUnit.Framework.Constraints;

public static class TRStatic
{    
    private static GameManager        GM                          = null;
    private static TRHUD                HUD                         = null;
    private static TRCamera             TRCAMERA                    = null;
    //private static TRFrontEndManager    TRFEM                       = null;

    public static float                 DEFAULT_SPEED_BASE_VALUE    = 10f;
    public static float                 DEFAULT_SPEED_UNIT_VALUE    = 0.02f;
    public static float                 DEFAULT_ACCEL_BASE_VALUE    = 3f;
    public static float                 DEFAULT_ACCEL_UNIT_VALUE    = 0.025f;
    public static float                 DEFAULT_POWER_BASE_VALUE    = 1f;
    public static float                 DEFAULT_POWER_UNIT_VALUE    = 0.5f;

    // exp    
    public static int                   EXP_FIRST                   = 107;
    public static int                   EXP_SECOND                  = 95;
    public static int                   EXP_THIRD                   = 86;
    public static int                   EXP_FOURTH                  = 76;

    // game money    
    public static int                   GAMEMONEY_FIRST             = 583;
    public static int                   GAMEMONEY_SECOND            = 524;
    public static int                   GAMEMONEY_THIRD             = 466;
    public static int                   GAMEMONEY_FOURTH            = 408;
    public static int                   GAMEMONEY_BREAK_DOWN        = 10;

    internal static bool IsNetworkConnected()
    {
        // TODO 
        return false;
    }

    internal static void Clear()
    {
        //GM = null;
        //HUD = null;
        //TRCAMERA = null;
        //TRFEM = null;
    }

    internal static GameManager GetGameManager()
    {
        if (GM == null)
        {
            GameObject go = GameObject.FindWithTag(Constants.GAME_MANAGER_TAG_NAME);
            if (null != go)
            {
                GM = go.GetComponent<GameManager>();
            }
        }

        return GM;
    }

    internal static TRHUD GetHUD()
    {
        if (null == HUD)
        {
            GameObject go = GameObject.Find(Constants.HUD_NAME);
            //YPLog.Log(" HUD = " + go);

            if (null != go)
                HUD = go.GetComponent<TRHUD>();
        }

        return HUD;
    }

    internal static TRCamera GetTRCamera()
    {
        if (null == TRCAMERA)
        {
            GameObject goCamera = GameObject.Find(Constants.CAMERA_NAME);
            if (goCamera != null)
            {
                TRCAMERA = goCamera.GetComponent<TRCamera>();
            }
        }

        return TRCAMERA;
    }

    //internal static TRFrontEndManager GetTRFrontEndManager()
    //{
    //    if (null == TRFEM)
    //    {
    //        GameObject go = GameObject.Find(Constants.FRONT_END_MANAGER_NAME);
    //        if( null != go)
    //            TRFEM = go.GetComponent<TRFrontEndManager>();
    //    }

    //    return TRFEM;
    //}

    internal static Constants.ITEM_CATEGORY GetItemCategory(int itemNo)
    {
        return (Constants.ITEM_CATEGORY)(itemNo / 10000000);
    }

    internal static Constants.PARTS_CATEGORY GetPartsCategory(int itemNo)
    {
        return (Constants.PARTS_CATEGORY)((itemNo / 1000000) - 10);
    }

    internal static Constants.ABILITY_CATEGORY GetAbilityCatagory(int abilityNo)
    {
        return (Constants.ABILITY_CATEGORY)((abilityNo / 1000000) - 30);
    }

	internal static int GetAbilityBaseNo(int abilityNo)
	{
		return (abilityNo/10)*10;
	}

	internal static int GetAbilityLevel(int abilityNo)
	{
        int level = abilityNo % 10;
        if (0 != level)
        {
            level -= 1;
        }

        return level;
	}

    internal static int GetAbilityGrade(int abilityNo)
	{
		return (abilityNo % 100) / 10;
	}

    internal static string GetTimeString(float aTime)
    {
        if (Constants.BASE_PLAY_TIME == aTime)
            return Localization.instance.Get("@Playing");

        float displaytime = aTime * 100f;
        int minuteTime = (int)displaytime / 6000;
        displaytime -= minuteTime * 6000;
        int secondTime = (int)displaytime / 100;
        displaytime -= secondTime * 100;
        int millisecond = (int)displaytime;

        return string.Format("{0}:{1:00}.{2:00}", minuteTime, secondTime, millisecond);
    }

    internal static string GetTimeString(long aTime)
    {
        long day = aTime / (long)86400;
        aTime = aTime - ((long)86400 * day);

        long hour = aTime / (long)3600;
        aTime = aTime - ((long)3600 * hour);

        long minute = aTime / (long)60;
        long second = aTime - ((long)60 * minute);

        return string.Format(Localization.instance.Get("@ResetRankingRemainTime"), day, hour, minute, second);
    }

    /** get property name */
    internal static string GetPropertyName(Constants.property_key index)
    {
        string sBuffer = "";

        switch (index)
        {
            case Constants.property_key.ACCEL_UP:
                sBuffer = Localization.instance.Get("@Acceleration");
                break;
            case Constants.property_key.SPEED_UP:
                sBuffer = Localization.instance.Get("@Speed");
                break;
            case Constants.property_key.BOOST_UP:
                sBuffer = Localization.instance.Get("@Boost");
                break;
            case Constants.property_key.POWER_UP:
                sBuffer = Localization.instance.Get("@Stiffness");
                break;
        }
        return sBuffer;
    }

    internal static string GetItemName(int aItemID)
    {
        return Localization.instance.Get("@ItemName_" + aItemID.ToString());
    }

    internal static string GetItemDesc(int aItemID)
    {
        return Localization.instance.Get("@ItemDesc_" + aItemID.ToString());
    }

    internal static string GetAbilityName(int aItemID)
	{
        return Localization.instance.Get(string.Format("@AbilityName_{0}", GetAbilityBaseNo(aItemID)));
	}

    internal static T FindComponentInParents<T>(GameObject aGO) where T : Component
    {
        if (null == aGO)
            return null;

        T comp = aGO.GetComponent<T>();
        if (null == comp)
        {
            Transform t = aGO.transform.parent;
            while (null != t && null == comp)
            {
                comp = t.gameObject.GetComponent<T>();
                t = t.parent;
            }
        }

        return comp;
    }
}