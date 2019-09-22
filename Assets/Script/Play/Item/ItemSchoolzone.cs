using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ItemSchoolzone : ItemPeriod
{
    private List<CarController> _applyCars = new List<CarController>();

    protected override IEnumerator CheckApplyTime()
    {
        yield return new WaitForSeconds(_applyTime);
        RpcEndApply();

        _owner.GM.RemoveSchoolZoneInfo(this);

        // hide animation length = 0.5 sec
        yield return new WaitForSeconds(0.5f);
        _owner.RemoveItem();
        NetworkServer.Destroy(gameObject);
    }

    protected override void ApplyEffect(CarController aCar)
    {
        _owner.GM.AddSchoolZoneInfo(this);

        List<CarController> otherCars = _owner.GM.GetOtherPlayers(_owner);
        for (int index = 0; index < otherCars.Count; ++index)
        {
            if (_owner._laneNO != otherCars[index]._laneNO)
            {
                continue;
            }

            _applyCars.Add(otherCars[index]);
            otherCars[index].ApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
            otherCars[index].RpcApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
        }
    }

    internal void ApplyEffectDuringChangeLane(CarController aCar, int aLaneNO)
    {
        if (_owner == aCar)
        {
            return;
        }

        if (_applyCars.Contains(aCar))
        {
            return;
        }

        if (_owner._laneNO != aLaneNO)
        {
            return;
        }

        _applyCars.Add(aCar);
        aCar.ApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
        aCar.RpcApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
    }

    internal override void Enhance(float aValue)
    {
        _bonusSpeedApplyTime = aValue;
    }
}