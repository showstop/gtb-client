using UnityEngine;
using UnityEngine.Networking;

public class ItemFireUnitCollider : MonoBehaviour
{
    [SerializeField]
    protected ItemFireUnit _itemUnit;

    void OnTriggerEnter(Collider aCol)
    {
        CarController target = aCol.gameObject.GetComponentInParent<CarController>();
        if (null == target)
        {
            return;
        }

        _itemUnit.Stick(target);
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
}