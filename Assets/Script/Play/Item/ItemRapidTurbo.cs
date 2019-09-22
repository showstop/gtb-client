using UnityEngine;
using System.Collections;

public class ItemRapidTurbo : ItemTurboVariation 
{
    internal override void Enhance(float aValue)
    {
        _bonusSpeed = aValue;
    }
}