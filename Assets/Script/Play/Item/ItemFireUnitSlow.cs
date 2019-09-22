using UnityEngine;
using System.Collections;

public class ItemFireUnitSlow : ItemFireUnit 
{
    protected override void ApplyEffect(CarController aCar)
    {
        if (aCar.ShieldDefence())
        {
            return;
        }

        if (aCar.DefenceSpeeding())
        {
            return;
        }

        aCar.ApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, true);
        aCar.RpcApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, true);
    }
}