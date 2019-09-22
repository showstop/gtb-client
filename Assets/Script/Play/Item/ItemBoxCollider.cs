using UnityEngine;
using UnityEngine.Networking;

public class ItemBoxCollider : MonoBehaviour 
{
    [SerializeField]
    private ItemBox _itemBox;

    void OnTriggerEnter(Collider aCol)
    {
        CarController target = aCol.gameObject.GetComponentInParent<CarController>();
        if (null == target)
        {
            return;
        }        
        
        if (target.isLocalPlayer)
        {
            //YPLog.Trace();
            //YPLog.Log("localPlayer = " + target.isLocalPlayer + ", ai = " + target._isAI + ", server = " + target.NetworkServer.active);
            target.GetItemFX();
            target.CmdGetItem(target._rank, _itemBox.netId);
        }
        else if (target._isAI && NetworkServer.active)
        {
            _itemBox.ToggleActive(false);
            target.GetItemFX();
            target.GM.GiveItem(target, target._rank);
        }
    }
}