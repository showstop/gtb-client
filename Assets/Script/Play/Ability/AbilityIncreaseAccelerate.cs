using UnityEngine;
using System.Collections;

public class AbilityIncreaseAccelerate : Ability 
{
    internal override void ApplyAtStartGame(int aLevel, CarController aCar)
    {
        aCar._accelerate += TRStatic.DEFAULT_ACCEL_UNIT_VALUE * _levelValue[aLevel];
    }
}