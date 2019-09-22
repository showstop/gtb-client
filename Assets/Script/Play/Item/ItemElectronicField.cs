using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemElectronicField : ItemPermanent
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

    internal override void Enhance(float aValue)
    {
        _bonusSpeedApplyTime = aValue;
    }
}
