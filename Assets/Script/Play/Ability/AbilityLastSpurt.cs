using UnityEngine;
using System.Collections;

public class AbilityLastSpurt : AbilitySpeedBoost 
{
    internal override void ApplyAtLapCount(int aLevel, CarController aCar) 
    {
        if (aCar._goalLapCount != aCar._lapCount + 1)
        {
            return;
        }
    
        aCar.ApplyAbilityBonusSpeed(_bonusSpeed, _bonusTime, _applyFX, _applySound);
    }
}