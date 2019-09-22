using UnityEngine;
using System.Collections;

public class AbilityStrongWill : Ability 
{
    internal override void ApplyAtZeroHP(int aLevel, CarController aCar)
    {   
        aCar.RpcAbilityActivated(_id, 1f, false);
        aCar._strongWillShield = (int)_levelValue[aLevel];
    }
}