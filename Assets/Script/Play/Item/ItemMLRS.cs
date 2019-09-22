using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemMLRS : ItemMultiFire 
{
    [SerializeField]
    private float _fireInterval;

    internal override void AddFireUnit(ItemFireUnit aUnit, int aIndex)
    {
        aUnit.gameObject.transform.parent = _fireUnitLoc[aIndex];
        aUnit.gameObject.transform.localPosition = Vector3.zero;
        aUnit.gameObject.transform.localRotation = Quaternion.identity;
        aUnit.gameObject.transform.localScale = Vector3.one;

        _fireUnit[aIndex] = aUnit;
        _fireUnit[aIndex].SetOwner(_owner);
        _fireUnit[aIndex].SetItemInfo(_damage, _speed, _bonusSpeed, _bonusSpeedApplyTime);
        _fireUnit[aIndex].SetFXInfo(_useItemFX, _useItemSound, _applyItemFX, _applyItemSound);
    }

    internal override void UseToTarget(CarController aTarget) 
    {
        _owner.UpdatePlayData((short)Constants.StatKey.USE_INGAME_ITEM, 1);
        RpcUseToTarget(aTarget.netId);
        StartCoroutine(DestroyItem());
    }

    [ClientRpc]
    public override void RpcUseToTarget(NetworkInstanceId aNetID)
    {
        ToggleItemFX(true);
        _owner.UseItemFX(_useItemFX, _useItemSound);

        CarController target = ClientScene.FindLocalObject(aNetID).GetComponent<CarController>();
        StartCoroutine(AutoFireToTarget(target));
    }

    private IEnumerator AutoFireToTarget(CarController aTarget)
    {
        while (_level != _fireCount)
        {
            _fireUnit[_fireCount].FireToTarget(aTarget);
            ++_fireCount;


            yield return new WaitForSeconds(_fireInterval);
        }

        _animation.Play(_hideAnimName);
    }

    protected override IEnumerator DestroyItem()
    {
        YPLog.Trace();

        _using = true;
        while (_level != _fireCount)
        {
            yield return null;
        }

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
}