using UnityEngine;
using UnityEngine.Networking;

public class ItemFireUnitBarrelCollider : ItemFireUnitCollider
{
    void OnTriggerEnter(Collider aCol)
    {
        CarController target = aCol.gameObject.GetComponentInParent<CarController>();
        if (null != target)
        {
            target.ApplyItemFX(_itemUnit._applyItemFX, _itemUnit._applyItemSound, 0f);
            if (target.isLocalPlayer)
            {
                target.CmdCollideItemFireUnit(_itemUnit.netId);
            }
            else if (target._isAI && NetworkServer.active)
            {
                _itemUnit.Collide(target);
            }
        }
        else
        {
            Item item = aCol.gameObject.GetComponentInParent<Item>();
            if (null != item)
            {
                if (NetworkServer.active)
                {
                    item.CollideOtherItem();
                    _itemUnit.CollideOtherItem();
                }
                else
                {
                    ItemFireUnitBarrel unit = _itemUnit as ItemFireUnitBarrel;
                    unit.CmdCollideItem(item.netId);
                }
            }
            else
            {
                ItemFireUnit fireUnit = aCol.gameObject.GetComponentInParent<ItemFireUnit>();
                if (null != fireUnit)
                {
                    if (NetworkServer.active)
                    {
                        fireUnit.CollideOtherItem();
                        _itemUnit.CollideOtherItem();
                    }
                    else
                    {
                        ItemFireUnitBarrel unit = _itemUnit as ItemFireUnitBarrel;
                        unit.CmdCollideItem(item.netId);
                    }
                }
            }
        }
    }
}