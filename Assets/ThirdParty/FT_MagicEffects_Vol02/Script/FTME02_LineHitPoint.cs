using UnityEngine;
using System.Collections;

public class FTME02_LineHitPoint : MonoBehaviour 
{
    public Transform _hitPoint = null;
    private Vector3 _localPosition;

    void Update()
    {
        if (null != _hitPoint)
        {
            transform.position = _hitPoint.position + _localPosition;
        }
        else
        {
            YPLog.Trace();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500f))
            {
                transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }
        }
    }

    public void SetTargetPosition(CarController aTarget, Vector3 aLocalPosition)
    {
        _hitPoint = aTarget.CarTransform;
        _localPosition = aLocalPosition;
    }
}
