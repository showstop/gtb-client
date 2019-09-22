using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemHammer : ItemPeriod 
{
    [SerializeField]
    private float _range;

    [SerializeField]
    private int _colorIndex;

    [SerializeField]
    private float _hitTime;

    public override void OnStartClient()
    {
        base.OnStartClient();
        _owner.ShowItemRange(_range, _colorIndex, _show);
    }

    internal override void GiveTo(CarController aCar)
    {
        base.GiveTo(aCar);
        aCar.ShowItemRange(_range, _colorIndex, _show);
    }

    internal override void Use()
    {
        StartCoroutine(CheckApplyTime());

        _owner.UpdatePlayData((short)Constants.StatKey.USE_INGAME_ITEM, 1);
        CommonUse();
        RpcUse();
    }

    protected override void CommonUse()
    {
        StartCoroutine(DelayJump());
    }

    protected override IEnumerator CheckApplyTime()
    {
        yield return new WaitForSeconds(_hitTime);

        for (int index = 0; index < Constants.MAX_PLAYER_NUM; ++index)
        {
            CarController target = _owner.GM.GetCarWithRank(index + 1);
            if (null == target || target == _owner)
            {
                continue;
            }

            float distance = Vector3.Distance(_owner.CarTransform.position, target.CarTransform.position);
            if (distance <= _range)
            {
                ApplyEffect(target);
            }
        }

        yield return new WaitForSeconds(_applyTime - _hitTime);
        RpcEndApply();

        // hide animation length = 0.5 sec
        yield return new WaitForSeconds(0.5f);
        _owner.RemoveItem();
        NetworkServer.Destroy(gameObject);
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

    private IEnumerator DelayJump()
    {
        _animation.Play(_useAnimName);
        yield return new WaitForSeconds(_hitTime);

        _owner.ShowItemRange(_range, _colorIndex, false);
        _owner.UseItemFX(_useItemFX, _useItemSound);
        _owner.StartJump(false, Constants.SLOW_HAMMER_JUMP_START_VELOCITY);
        if (_owner.isLocalPlayer)
        {
            _owner.ShakeCamera(_bonusSpeedApplyTime);
        }
    }
}