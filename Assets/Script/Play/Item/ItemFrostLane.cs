using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemFrostLane : ItemMultiFire 
{
    internal override void SpawnFireUnit()
    {
        // fire unit count = level + 1
        for (int index = 0; index < _level + 1; ++index)
        {
            GameObject unitGO = Instantiate(_fireUnitPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            ItemFireUnit unit = unitGO.GetComponent<ItemFireUnit>();
            unit.SetUnitInfo(index, netId);

            NetworkServer.Spawn(unitGO);
        }
    }

    internal override void AddFireUnit(ItemFireUnit aUnit, int aIndex)
    {
        aUnit.gameObject.transform.parent = _fireUnitLoc[aIndex];
        aUnit.gameObject.transform.localPosition = Vector3.zero;
        aUnit.gameObject.transform.localRotation = Quaternion.identity;

        _fireUnit[aIndex] = aUnit;
        _fireUnit[aIndex].SetOwner(_owner);
        _fireUnit[aIndex].SetItemInfo(_damage, _speed, _bonusSpeed, _bonusSpeedApplyTime);
        _fireUnit[aIndex].SetFXInfo(_useItemFX, _useItemSound, _applyItemFX, _applyItemSound);
    }

    internal override void Use()
    {
        YPLog.Trace();

        if (0 == _fireCount)
        {
            _owner.UpdatePlayData((short)Constants.StatKey.USE_INGAME_ITEM, 1);
        }

        _fireCount = _level;
        FireUnit(_fireCount);
        RpcFireUnit(_fireCount);
        StartCoroutine(DestroyItem());
    }

    protected override void FireUnit(int aFireCount)
    {
        YPLog.Trace();
        YPLog.Log("fire count = " + _fireCount);

        _fireCount = aFireCount;
        for (int index = 0; index < _fireCount; ++index)
        {
            _fireUnit[index].Fire();
        }
    }

    protected override void UpdateItemUseButton()
    {
        if (!_owner.isLocalPlayer)
        {
            return;
        }

        _owner._updateItemUseButton = true;
    }

    protected override IEnumerator DestroyItem()
    {
        YPLog.Trace();

        _using = true;
        yield return null;

        _owner.RemoveItem();
        NetworkServer.Destroy(gameObject);
    }
}