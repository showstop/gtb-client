using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemBarrel : ItemMultiFire 
{
    internal override void SpawnFireUnit()
    {
        for (int index = 0; index < _level; ++index)
        {
            GameObject unitGO = Instantiate(_fireUnitPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            ItemFireUnit unit = unitGO.GetComponent<ItemFireUnit>();
            unit.SetUnitInfo(index, netId);

            NetworkServer.Spawn(unitGO);
        }
    }
}
