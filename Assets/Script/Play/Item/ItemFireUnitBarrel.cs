using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemFireUnitBarrel : ItemFireUnitPhantom 
{
    [SerializeField]
    private string _fireAnimName;

    [SerializeField]
    private MeshRenderer _renderer;

    [SerializeField]
    private Material _fireMaterial;

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
        float tf = _owner._swc.TF - _owner._swc.Spline.DistanceToTF(0.1f);
        if (tf < 0f)
        {
            tf += 1f;
        }
        _swc.InitialF = tf;
        _swc.Speed = _owner._speed;
        StartCoroutine(UpdateSpeed());

        _itemGO.transform.localPosition = new Vector3(_swc.Spline.GetLaneOffsetX(_index + 1), _swc.Spline.LaneOffsetY + _itemGO.transform.localPosition.y, 0f);        
        _itemGO.transform.localRotation = Quaternion.Euler(Vector3.zero);        

        _animation.Play(_showAnimName);
    }

    internal override void Fire()
    {
        _updateSpeed = false;
        if (null != _animation)
        {
            _animation.Play(_fireAnimName);
        }

        _swc.Forward = false;
        _itemGO.transform.localScale = new Vector3(_unitScale, _unitScale, _unitScale);
        _renderer.sharedMaterial = _fireMaterial;

        ToggleItemFX(true);

        //collider box Enable
        StartCoroutine(IgnoreCollisionWithOwner());
    }

    internal override void Collide(CarController aCar)
    {
        ApplyEffect(aCar);

        NetworkServer.Destroy(gameObject);
    }

    [Command]
    public override void CmdCollideItem(NetworkInstanceId aNetID)
    {
        Item item = NetworkServer.FindLocalObject(aNetID).GetComponent<Item>();
        if (item != null)
        {
            item.CollideOtherItem();
            CollideOtherItem();
        }
    }

    [Command]
    public override void CmdCollideItemFireUnit(NetworkInstanceId aNetID)
    {
        ItemFireUnit unit = NetworkServer.FindLocalObject(aNetID).GetComponent<ItemFireUnit>();
        if (unit != null)
        {
            unit.CollideOtherItem();
            CollideOtherItem();
        }
    }
}