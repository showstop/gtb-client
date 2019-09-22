using UnityEngine;
using System.Collections;

public class ItemArtisanHands : ItemPeriod 
{
    [SerializeField]
    private float _healingTime;

    [SerializeField]
    private float _healingPercentage;

    protected override void ApplyEffect(CarController aCar)
    {
        StartCoroutine(Heal());        
    }

    private IEnumerator Heal()
    {
        yield return new WaitForSeconds(_healingTime);

        int heal = (int)((float)_owner._maxHP * _healingPercentage);        
        _owner.UpdateHP(_owner._maxHP, null, false);
    }

    internal override void Enhance(float aValue)
    {
        _healingPercentage = aValue;
    }
}
