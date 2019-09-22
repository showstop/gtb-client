using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemShield : ItemPeriod 
{
    protected override void ApplyEffect(CarController aCar)
    {
        aCar._damageShield = _damage;
    }

    protected override IEnumerator CheckApplyTime()
    {
        yield return new WaitForSeconds(_applyTime);
        RpcEndApply();
        _owner._damageShield = 0;

        // hide animation length = 0.5 sec
        yield return new WaitForSeconds(0.5f);
        _owner.RemoveItem();        
        NetworkServer.Destroy(gameObject);
    }

    internal void Defence()
    {
        _owner.RemoveItem();        
        NetworkServer.Destroy(gameObject);
    }
}