using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemSirenBoost : ItemPeriod 
{
    protected override IEnumerator CheckApplyTime()
    {
        yield return new WaitForSeconds(_applyTime);
        _owner._sirenBoost = false;
        RpcEndApply();        

        // hide animation length = 0.5 sec
        yield return new WaitForSeconds(0.5f);
        _owner.RemoveItem();
        NetworkServer.Destroy(gameObject);
    }

    protected override void ApplyEffect(CarController aCar)
    {
        aCar._sirenBoost = true;

        aCar.ApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
        aCar.RpcApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
    }

    internal override void Enhance(float aValue)
    {
        _applyTime = aValue;
        _bonusSpeedApplyTime = aValue;
    }
}