using UnityEngine;
using System.Collections;

public class AbilityIncreaseGameMoney : Ability
{
    internal override void ApplyAtEndGame(int aLevel, CarController aCar)
    {
        aCar._gameMoney += (int)((float)aCar._gameMoney * _levelValue[aLevel]);
    }
}