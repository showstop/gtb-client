using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemMagnet : ItemPermanent 
{
    [SerializeField]
    private string _detectAnimName;

    [SerializeField]
    private float _destScale;

    [SerializeField]
    private float _changeScaleRatio;
    
    public float _startPullPower;
    public float _increasePullPowerRatio;

    [SerializeField]
    private BoxCollider _collider;

    protected override void UpdateJumpState()
    {
        if (Constants.JumpState.None == _jumpState)
        {
            return;
        }

        if (Constants.JumpState.GoingUp == _jumpState)
        {
            float currentScale = _itemGO.transform.localScale.x;
            currentScale += _offsetVelocity * _changeScaleRatio * Time.deltaTime;
            if (_destScale < currentScale)
            {
                currentScale = _destScale;
            }
            _itemGO.transform.localScale = new Vector3(currentScale, currentScale, currentScale);

            _currentOffsetY += _offsetVelocity * Time.deltaTime;            
            if (_floorOffsetY <= _currentOffsetY)
            {
                _currentOffsetY = _floorOffsetY;
                _jumpState = Constants.JumpState.None;                

                _animation.Play(_detectAnimName);
                _collider.enabled = true;
                ToggleItemFX(true);
            }
            _itemGO.transform.localPosition = new Vector3(_itemGO.transform.localPosition.x, _currentOffsetY, _itemGO.transform.localPosition.z);
        }
    }

    protected override void CommonUse()
    {
        YPLog.Trace();

        DetachFromCar();

        _swc.Spline = _owner._swc.Spline;
        _swc.TF = _owner._swc.TF;
        _swc.Speed = _speed;
        _swc.Clamping = _owner._swc.Clamping;

        int dir = 1;
        float tf = _swc.TF;
        transform.position = _swc.Spline.MoveBy(ref tf, ref dir, 0f, _swc.Clamping);
        float offsetX = _swc.Spline.GetLaneOffsetX(_owner._laneNO) / transform.localScale.x;
        _itemGO.transform.localPosition = new Vector3(offsetX, _itemGO.transform.localPosition.y + _spawnOffsetY, _itemGO.transform.localPosition.z);
        for (int index = 0; index < _fx.Count; ++index)
        {
            _fx[index].gameObject.transform.localPosition = new Vector3(offsetX, _fx[index].transform.localPosition.y, _fx[index].transform.localPosition.z);
        }

        _currentOffsetY = _itemGO.transform.localPosition.y;
        _jumpState = Constants.JumpState.GoingUp;

        ToggleItemFX(true);
        _owner.UseItemFX(_useItemFX, _useItemSound);
    }

    internal override void Collide(CarController aCar)
    {
        YPLog.Trace();

        // apply effect
        ApplyEffect(aCar);
    }

    protected override void ApplyEffect(CarController aCar)
    {
        aCar.StolenItemByMagnet(this);
    }
}