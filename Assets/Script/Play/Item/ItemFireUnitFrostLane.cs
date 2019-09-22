using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemFireUnitFrostLane : ItemFireUnitSlow 
{
    [SerializeField]
    private float _locInterval;

    private bool _updateLocalLocation;

    void Start()
    {
        YPLog.Trace();
        YPLog.Log("index = " + _index + ", _netID = " + _netID);

        if (NetworkServer.active)
        {
            GameObject go = NetworkServer.FindLocalObject(_netID);            
            ItemMultiFire item = go.GetComponent<ItemMultiFire>();
            item.AddFireUnit(this, _index);
            _updateLocalLocation = true;
        }
        else if (NetworkClient.active)
        {
            GameObject go = ClientScene.FindLocalObject(_netID);            
            ItemMultiFire item = go.GetComponent<ItemMultiFire>();
            item.AddFireUnit(this, _index);
            _updateLocalLocation = true;
        }
    }

	void LateUpdate()
    {
        if (!_updateLocalLocation)
        {
            return;
        }

        float distance = _owner._swc.Distance - (_locInterval * (_index + 1)) - 0.1f;
        if (distance < 0f)
        {
            distance += _owner._swc.Spline.Length;
        }

        float tf = _owner._swc.Spline.DistanceToTF(distance);
        int dir = 1;
        Vector3 unitPos = _owner._swc.Spline.MoveBy(ref tf, ref dir, 0f, CurvyClamping.Loop);

        gameObject.transform.position = unitPos;
        _itemGO.transform.localPosition = _owner.CarTransform.localPosition + new Vector3(0f, _spawnOffsetY, 0f);
    }

    internal override void Fire()
    {
        _updateLocalLocation = false;

        gameObject.transform.parent = null;        
        _itemGO.transform.localPosition = new Vector3(_itemGO.transform.localPosition.x, _floorOffsetY, _itemGO.transform.localPosition.z);        

        ToggleItemFX(true);
        _owner.UseItemFX(_useItemFX, _useItemSound);
        StartCoroutine(IgnoreCollisionWithOwner());
    }
}