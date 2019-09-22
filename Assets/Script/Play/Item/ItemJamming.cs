using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemJamming : ItemPeriod 
{
    protected override IEnumerator CheckApplyTime()
    {
        yield return new WaitForSeconds(_applyTime);
        RpcEndApply();
        for (int index = 0; index < Constants.MAX_PLAYER_NUM; ++index)
        {
            CarController target = _owner.GM.GetCarWithRank(index + 1);
            if (null == target || _owner == target)
            {
                continue;
            }

            target._jamming = false;
        }

        // hide animation length = 0.5 sec
        yield return new WaitForSeconds(0.5f);
        _owner.RemoveItem();
        NetworkServer.Destroy(gameObject);
    }

    protected override void ApplyEffect(CarController aCar)
    {
        for (int index = 0; index < Constants.MAX_PLAYER_NUM; ++index)
        {
            CarController target = _owner.GM.GetCarWithRank(index + 1);
            if (null == target || _owner == target)
            {
                continue;
            }

            if (target.ShieldDefence())
            {
                continue;
            }

            target._jamming = true;
        }
    }
}
