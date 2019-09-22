using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityWaterBomb : AbilitySpeedBoost
{
    [SerializeField]
    private float _range;

    internal override void ApplyAtZeroHP(int aLevel, CarController aCar)
    {
        aCar.RpcAbilityActivated(_id, 1f, true);

        List<CarController> others = aCar.GM.GetOtherPlayers(aCar);
        for (int index = 0; index < others.Count; ++index)
        {
            float dist = Vector3.Distance(aCar.CarTransform.position, others[index].CarTransform.position);
            YPLog.Log("dist = " + dist);
            if (dist > _range)
            {
                continue;
            }

            others[index].ApplyAbilityBonusSpeed(_bonusSpeed, _bonusTime + _levelValue[aLevel], null, null);
            others[index].RpcApplyAbilityBonusSpeed(_bonusSpeed, _bonusTime + _levelValue[aLevel], 0);
        }
    }
}
