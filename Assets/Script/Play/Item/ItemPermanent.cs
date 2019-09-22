using UnityEngine;
using System.Collections;

public class ItemPermanent : Item 
{
    internal override void Collide(CarController aCar)
    {
        YPLog.Trace();

        // apply effect
        ApplyEffect(aCar);
    }
}