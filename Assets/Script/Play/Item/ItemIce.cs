using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemIce : ItemPeriod 
{
    [SerializeField]
    private GameObject _attachFX;

    internal override void Use()
    {
        _owner.UpdatePlayData((short)Constants.StatKey.USE_INGAME_ITEM, 1);
        CommonUse();
        RpcUse();
    }

    protected override void CommonUse()
    {
        DetachFromCar();

        _swc.Spline = _owner._swc.Spline;
        _swc.TF = _owner._swc.TF;
        _swc.Speed = _speed;
        _swc.Clamping = _owner._swc.Clamping;

        int dir = 1;
        float tf = _swc.TF;
        transform.position = _swc.Spline.MoveBy(ref tf, ref dir, 0f, _swc.Clamping);
        _itemGO.transform.localPosition = new Vector3(_swc.Spline.GetLaneOffsetX(_owner._laneNO) / transform.localScale.x,
                                                    _itemGO.transform.localPosition.y + _swc.Spline.LaneOffsetY + _spawnOffsetY,
                                                    _itemGO.transform.localPosition.z);

        _currentOffsetY = _itemGO.transform.localPosition.y;
        _jumpState = Constants.JumpState.GoingUp;

        ToggleItemFX(true);
        _owner.UseItemFX(_useItemFX, _useItemSound);
    }

    internal override void AttachFX(CarController aCar)
    {
        GameObject go = Instantiate(_attachFX, Vector3.zero, Quaternion.identity) as GameObject;
        ItemAttachFX fx = go.GetComponent<ItemAttachFX>();
        fx.Play(_applyTime, aCar);
    }

    protected override void ApplyEffect(CarController aCar)
    {
        if (aCar.ShieldDefence())
        {
            return;
        }

        if (aCar.DefenceSpeeding())
        {
            return;
        }

        aCar.ApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
        aCar.RpcApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
    }
}