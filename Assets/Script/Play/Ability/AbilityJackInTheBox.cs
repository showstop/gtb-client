using UnityEngine;
using System.Collections;

public class AbilityJackInTheBox : AbilitySpeedBoost 
{
    private int _startNumber = -1;
    private int _endNumber = 0;
    private bool _reverse = false;

    internal override void GetItemBox(int aLevel, CarController aCar)
    {
        YPLog.Trace();
        YPLog.Log("level = " + aLevel + ", car = " + aCar + ", start = " + _startNumber + ", end = " + _endNumber + ", reverse = " + _reverse);

        SetNumber((int)_levelValue[aLevel]);

        bool apply = false;
        int number = Random.Range(1, 101);
        YPLog.Log("random number = " + number);
        if (_reverse)
        {
            if (_startNumber <= number || number < _endNumber)
            {
                apply = true;
            }
        }
        else
        {
            if (_startNumber <= number && number < _endNumber)
            {
                apply = true;
            }
        }

        if (apply)
        {
            aCar.ApplyAbilityBonusSpeed(_bonusSpeed, _bonusTime, _applyFX, _applySound);
            aCar.RpcApplyAbilityBonusSpeed(_bonusSpeed, _bonusTime, _id);
            aCar.RpcAbilityActivated(_id, 1f, false);
        }
    }

    private void SetNumber(int aLevelValue)
    {
        if (-1 != _startNumber)
        {
            return;
        }

        _startNumber = Random.Range(1, 101);
        _endNumber = _startNumber + aLevelValue;
        if (100 < _endNumber)
        {
            _reverse = true;
            _endNumber -= 100;
        }

        YPLog.Log("start = " + _startNumber + ", end = " + _endNumber + ", levelValue = " + aLevelValue + ", reverse = " + _reverse);
    }
}