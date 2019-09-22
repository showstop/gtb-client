using UnityEngine;
using System.Collections;

public class AbilityDecreaseCarExplosionTime : Ability 
{
    internal override void ApplyAtStartGame(int aLevel, CarController aCar)
    {
        aCar._explosionTime -= Constants.EXPLOSION_TIME * _levelValue[aLevel];
    }	
}