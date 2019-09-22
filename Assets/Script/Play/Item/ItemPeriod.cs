using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemPeriod : Item
{
    [SerializeField]
    protected float _applyTime;

    internal override void Use()
    {
        StartCoroutine(CheckApplyTime());
        base.Use();
        ApplyEffect(_owner);
    }

    protected override void CommonUse()
    {
        _animation.Play(_useAnimName);

        ToggleItemFX(true);
        _owner.UseItemFX(_useItemFX, _useItemSound);
    }

    protected virtual IEnumerator CheckApplyTime()
    {
        yield return new WaitForSeconds(_applyTime);
        RpcEndApply();

        // hide animation length = 0.5 sec
        yield return new WaitForSeconds(0.5f);
        _owner.RemoveItem();
        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    public virtual void RpcEndApply()
    {
        ToggleItemFX(false);
        _animation.Play(_hideAnimName);
    }

    protected override void ApplyEffect(CarController aCar)
    {
        aCar.ApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
        aCar.RpcApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
    }

    internal override void Enhance(float aValue)
    {
        _applyTime = aValue;
    }
}