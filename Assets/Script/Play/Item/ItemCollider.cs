using UnityEngine;
using UnityEngine.Networking;

public class ItemCollider : MonoBehaviour
{
    [SerializeField]
    protected Item _item;

    void OnTriggerEnter(Collider aCol)
    {
        CarController target = aCol.gameObject.GetComponentInParent<CarController>();
        if (null == target)
        {
            return;
        }

        _item.AttachFX(target);
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

            if (TutorialConstants._tutorialPlaying)
            {
                TutoCarController tutoTarget = aCol.gameObject.GetComponentInParent<TutoCarController>();
                tutoTarget.CmdCollideItem(_item.netId);
            }
        }
    }
}