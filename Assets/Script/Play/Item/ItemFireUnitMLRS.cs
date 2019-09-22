using UnityEngine;
using System.Collections;

public class ItemFireUnitMLRS : ItemFireUnit 
{
    [SerializeField]
    private float _launchTime;

    [SerializeField]
    private float _launchSpeed;

    internal override void FireToTarget(CarController aTarget) 
    {
        ToggleItemFX(true);
        _owner.UseItemFX(_useItemFX, _useItemSound);

        StartCoroutine(IgnoreCollisionWithOwner());
        StartCoroutine(Guided(aTarget));
    }

    private IEnumerator Guided(CarController aTarget)
    {
        float checkTime = 0f;
        while (_launchTime >= checkTime)
        {
            // up&down is z axis
            _itemGO.transform.localPosition = new Vector3(_itemGO.transform.localPosition.x, _itemGO.transform.localPosition.y, _launchSpeed * Time.deltaTime + _itemGO.transform.localPosition.z);
            checkTime += Time.deltaTime;

            yield return null;
        }

        transform.parent = null;
        transform.position = _owner.CarTransform.position + new Vector3(0f, _itemGO.transform.localPosition.z, 0f);
        _itemGO.transform.localPosition = Vector3.zero;

        while (true)
        {
            Vector3 targetPos = aTarget.CarTransform.position;
            Vector3 currentPos = gameObject.transform.position;
            Vector3 dir = (targetPos - currentPos).normalized;

            Vector3 nextPos = gameObject.transform.position + dir * _speed * Time.deltaTime;
            Vector3 velocity = Vector3.zero;
            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, nextPos, ref velocity, Time.deltaTime);
            _itemGO.transform.LookAt(targetPos);

            yield return null;
        }
    }
}