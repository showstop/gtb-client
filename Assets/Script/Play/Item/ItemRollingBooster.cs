using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemRollingBooster : ItemPeriod 
{
    [SerializeField]
    private float _maxBonusTime;

    [SerializeField]
    private float _increaseBonusTimePerCollide;

    private float _checkBonusTime = 0f;

    protected override void ApplyEffect(CarController aCar)
    {
        aCar._rollingBoosterItem = this;

        aCar.ApplyRollingBooster(_bonusSpeed, _bonusSpeedApplyTime, _maxBonusTime, _increaseBonusTimePerCollide);
        aCar.RpcApplyRollingBooster(_bonusSpeed, _bonusSpeedApplyTime, _maxBonusTime, _increaseBonusTimePerCollide);        
    }

    internal void IncreaseRollingBoosterApplyTime(float aTime)
    {
        YPLog.Trace();

        if (_maxBonusTime <= _applyTime)
        {
            return;
        }

        _applyTime += aTime;
        YPLog.Log("apply time = " + _applyTime + ", time = " + aTime);
    }

    protected override IEnumerator CheckApplyTime()
    {
        while (_applyTime > _checkBonusTime)
        {
            yield return null;
            _checkBonusTime += Time.deltaTime;
            YPLog.Log("apply time = " + _applyTime + ", check time = " + _checkBonusTime);
        }
        
        RpcEndApply();

        // hide animation length = 0.5 sec
        yield return new WaitForSeconds(0.5f);
        _owner.RemoveItem();
        NetworkServer.Destroy(gameObject);
    }

    internal override void Enhance(float aValue)
    {
        _applyTime = aValue;
        _bonusSpeedApplyTime = aValue;
    }
}