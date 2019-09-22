using UnityEngine;
using System.Collections;

public class BoostPedalCollider : MonoBehaviour 
{
    [SerializeField]
    private BoostPedal _bp;

    void OnTriggerEnter(Collider aCol)
    {
        CarController target = aCol.gameObject.GetComponent<CarController>();
        if (null == target)
        {
            return;
        }

        if (target.isLocalPlayer)
        {
            _bp.CmdBoost(target.netId);
        }
        else if (target._isAI && target.isServer)
        {
            _bp.Boost(target);
        }
    }
}