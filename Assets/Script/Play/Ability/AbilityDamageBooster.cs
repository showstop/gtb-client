using UnityEngine;
using System.Collections;

public class AbilityDamageBooster : AbilitySpeedBoost 
{
    private float _gauge = 0f;
    private int _count = 0;

    internal override void ChangeDamage(int aLevel, ref int oDamage, bool aCollide, bool aAttack, CarController aCar)
    {
 	    if (aCollide)
        {
            return;
        }

        if (aAttack)
        {
            return;
        }

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
