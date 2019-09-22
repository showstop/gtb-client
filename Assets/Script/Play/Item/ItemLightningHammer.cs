using UnityEngine;
using System.Collections;

public class ItemLightningHammer : ItemHammer 
{
    protected override void ApplyEffect(CarController aCar)
    {
        if (aCar.ShieldDefence())
        {
            return;
        }

        int damage = -_damage;
        aCar.DefenceDamage(ref damage);
        aCar.UpdateHP(damage, _owner, false);

        if (!aCar.DefenceSpeeding())
        {
            aCar.ApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
            aCar.RpcApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
        }
    }

    internal override void Enhance(float aValue)
    {
        _damage = (int)aValue;
    }
}