using UnityEngine;
using System.Collections;

public class AbilityRewind : Ability
{
    internal override void ApplyAtZeroHP(int aLevel, CarController aCar)
    {   
        aCar.RpcAbilityActivated(_id, 1f, true);

        aCar._rewindActivated = true;
        aCar._rewindPercentage = _levelValue[aLevel];
    }
}