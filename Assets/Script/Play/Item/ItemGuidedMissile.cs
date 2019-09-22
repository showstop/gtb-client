using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemGuidedMissile : Item 
{
    [SerializeField]
    private float _launchTime;

    [SerializeField]
    private float _launchSpeed;

    internal override void UseToTarget(CarController aTarget) 
    {
        _owner.UpdatePlayData((short)Constants.StatKey.USE_INGAME_ITEM, 1);
        RpcUseToTarget(aTarget.netId);
    }

    [ClientRpc]
    public override void RpcUseToTarget(NetworkInstanceId aNetID) 
    {
        ToggleItemFX(true);
        _owner.UseItemFX(_useItemFX, _useItemSound);

        CarController target = ClientScene.FindLocalObject(aNetID).GetComponent<CarController>();
        StartCoroutine(Fire(target));
    }

    private IEnumerator Fire(CarController aTarget)
    {
        float checkTime = 0f;
        while (_launchTime >= checkTime)
        {
            _itemGO.transform.localPosition = new Vector3(_itemGO.transform.localPosition.x, _itemGO.transform.localPosition.y + _launchSpeed * Time.deltaTime, _itemGO.transform.localPosition.z);
            checkTime += Time.deltaTime;

            yield return null;
        }

        DetachFromCar();
        transform.position = _owner.CarTransform.position + _itemGO.transform.localPosition;
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