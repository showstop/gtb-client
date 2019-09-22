using UnityEngine;
using System.Collections;

public class ItemDashTurbo : ItemTurboVariation
{
    internal override void Enhance(float aValue)
    {
        _bonusSpeedApplyTime = aValue;
        _applyTime = aValue;
    }
}