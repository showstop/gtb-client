using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemChaserFireUnit : ItemFireUnitSlow 
{
    [SerializeField]
    private float _fireScale;

    [SerializeField]
    private float _scaleChangeRatio;

    [SerializeField]
    private BoxCollider _collider;

    [SerializeField]
    private GameObject _handleGO;
    
    private Transform _crossBow = null;

    internal override void AttachToCar(CarController aCar)
    {
        if( null == _crossBow)
        {
            _crossBow = transform.parent.transform.parent;
        }

        transform.parent = _crossBow;
    }

    internal override void Fire()    
    {
        if (null != _animation)
        {
            _animation.Stop();
        }

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
        _jumpState = Constants.JumpState.GoingDown;

        ToggleItemFX(true);
        _owner.UseItemFX(_useItemFX, _useItemSound);
        StartCoroutine(IgnoreCollisionWithOwner());
        StartCoroutine(ChangeScale(_itemGO.transform.localPosition.x));
    }

    private IEnumerator ChangeScale(float aFireLocalPositionX)
    {   
        float currentScale = gameObject.transform.localScale.x;
        while (_fireScale > currentScale)
        {
            currentScale += _scaleChangeRatio * Time.deltaTime;
            if (_fireScale < currentScale)
            {
                currentScale = _fireScale;
            }

            gameObject.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            _itemGO.transform.localPosition = new Vector3(aFireLocalPositionX / currentScale, _itemGO.transform.localPosition.y, _itemGO.transform.localPosition.z);

            yield return null;
        }
    }

    internal override void Collide(CarController aCar)
    {
        YPLog.Trace();

        ApplyEffect(aCar);
        StartCoroutine(DestroyItem());
    }

    private IEnumerator DestroyItem()
    {
        yield return new WaitForSeconds(_bonusSpeedApplyTime);
        NetworkServer.Destroy(gameObject);
    }
    
    internal override void Stick(CarController aCar)
    {
        _handleGO.SetActive(false);
        _collider.enabled = false;

        _swc.Speed = 0f;
        _swc.enabled = false;

        transform.parent = aCar.CarTransform;
        transform.localPosition = _attachLocalPosition;
        _itemGO.transform.localPosition = new Vector3(0f, _itemGO.transform.localPosition.y, _itemGO.transform.localPosition.z);
    }
}
