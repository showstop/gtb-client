using UnityEngine;
using System.Collections;

public class AbilityDecreaseCollideDamage : Ability 
{
    internal override void ChangeDamage(int aLevel, ref int oDamage, bool aCollide, bool aAttack, CarController aCar)
    {
        if (!aCollide)
        {
            return;
        }

        if (aAttack)
        {
            return;
        }

        oDamage += (int)_levelValue[aLevel];
    }
}