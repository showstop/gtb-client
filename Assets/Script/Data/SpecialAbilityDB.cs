using UnityEngine;
using System.Collections.Generic;

[System.SerializableAttribute]
public class SpecialAbilityData
{
    public int _id;
    public int _unlockLevel;
    public int _price;    
    public int _maxLevel;
    public int[] _levelUpPrice;
}

public class SpecialAbilityDB : ScriptableObject
{
    public List<SpecialAbilityData> _data = new List<SpecialAbilityData>();
}