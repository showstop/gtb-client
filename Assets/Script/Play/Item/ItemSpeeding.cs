using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemSpeeding : ItemPeriod
{
    protected override void ApplyEffect(CarController aCar)
    {
        aCar._speedingShield = true;
    }

    protected override IEnumerator CheckApplyTime()
    {
        yield return new WaitForSeconds(_applyTime);
        RpcEndApply();
        _owner._speedingShield = false;

        // hide animation length = 0.5 sec
        yield return new WaitForSeconds(0.5f);
        _owner.RemoveItem();
        NetworkServer.Destroy(gameObject);
    }
}
