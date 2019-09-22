using UnityEngine;
using System.Collections;

public class AbilityDangerAlarm : Ability 
{
    internal override void ApplyAtStartGame(int aLevel, CarController aCar)
    {
        aCar._dangerAlarm = true;
    }
}