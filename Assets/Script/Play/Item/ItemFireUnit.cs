using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ItemFireUnit : NetworkBehaviour
{
    [SerializeField]
    protected GameObject _itemGO;

    [SerializeField]
    protected Animation _animation;

    [SerializeField]
    protected SplineWalkerCon _swc;

    [SerializeField]
    private List<GameObject> _fx = new List<GameObject>();

    [SerializeField]
    protected string _showAnimName;

    [SerializeField]
    private ItemAttachPos _attachPos;
    [SerializeField]
    protected Vector3 _attachLocalPosition;
    [SerializeField]
    private Vector3 _attachLocalRotation;
    [SerializeField]
    protected Vector3 _detachLocalRotation;

    [SerializeField]
    private float _offsetVelocity;
    [SerializeField]
    private float _offsetVelocityChangeRatio;
    [SerializeField]
    protected float _floorOffsetY;
    [SerializeField]
    protected float _spawnOffsetY;
    protected float _currentOffsetY = 0f;
    protected Constants.JumpState _jumpState = Constants.JumpState.None;

    [SerializeField]
    protected float _unitScale;
    [SerializeField]
    protected float _itemScale;

    private int _damage;
    protected float _speed;
    protected float _bonusSpeed;
    protected float _bonusSpeedApplyTime;

    protected GameObject _useItemFX;
    protected AudioClip _useItemSound;
    public GameObject _applyItemFX;
    public AudioClip _applyItemSound;

    protected CarController _owner;
    [SyncVar]
    protected int _index;

    [SyncVar]
    protected NetworkInstanceId _netID;

    void Start()
    {
        YPLog.Trace();
        YPLog.Log("index = " + _index + ", _netID = " + _netID);

        if (NetworkServer.active)
        {
            GameObject go = NetworkServer.FindLocalObject(_netID);
            ItemMultiFire item = go.GetComponent<ItemMultiFire>();
            YPLog.Log("server! go = " + go + ", item = " + item);
            item.AddFireUnit(this, _index);
        }
        else if (NetworkClient.active)
        {
            GameObject go = ClientScene.FindLocalObject(_netID);
            ItemMultiFire item = go.GetComponent<ItemMultiFire>();
            YPLog.Log("client! go = " + go + ", item = " + item);
            item.AddFireUnit(this, _index);
        }
    }

    void Update()
    {
        UpdateJumpState();
    }

    protected void UpdateJumpState()
    {
        if (Constants.JumpState.None == _jumpState)
        {
            return;
        }

        if (Constants.JumpState.GoingUp == _jumpState)
        {
            _offsetVelocity -= _offsetVelocityChangeRatio * Time.deltaTime;
            _currentOffsetY += _offsetVelocity * Time.deltaTime;
            if (0f >= _offsetVelocity)
            {
                _jumpState = Constants.JumpState.GoingDown;
            }
        }
        else if (Constants.JumpState.GoingDown == _jumpState)
        {
            _offsetVelocity += _offsetVelocityChangeRatio * Time.deltaTime;
            _currentOffsetY -= _offsetVelocity * Time.deltaTime;
            if (_floorOffsetY >= _currentOffsetY)
            {
                _currentOffsetY = _floorOffsetY;
                _jumpState = Constants.JumpState.None;
            }
        }

        _itemGO.transform.localPosition = new Vector3(_itemGO.transform.localPosition.x, _currentOffsetY, _itemGO.transform.localPosition.z);
    }

    internal virtual void AttachToCar(CarController aCar)
    {
        if (ItemAttachPos.NONE == _attachPos)
        {
            transform.parent = aCar.CarTransform;
        }
        else if (ItemAttachPos.TOP == _attachPos)
        {
            transform.parent = aCar.ItemTop.transform;
        }
        else if (ItemAttachPos.BACK == _attachPos)
        {
            transform.parent = aCar.ItemBack.transform;
        }

        transform.localPosition = _attachLocalPosition;
        transform.localRotation = Quaternion.Euler(_attachLocalRotation);

        _animation.Play(_showAnimName);
    }

    internal void SetOwner(CarController aCar)
    {
        _owner = aCar;
    }

    internal void SetItemInfo(int aDamage, float aSpeed, float aBonusSpeed, float aBonusSpeedApplyTime)
    {
        _damage = aDamage;
        _speed = aSpeed;
        _bonusSpeed = aBonusSpeed;
        _bonusSpeedApplyTime = aBonusSpeedApplyTime;
    }

    internal void SetFXInfo(GameObject aUseFX, AudioClip aUseSound, GameObject aApplyFX, AudioClip aApplySound)
    {
        _useItemFX = aUseFX;
        _useItemSound = aUseSound;
        _applyItemFX = aApplyFX;
        _applyItemSound = aApplySound;
    }

    internal void SetUnitInfo(int aIndex, NetworkInstanceId aNetID)
    {
        _index = aIndex;
        _netID = aNetID;
    }

    internal virtual void Fire()
    {
        _animation.Stop();

        _swc.Spline = _owner._swc.Spline;
        _swc.TF = _owner._swc.TF;
        _swc.Speed = _speed;
        _swc.Clamping = _owner._swc.Clamping;

        int dir = 1;
        float tf = _swc.TF;
        transform.parent = null;
        transform.position = _swc.Spline.MoveBy(ref tf, ref dir, 0f, _swc.Clamping);
        transform.localScale = new Vector3(_unitScale, _unitScale, _unitScale);
        _itemGO.transform.localPosition = new Vector3(_swc.Spline.GetLaneOffsetX(_owner._laneNO) / transform.localScale.x, _itemGO.transform.localPosition.y + _spawnOffsetY, _itemGO.transform.localPosition.z);
        _itemGO.transform.localRotation = Quaternion.Euler(_detachLocalRotation);
        _itemGO.transform.localScale = new Vector3(_itemScale, _itemScale, _itemScale);

        _currentOffsetY = _itemGO.transform.localPosition.y;
        _jumpState = Constants.JumpState.GoingUp;

        ToggleItemFX(true);
        _owner.UseItemFX(_useItemFX, _useItemSound);
        StartCoroutine(IgnoreCollisionWithOwner());
    }

    protected void ToggleItemFX(bool aShow)
    {
        foreach (GameObject go in _fx)
        {
            go.SetActive(aShow);
        }
    }

    protected IEnumerator IgnoreCollisionWithOwner()
    {
        BoxCollider[] colliders = gameObject.GetComponentsInChildren<BoxCollider>();
        for (int index = 0; index < colliders.Length; ++index)
        {
            Physics.IgnoreCollision(colliders[index], _owner.Collider, true);
            colliders[index].enabled = true;
        }

        yield return new WaitForSeconds(0.5f);
        for (int index = 0; index < colliders.Length; ++index)
        {
            Physics.IgnoreCollision(colliders[index], _owner.Collider, false);
        }
    }

    internal virtual void Collide(CarController aCar)
    {
        YPLog.Trace();

        ApplyEffect(aCar);

        NetworkServer.Destroy(gameObject);
    }

    protected virtual void ApplyEffect(CarController aCar)
    {
        if (aCar.ShieldDefence())
        {
            return;
        }

        int damage = -_damage;
        if (aCar.DefenceDamage(ref damage))
        {
            return;
        }

        aCar.UpdateHP(damage, _owner, false);
    }

    internal virtual void FireToTarget(CarController aTarget) { }
    internal Vector3 GetAttachLocalRotation()
    {
        return _attachLocalRotation;
    }

    internal virtual void Stick(CarController aCar) { }

    internal void CollideOtherItem()
    {
        NetworkServer.Destroy(gameObject);
    }
}