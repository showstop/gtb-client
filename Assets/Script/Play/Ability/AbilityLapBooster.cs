using UnityEngine;
using System.Collections;

public class AbilityLapBooster : AbilitySpeedBoost 
{
    internal override void ApplyAtLapCount(int aLevel, CarController aCar)
    {   
        aCar.ApplyAbilityBonusSpeed(_bonusSpeed + _levelValue[aLevel], _bonusTime, _applyFX, _applySound);
    }
}