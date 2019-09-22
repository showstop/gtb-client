using UnityEngine;
using System.Collections;

public class AbilityIncreasePower : Ability 
{
    internal override void ApplyAtStartGame(int aLevel, CarController aCar)
    {
        aCar._power += TRStatic.DEFAULT_POWER_UNIT_VALUE * _levelValue[aLevel];
    }
}