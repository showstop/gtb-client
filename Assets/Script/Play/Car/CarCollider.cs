using UnityEngine;
using System.Collections;

public class CarCollider : MonoBehaviour
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

        _owner.Collide(otherCar);
    }
}
