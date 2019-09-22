using UnityEngine;
using System.Collections;

public class AbilityIncreaseExp : Ability 
{
    internal override void ApplyAtEndGame(int aLevel, CarController aCar)
    {
        aCar._exp += (int)((float)aCar._exp * _levelValue[aLevel]);
    }
}