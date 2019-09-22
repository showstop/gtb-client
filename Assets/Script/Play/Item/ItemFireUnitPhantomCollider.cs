using UnityEngine;
using UnityEngine.Networking;

public class ItemFireUnitPhantomCollider : ItemFireUnitCollider
{
    void OnTriggerEnter(Collider aCol)
    {
        CarController target = aCol.gameObject.GetComponentInParent<CarController>();
        if (null != target)
        {
            target.ApplyItemFX(_itemUnit._applyItemFX, _itemUnit._applyItemSound, 0f);
            if (target.isLocalPlayer)
            {
                YPLog.Trace();
                YPLog.Log("localPlayer = " + target.isLocalPlayer + ", ai = " + target._isAI + ", server = " + NetworkServer.active);

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
                }
                else
                {
                    ItemFireUnitPhantom unit = _itemUnit as ItemFireUnitPhantom;
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
                    }
                    else
                    {
                        ItemFireUnitPhantom unit = _itemUnit as ItemFireUnitPhantom;
                        unit.CmdCollideItem(item.netId);
                    }    
                }
            }
        }
    }
}