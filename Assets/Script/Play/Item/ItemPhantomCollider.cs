using UnityEngine;
using UnityEngine.Networking;

public class ItemPhantomCollider : ItemCollider 
{
    void OnTriggerEnter(Collider aCol)
    {
        YPLog.Trace();

        CarController target = aCol.gameObject.GetComponentInParent<CarController>();
        if (null != target)
        {
            target.ApplyItemFX(_item._applyItemFX, _item._applyItemSound, 0f);
            if (target.isLocalPlayer)
            {
                YPLog.Trace();
                YPLog.Log("localPlayer = " + target.isLocalPlayer + ", ai = " + target._isAI + ", server = " + NetworkServer.active);

                target.CmdCollideItem(_item.netId);
            }
            else if (target._isAI && NetworkServer.active)
            {
                _item.Collide(target);
            }
        }
        else
        {
            // TO DO : show fx & sound
            Item item = aCol.gameObject.GetComponentInParent<Item>();
            if (null != item)
            {
                if (NetworkServer.active)
                {
                    item.CollideOtherItem();
                }
                else
                {
                    ItemPhantom phantom = _item as ItemPhantom;
                    phantom.CmdCollideItem(item.netId);
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
                        ItemPhantom phantom = _item as ItemPhantom;
                        phantom.CmdCollideItem(item.netId);
                    }
                }
            }
        }
    }
}