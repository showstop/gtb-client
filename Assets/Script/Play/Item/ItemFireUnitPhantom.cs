using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ItemFireUnitPhantom : ItemFireUnit
{   
    private float   _lifeTime       = 0f;
    protected bool  _updateSpeed    = true;

    [SerializeField]
    protected float _fireAddSpeed   = 0f;

    void Start()
    {
        if (NetworkServer.active)
        {
            GameObject go = NetworkServer.FindLocalObject(_netID);
            ItemMultiFire item = go.GetComponent<ItemMultiFire>();            
            item.AddFireUnit(this, _index);
        }
        else if (NetworkClient.active)
        {
            GameObject go = ClientScene.FindLocalObject(_netID);
            ItemMultiFire item = go.GetComponent<ItemMultiFire>();            
            item.AddFireUnit(this, _index);
        }

        _swc.Spline = _owner._swc.Spline;
        float tf = _owner._swc.TF + _owner._swc.Spline.DistanceToTF(0.1f);
        if (tf > 1f)
        {
            tf -= 1f;
        }
        _swc.InitialF = tf;
        _swc.Speed = _owner._speed;
        StartCoroutine(UpdateSpeed());

        _itemGO.transform.localPosition = new Vector3(_swc.Spline.GetLaneOffsetX(_index + 1), _swc.Spline.LaneOffsetY, 0f);
        _itemGO.transform.localRotation = Quaternion.Euler(Vector3.zero);

        _animation.Play(_showAnimName);
    }

    protected IEnumerator UpdateSpeed()
    {
        while (_updateSpeed)
        {
            yield return null;
            _swc.Speed = _owner._speed;
        }

        _swc.Speed = _owner._maxSpeed + _fireAddSpeed;
    }

    internal override void Fire()
    {
        _updateSpeed = false;        
        if (null != _animation)
        {
            _animation.Stop();
        }        

        ToggleItemFX(true);        

        //collider box Enable
        StartCoroutine(IgnoreCollisionWithOwner());
        if (NetworkServer.active)
        {
            StartCoroutine(CheckLifeTime());
        }
    }

    private IEnumerator CheckLifeTime()
    {
        while (_lifeTime > 0f)
        {
            yield return null;
            _lifeTime -= Time.deltaTime;            
        }

        NetworkServer.Destroy(gameObject);
    }

    internal override void Collide(CarController aCar)
    {
        ApplyEffect(aCar);
    }

    [Command]
    public virtual void CmdCollideItem(NetworkInstanceId aNetID)
    {
        Item item = NetworkServer.FindLocalObject(aNetID).GetComponent<Item>();
        if(item != null)
        {
            item.CollideOtherItem();
        }
    }

    [Command]
    public virtual void CmdCollideItemFireUnit(NetworkInstanceId aNetID)
    {
        ItemFireUnit unit = NetworkServer.FindLocalObject(aNetID).GetComponent<ItemFireUnit>();
        if (unit != null)
        {
            unit.CollideOtherItem();
        }
    }

    internal void SetLifeTime(float aLifeTime)
    {
        _lifeTime = aLifeTime;
    }
}