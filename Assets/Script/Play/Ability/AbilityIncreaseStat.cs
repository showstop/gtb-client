using UnityEngine;
using System.Collections;

public class AbilityIncreaseStat : Ability 
{
    internal override void ApplyAtStartGame(int aLevel, CarController aCar)
    {
        aCar._maxSpeed      += _levelValue[aLevel] * TRStatic.DEFAULT_SPEED_UNIT_VALUE;
        aCar._accelerate    += _levelValue[aLevel] * TRStatic.DEFAULT_ACCEL_UNIT_VALUE;
        aCar._power         += _levelValue[aLevel] * TRStatic.DEFAULT_POWER_UNIT_VALUE;
    }
}