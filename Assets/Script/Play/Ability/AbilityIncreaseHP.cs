using UnityEngine;
using System.Collections;

public class AbilityIncreaseHP : Ability 
{
    internal override void ApplyAtStartGame(int aLevel, CarController aCar)
    {
        aCar._maxHP += (int)_levelValue[aLevel];
        aCar._hp += (int)_levelValue[aLevel];
    }
}