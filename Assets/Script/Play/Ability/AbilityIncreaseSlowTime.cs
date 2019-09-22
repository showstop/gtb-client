using UnityEngine;
using System.Collections;

public class AbilityIncreaseSlowTime : Ability
{
    internal override float ChangeSlowTime(int aLevel, float aTime, bool aAttack)
    {
        if (!aAttack)
        {
            return aTime;
        }

        return aTime + _levelValue[aLevel];
    }
}