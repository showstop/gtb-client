using UnityEngine;
using System.Collections;

public class AbilityIgnoreDamage : Ability 
{
    private int _startNumber = -1;
    private int _endNumber = 0;
    private bool _reverse = false;

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

        SetRange((int)_levelValue[aLevel]);
        
        int number = Random.Range(1, 101);
        if (_reverse)
        {
            if (_endNumber <= number && number < _startNumber)
            {
                oDamage = 0;
                aCar.RpcAbilityActivated(_id, 1f, false);
            }
        }
        else
        {
            if (_startNumber <= number && number < _endNumber)
            {
                oDamage = 0;
                aCar.RpcAbilityActivated(_id, 1f, false);
            }
        }
    }

    private void SetRange(int aRange)
    {
        if (-1 != _startNumber)
        {
            return;
        }

        _startNumber = Random.Range(1, 101);
        _endNumber = _startNumber + aRange;
        if (100 < _endNumber)
        {
            _reverse = true;
            _endNumber -= 100;
        }
    }
}