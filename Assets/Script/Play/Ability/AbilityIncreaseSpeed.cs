using UnityEngine;
using System.Collections;

public class AbilityIncreaseSpeed : Ability 
{
    internal override void ApplyAtStartGame(int aLevel, CarController aCar)
    {
        aCar._maxSpeed += TRStatic.DEFAULT_SPEED_UNIT_VALUE * _levelValue[aLevel];
    }
}