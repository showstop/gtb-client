using UnityEngine;
using System.Collections;

public class AbilityNitroTurbo : AbilitySpeedBoost
{
    private float _gauge = 0f;
    private int _count = 0;

    internal override void GetItemBox(int aLevel, CarController aCar)
    {
        ++_count;
        _gauge = (float)_count / _levelValue[aLevel];

        aCar.RpcAbilityActivated(_id, _gauge, false);
        if (_levelValue[aLevel] == _count)
        {
            aCar.ApplyAbilityBonusSpeed(_bonusSpeed, _bonusTime, _applyFX, _applySound);
            aCar.RpcApplyAbilityBonusSpeed(_bonusSpeed, _bonusTime, _id);
            _count = 0;
        }
    }
}