using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public enum ItemAttachPos
{
    NONE,
    TOP,
    BACK
};

public class Item : NetworkBehaviour
{
    [SerializeField]
    protected GameObject _itemGO;
    [SerializeField]
    protected Animation _animation;

    [SerializeField]
    protected SplineWalkerCon _swc;

    public int _itemID;
    [SerializeField]
    protected float _speed;
    [SerializeField]
    protected int _damage;
    [SerializeField]
    public float _bonusSpeed;
    [SerializeField]
    public float _bonusSpeedApplyTime;

    [SerializeField]
    private ItemAttachPos _attachPos;
    [SerializeField]
    private float _attachScale;
    private Vector3 _originalScale;
    [SerializeField]
    private Vector3 _attachLocalPosition = Vector3.zero;
    [SerializeField]
    private Vector3 _attachLocalRotation = Vector3.zero;

    [SerializeField]
    protected GameObject _useItemFX;
    [SerializeField]
    protected AudioClip _useItemSound;

    public GameObject _applyItemFX;    
    public AudioClip _applyItemSound;

    [SerializeField]
    protected string _showAnimName = "show";
    [SerializeField]
    protected string _hideAnimName = "hide";
    [SerializeField]
    protected string _useAnimName = "use";

    [SerializeField]
    protected float _spawnOffsetY;
    [SerializeField]
    protected float _floorOffsetY;
    protected float _currentOffsetY;
    protected Constants.JumpState _jumpState;
    [SerializeField]
    protected float _offsetVelocity;
    [SerializeField]
    private float _offsetVelocityChangeRatio;

    [SyncVar]
    protected bool _show;
    [SyncVar]
    protected string _carPlayerUUID;

    [SerializeField]
    protected List<GameObject> _fx = new List<GameObject>();

    [SerializeField]
    protected float _magnetAttachDistance;

    [SerializeField]
    private bool _dangerAlarm;
    private int _alarmLaneNO;
    private bool _alarmActivated;

    public bool _targetable = false;
    public bool _using = false;
    public bool _stolenByMagnet = false;    
    protected CarController _owner;

    public bool _multiItem = false;

    void Update()
    {
        UpdateJumpState();
    }

    public override void OnStartClient()
    {
        // except host
        if (NetworkServer.active)
        {
            return;
        }
        
        _owner = GameObject.FindWithTag(Constants.GAME_MANAGER_TAG_NAME).GetComponent<GameManager>().FindCarWithPlayerUUID(_carPlayerUUID);
        AttachToCar();
        ToggleShow();        
    }

    protected virtual void UpdateJumpState() 
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
                _jumpState = Constants.JumpState.None;
            }
        }

        _itemGO.transform.localPosition = new Vector3(_itemGO.transform.localPosition.x, _currentOffsetY, _itemGO.transform.localPosition.z);
    }

    internal virtual void GiveTo(CarController aCar)
    {   
        _show = aCar.GetItem(this);
        _owner = aCar;
        _carPlayerUUID = aCar._playerNo;

        AttachToCar();
        ToggleShow();
    }

    internal virtual void AttachToCar()
    {
        if (ItemAttachPos.NONE == _attachPos)
        {
            transform.parent = _owner.CarTransform;
        }
        else if (ItemAttachPos.TOP == _attachPos)
        {
            transform.parent = _owner.ItemTop.transform;
        }
        else if (ItemAttachPos.BACK == _attachPos)
        {
            transform.parent = _owner.ItemBack.transform;
        }    
        
        transform.localPosition = _attachLocalPosition;
        transform.localRotation = Quaternion.Euler(_attachLocalRotation);

        _originalScale = gameObject.transform.localScale;
        gameObject.transform.localScale *= _attachScale;
    }

    protected void DetachFromCar()
    {
        _owner.RemoveItem();

        transform.parent = null;
        transform.localPosition = Vector3.zero;
        transform.localScale = _originalScale;
        StartCoroutine(IgnoreCollisionWithOwner());        
    }

    private IEnumerator IgnoreCollisionWithOwner()
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

    internal virtual void ToggleShow()
    {
        _itemGO.SetActive(_show);
        if (_show)
        {
            if (null != _animation)
            {
                _animation.Play(_showAnimName);
            }

            if (_owner.isLocalPlayer)
            {
                _owner._updateItemUseButton = true;
            }
        }
    }

    internal virtual void Use()
    {
        YPLog.Trace();

        _using = true;
        _owner.UpdatePlayData((short)Constants.StatKey.USE_INGAME_ITEM, 1);        
        CommonUse();
        RpcUse();
    }

    [ClientRpc]
    public virtual void RpcUse()
    {
        if (NetworkServer.localClientActive)
        {
            return;
        }

        CommonUse();
    }

    protected virtual void CommonUse()
    {
        YPLog.Trace();

        DetachFromCar();

        if (null != _animation)
        {
            AnimationClip clip = _animation.GetClip(_useAnimName);
            if (null != clip)
            {
                _animation.Play(_useAnimName);
            }
        }
        _swc.Spline = _owner._swc.Spline;
        _swc.TF = _owner._swc.TF;
        _swc.Speed = _speed;
        _swc.Clamping = _owner._swc.Clamping;

        int dir = 1;
        float tf = _swc.TF;
        transform.position = _swc.Spline.MoveBy(ref tf, ref dir, 0f, _swc.Clamping);

        if(TutorialConstants._tutorialPlaying)
        {
            _itemGO.transform.localPosition = new Vector3(_swc.Spline.GetLaneOffsetX(_owner._laneNO) / transform.localScale.x, 0.0f, _itemGO.transform.localPosition.z);
        }
        else
        {
            _itemGO.transform.localPosition = new Vector3(_swc.Spline.GetLaneOffsetX(_owner._laneNO) / transform.localScale.x, _itemGO.transform.localPosition.y + _spawnOffsetY, _itemGO.transform.localPosition.z);
        }
        _currentOffsetY = _itemGO.transform.localPosition.y;
        _jumpState = Constants.JumpState.GoingDown;

        ToggleItemFX(true);
        _owner.UseItemFX(_useItemFX, _useItemSound);

        if (_dangerAlarm)
        {
            _alarmLaneNO = _owner._laneNO;
            StartCoroutine(DangerAlarm());
        }
    }

    private IEnumerator DangerAlarm()
    {
        CarController target = _owner.GM.GetLocalPlayer();
        if (null == target || !target._dangerAlarm)
        {
            yield break;
        }

        float maximumDistance = 10f;        
        float checkDistance = 0f;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            
            checkDistance = maximumDistance;
            if (_swc.TF <= target._swc.TF)
            {
                float diff = target._swc.TF - _swc.TF;
                checkDistance = _swc.Spline.TFToDistance(diff);
            }
            else
            {
                if (0.95f < _swc.TF && 0.05f > target._swc.TF)
                {
                    float diff = 1f + (target._swc.TF - _swc.TF);
                    checkDistance = _swc.Spline.TFToDistance(diff);
                }
            }

            if (checkDistance < maximumDistance)
            {
                if (!_alarmActivated)
                {
                    target.AddDangerItem(_alarmLaneNO, this);
                    _alarmActivated = true;
                }
            }
            else
            {
                if (_alarmActivated)
                {
                    target.RemoveDangerItem(_alarmLaneNO, this);
                    _alarmActivated = false;                    
                }
            }            
        }
    }

    public override void OnNetworkDestroy()
    {
        if (_alarmActivated)
        {
            CarController target = _owner.GM.GetLocalPlayer();
            if (null != target)
            {
                target.RemoveDangerItem(_alarmLaneNO, this);
            }
        }

        base.OnNetworkDestroy();
    }

    internal virtual void UseToTarget(CarController aTarget) { }
    [ClientRpc]
    public virtual void RpcUseToTarget(NetworkInstanceId aNetID) { }
    protected virtual void CommonUseToTarget(CarController aTarget) { }

    protected virtual void ToggleItemFX(bool aShow)
    {
        foreach (GameObject go in _fx)
        {
            go.SetActive(aShow);
        }
    }
    
    internal virtual void Collide(CarController aCar)
    {
        YPLog.Trace();
        
        // apply effect        
        ApplyEffect(aCar);

        // destory item
        NetworkServer.Destroy(gameObject);
    }
    
    internal void CollideOtherItem()
    {   
        NetworkServer.Destroy(gameObject);
    }

    protected virtual void ApplyEffect(CarController aCar)
    {
        YPLog.Trace();
        YPLog.Log("damage = " + _damage);

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

    internal virtual void AttachFX(CarController aCar) { }

    internal virtual void StolenByMagnet(Item aMagnet)
    {
        _stolenByMagnet = true;
        _owner.RemoveItem();

        RpcStolenByMagnet(aMagnet.netId);       
    }

    [ClientRpc]
    public virtual void RpcStolenByMagnet(NetworkInstanceId aNetID)
    {
        transform.parent = null;
        transform.localPosition = Vector3.zero;
        transform.localScale = _originalScale;
        transform.position = _owner.CarTransform.position + new Vector3(0f, 0.5f, 0f);

        ItemMagnet magnet = ClientScene.FindLocalObject(aNetID).GetComponent<ItemMagnet>();
        StartCoroutine(MoveTowardMagnet(magnet));
    }

    protected IEnumerator MoveTowardMagnet(ItemMagnet aMagnet)
    {
        float distance = Vector3.Distance(transform.position, aMagnet._animation.gameObject.transform.position);
        float power = aMagnet._startPullPower;

        while (_magnetAttachDistance < distance)
        {
            Vector3 targetPos = aMagnet._animation.transform.position;
            Vector3 currentPos = gameObject.transform.position;
            Vector3 dir = (targetPos - currentPos).normalized;

            power += aMagnet._increasePullPowerRatio * Time.deltaTime;
            Vector3 nextPos = gameObject.transform.position + dir * power * Time.deltaTime;
            Vector3 velocity = Vector3.zero;
            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, nextPos, ref velocity, Time.deltaTime * 1f);

            distance = Vector3.Distance(gameObject.transform.position, aMagnet._animation.transform.position);

            yield return null;
        }

        gameObject.transform.parent = aMagnet._animation.transform;
    }

    internal virtual void SetLevel(int aLevel) { }
    internal virtual void SpawnFireUnit() { }
    internal virtual void Enhance(float aValue)
    {
        _damage = (int)aValue;
    }
}