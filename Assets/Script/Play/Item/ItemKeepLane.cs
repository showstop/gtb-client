using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemKeepLane : ItemPeriod
{
    protected override void CommonUse()
    {
        _animation[_useAnimName].speed = 2f;
        _animation.Play(_useAnimName);

        ToggleItemFX(true);
        _owner.UseItemFX(_useItemFX, _useItemSound);
    }

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

            target._oneWay = false;
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

            target._oneWay = true;
        }
    }	
}