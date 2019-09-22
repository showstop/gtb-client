using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemMultiFire : Item
{
    [SerializeField]
    protected GameObject _fireUnitPrefab;

    [SerializeField]
    protected Transform[] _fireUnitLoc;

    protected ItemFireUnit[] _fireUnit = new ItemFireUnit[6];

    [SyncVar]
    protected int _level = 1;
    protected int _fireCount = 0;

    internal override void SetLevel(int aLevel)
    {
        _level = aLevel;
    }

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

    internal virtual void AddFireUnit(ItemFireUnit aUnit, int aIndex)
    {
        aUnit.gameObject.transform.parent = _fireUnitLoc[aIndex];
        aUnit.gameObject.transform.localPosition = Vector3.zero;
        aUnit.gameObject.transform.localRotation = Quaternion.identity;

        _fireUnit[aIndex] = aUnit;
        _fireUnit[aIndex].SetOwner(_owner);
        _fireUnit[aIndex].SetItemInfo(_damage, _speed, _bonusSpeed, _bonusSpeedApplyTime);
        _fireUnit[aIndex].SetFXInfo(_useItemFX, _useItemSound, _applyItemFX, _applyItemSound);

        if (0 == aIndex)
        {
            aUnit.AttachToCar(_owner);
        }
    }

    internal bool CanFire()
    {
        if (_level > _fireCount)
        {
            return true;
        }

        return false;
    }

    internal override void ToggleShow()
    {
        _itemGO.SetActive(_show);
        if (_show)
        {
            if (null != _animation)
            {
                _animation.Play(_showAnimName);
            }

            if (_owner.isLocalPlayer)
            {
                _owner._updateItemUseButton = false;
            }
        }
    }

    internal override void Use()
    {
        YPLog.Trace();

        if (0 == _fireCount)
        {
            _owner.UpdatePlayData((short)Constants.StatKey.USE_INGAME_ITEM, 1);
        }

        ++_fireCount;
        FireUnit(_fireCount);
        RpcFireUnit(_fireCount);
        if (_level == _fireCount)
        {
            StartCoroutine(DestroyItem());
        }
    }

    [ClientRpc]
    public virtual void RpcFireUnit(int aFireCount)
    {
        if (NetworkServer.localClientActive)
        {
            return;
        }

        FireUnit(aFireCount);
        UpdateItemUseButton();
    }

    protected virtual void FireUnit(int aFireCount)
    {
        YPLog.Trace();
        YPLog.Log("fire count = " + _fireCount);

        _fireCount = aFireCount;
        _fireUnit[_fireCount - 1].Fire();

        if (_level == _fireCount)
        {
            _animation.Play(_hideAnimName);
        }
        else
        {
            _fireUnit[_fireCount].AttachToCar(_owner);
        }
    }

    protected virtual void UpdateItemUseButton()
    {
        if (!_owner.isLocalPlayer)
        {
            return;
        }

        if (_level == _fireCount + 1)
        {
            _owner._updateItemUseButton = true;
        }
        else
        {
            _owner._updateItemUseButton = false;
        }
    }

    protected virtual IEnumerator DestroyItem()
    {
        YPLog.Trace();

        _using = true;
        while (!_animation.isPlaying)
        {
            yield return null;
        }

        while (_animation.isPlaying)
        {
            yield return null;
        }

        _owner.RemoveItem();
        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    public override void RpcStolenByMagnet(NetworkInstanceId aNetID)
    {
        transform.parent = null;
        transform.localPosition = Vector3.zero;
        transform.position = _owner.CarTransform.position + new Vector3(0f, 0.2f, 0f);

        _fireUnit[_fireCount].gameObject.transform.parent = gameObject.transform;
        _fireUnit[_fireCount].gameObject.transform.localPosition = Vector3.zero;

        ItemMagnet magnet = ClientScene.FindLocalObject(aNetID).GetComponent<ItemMagnet>();
        StartCoroutine(MoveTowardMagnet(magnet));
    }
}