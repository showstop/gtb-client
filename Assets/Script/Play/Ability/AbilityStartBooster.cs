using UnityEngine;
using System.Collections;

public class AbilityStartBooster : AbilitySpeedBoost 
{
    internal override void ApplyAtStartGame(int aLevel, CarController aCar)
    {
        StartCoroutine(DelayApply(aCar));
    }

    private IEnumerator DelayApply(CarController aCar)
    {
        while (!aCar._matchStart)
        {
            yield return null;
        }

        aCar.ApplyItemBonusSpeed(_bonusSpeed, _bonusTime, true);
        aCar.RpcApplyItemBonusSpeed(_bonusSpeed, _bonusTime, true);
    }
}