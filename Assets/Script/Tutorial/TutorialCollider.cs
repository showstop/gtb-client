using UnityEngine;
using System.Collections;

public class TutorialCollider : MonoBehaviour
{
    [SerializeField]
    private CarController _owner;

    private void OnTriggerEnter(Collider aCollider)
    {
        CarController otherCar = aCollider.gameObject.GetComponentInParent<CarController>();
        if (null == otherCar)
        {
            return;
        }
        TutorialConstants.IngameState = TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_COLLISION_END;

        _owner.Collide(otherCar);
    }
}
