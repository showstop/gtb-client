using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemLastAttack : Item 
{
    [SerializeField]
    private float _launchTime;

    [SerializeField]
    private float _launchSpeed;

    [SyncVar]
    private string _targetPlayerUUID;

    [SyncVar]
    private Vector3 _addVector;

    public override void OnStartClient()
    {
        // except host
        if (NetworkServer.active)
        {
            return;
        }

        YPLog.Log("show = " + _show + ", playerUUID = " + _carPlayerUUID);
        GameManager gm = GameObject.FindWithTag(Constants.GAME_MANAGER_TAG_NAME).GetComponent<GameManager>();
        _owner = gm.FindCarWithPlayerUUID(_carPlayerUUID);
        CarController target = gm.FindCarWithPlayerUUID(_targetPlayerUUID);

        StartCoroutine(Fire(target));
    }

    internal void SetInfo(CarController aOwner, CarController aTarget, Vector3 aAddVector)
    {
        _owner = aOwner;
        _carPlayerUUID = _owner._playerNo;
        _targetPlayerUUID = aTarget._playerNo;
        _addVector = aAddVector;

        StartCoroutine(Fire(aTarget));
    }

    private IEnumerator Fire(CarController aTarget)
    {
        transform.position = _owner.CarTransform.position + _addVector;

        float checkTime = 0f;
        while (_launchTime >= checkTime)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + _launchSpeed * Time.deltaTime, transform.position.z);
            checkTime += Time.deltaTime;

            yield return null;
        }

        while (true)
        {
            Vector3 targetPos = aTarget.CarTransform.position;
            Vector3 currentPos = transform.position;
            Vector3 dir = (targetPos - currentPos).normalized;

            Vector3 nextPos = transform.position + dir * _speed * Time.deltaTime;
            Vector3 velocity = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, nextPos, ref velocity, Time.deltaTime);
            _itemGO.transform.LookAt(targetPos);

            yield return null;
        }
    }
}