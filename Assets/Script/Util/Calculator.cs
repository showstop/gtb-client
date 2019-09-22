using UnityEngine;
using System.Collections;

// correspond with protocol.vehicle_parts_id
public enum CarStat
{
    HP = 1,
    Power,
    Speed,
    Acceleration,
};

public class Calculator : MonoBehaviour 
{
    public const float BASE_IN_GAME_SPEED = 8f;
    public const float BASE_IN_GAME_ACCELERATION = 3f;
    public const float BASE_IN_GAME_POWER = 5f;
    public const int BASE_IN_GAME_HP = 100;

    public const int BASE_UI_UNIT_SPEED = 20;
    public const int BASE_UI_UNIT_ACCELERATION = 30;
    public const int BASE_UI_UNIT_POWER = 20;
    public const int BASE_UI_UNIT_HP = 100;

    public const float UI_SPEED_UNIT_TO_IN_GAME = 0.02f;
    public const float UI_ACCELERATION_UNIT_TO_IN_GAME = 0.025f;
    public const float UI_POWER_UNIT_TO_IN_GAME = 0.05f;
    public const int UI_HP_UNIT_TO_IN_GAME = 2;
    public const int UI_HP_UPGRADE_UNIT_TO_IN_GAME = 1;

    public const short CAR_PARTS_MAX_LEVEL = 5;

    internal static int CurrentUIUnit(Constants.VehicleLevel aCarLevel, short aBasicUnit, int aBaseUnit, short aPartsLevel)
    {
        int carUpgrade = aBasicUnit * CarUpgradePercentage(aCarLevel) / 100;
        int baseIncrease = aBasicUnit - aBaseUnit;
        int partsUpgrade = baseIncrease * PartsUpgradePercentage(aPartsLevel) / 100;

        return aBasicUnit + carUpgrade + partsUpgrade;
    }

    internal static int MaxUIUnit(short aBasicUnit, int aBaseUnit)
    {
        int carUpgrade = aBasicUnit * CarUpgradePercentage(Constants.VehicleLevel.S_CLASS) / 100;
        int baseIncrease = aBasicUnit - aBaseUnit;
        int partsUpgrade = baseIncrease * PartsUpgradePercentage(CAR_PARTS_MAX_LEVEL) / 100;

        return aBasicUnit + carUpgrade + partsUpgrade;
    }

    internal static int CarUpgradePercentage(Constants.VehicleLevel aCarLevel)
    {
        switch (aCarLevel)
        {
            case Constants.VehicleLevel.D_CLASS: return 0;
            case Constants.VehicleLevel.C_CLASS: return 10;
            case Constants.VehicleLevel.B_CLASS: return 20;
            case Constants.VehicleLevel.A_CLASS: return 35;
            case Constants.VehicleLevel.S_CLASS: return 50;            
        }

        return 0;
    }

    internal static int PartsUpgradePercentage(short aPartsLevel)
    {
        switch (aPartsLevel)
        {
            case 1: return 100;
            case 2: return 150;
            case 3: return 200;
            case 4: return 250;
            case 5: return 300;
        }

        return 100;
    }

    internal static float InGameStat(CarStat aType, short aBasicUIStat, Constants.VehicleLevel aCarLevel, short aPartsLevel)
    {
        int carUpgrade = 0;
        int baseIncrease = 0;
        int partsUpgrade = 0;

        Debug.Log("[InGameStat] type = " + aType + ", basic = " + aBasicUIStat + ", carLevel = " + aCarLevel + ", partsLevel = " + aPartsLevel);
        switch (aType)
        {
            case CarStat.Speed:
                carUpgrade = aBasicUIStat * CarUpgradePercentage(aCarLevel) / 100;
                baseIncrease = aBasicUIStat - BASE_UI_UNIT_SPEED;
                partsUpgrade = baseIncrease * PartsUpgradePercentage(aPartsLevel) / 100;

                return BASE_IN_GAME_SPEED + ((float)(carUpgrade + baseIncrease + partsUpgrade) * UI_SPEED_UNIT_TO_IN_GAME);

            case CarStat.Acceleration:
                carUpgrade = aBasicUIStat * CarUpgradePercentage(aCarLevel) / 100;
                baseIncrease = aBasicUIStat - BASE_UI_UNIT_ACCELERATION;
                partsUpgrade = baseIncrease * PartsUpgradePercentage(aPartsLevel) / 100;

                return BASE_IN_GAME_ACCELERATION + ((float)(carUpgrade + baseIncrease + partsUpgrade) * UI_ACCELERATION_UNIT_TO_IN_GAME);

            case CarStat.Power:
                carUpgrade = aBasicUIStat * CarUpgradePercentage(aCarLevel) / 100;
                baseIncrease = aBasicUIStat - BASE_UI_UNIT_POWER;
                partsUpgrade = baseIncrease * PartsUpgradePercentage(aPartsLevel) / 100;

                return BASE_IN_GAME_POWER + ((float)(carUpgrade + baseIncrease + partsUpgrade) * UI_POWER_UNIT_TO_IN_GAME);

            case CarStat.HP:
                carUpgrade = aBasicUIStat * CarUpgradePercentage(aCarLevel) / 100;
                baseIncrease = aBasicUIStat - BASE_UI_UNIT_HP;
                partsUpgrade = baseIncrease * PartsUpgradePercentage(aPartsLevel) / 100;

                return (float)BASE_IN_GAME_HP + ((float)baseIncrease * UI_HP_UNIT_TO_IN_GAME) + ((float)(carUpgrade + partsUpgrade) * UI_HP_UPGRADE_UNIT_TO_IN_GAME);
        }

        return 0f;
    }
}