using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemTurboVariation : ItemPeriod 
{
    [SerializeField]
    private Animation _additionalAnimation;

    [SerializeField]
    private string _additionalUseAnimName;
    
    protected override void CommonUse()
    {
        _animation.Play(_useAnimName);
        _additionalAnimation.Play(_additionalUseAnimName);

        ToggleItemFX(true);
        _owner.UseItemFX(_useItemFX, _useItemSound);
    }

    protected override void ApplyEffect(CarController aCar)
    {
        aCar.ApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
        aCar.RpcApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
    }
}