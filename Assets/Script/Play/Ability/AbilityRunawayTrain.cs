using UnityEngine;
using System.Collections;

public class AbilityRunawayTrain : AbilitySpeedBoost 
{
    internal override void ApplyAtZeroHP(int aLevel, CarController aCar)
    {
        float applyTime = _bonusTime + _levelValue[aLevel];

        aCar.ActivatedRunawayTrain(applyTime);
        aCar.ApplyAbilityBonusSpeed(_bonusSpeed, applyTime, _applyFX, _applySound);
        aCar.RpcApplyAbilityBonusSpeed(_bonusSpeed, applyTime, _id);
        aCar.RpcAbilityActivated(_id, 1f, false);
    }
}