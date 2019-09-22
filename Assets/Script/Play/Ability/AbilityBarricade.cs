using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AbilityBarricade : Ability 
{
    [SerializeField]
    private GameObject _barricadePrefab;

    internal override void ApplyAtZeroHP(int aLevel, CarController aCar)
    {
        int laneIndex = aCar._laneNO - 1;
        for (int index = 0; index < aCar._swc.Spline.LaneCount; ++index)
        {
            if (laneIndex == index)
            {
                continue;
            }

            GameObject go = Instantiate(_barricadePrefab, Vector2.zero, Quaternion.identity) as GameObject;
            ItemBarricade barricade = go.GetComponent<ItemBarricade>();
            barricade.Enhance(_levelValue[aLevel]);
            barricade.GiveTo(aCar);
            barricade._laneNO = index + 1;
            
            NetworkServer.Spawn(go);
        }
    }
}