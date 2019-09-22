using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ItemLightningChain : ItemPeriod 
{
    [SerializeField]
    private float _range;

    [SerializeField]
    private GameObject _chainFX;

    [SerializeField]
    private Transform _chainFXParent;

    [SerializeField]
    private Vector3 _chainAttachPosition;

    [SerializeField]
    private float _chainingInterval;

    [ClientRpc]
    public override void RpcUse()
    {
        CommonUse();
        StartCoroutine(LightningChain(false));
    }

    protected override void CommonUse()
    {
        _animation[_useAnimName].speed = 5f;
        _animation.Play(_useAnimName);

        ToggleItemFX(true);
        _owner.UseItemFX(_useItemFX, _useItemSound);
    }

    protected override void ApplyEffect(CarController aCar)
    {
        StartCoroutine(LightningChain(true));
    }

    private IEnumerator LightningChain(bool aGiveDamage)
    {
        // TO DO : make fx ball bigger!!
        while (_animation.isPlaying)
        {
            yield return null;
        }

        List<CarController> targets = new List<CarController>();
        targets.Add(_owner);

        CarController first = Chaining(ref targets, _owner, true, aGiveDamage);
        if (null != first)
        {
            yield return new WaitForSeconds(_chainingInterval);
            CarController second = Chaining(ref targets, first, false, aGiveDamage);
            if (null != second)
            {
                yield return new WaitForSeconds(_chainingInterval);
                CarController third = Chaining(ref targets, second, false, aGiveDamage);
            }    
        }
    }

    private CarController Chaining(ref List<CarController> oTargets, CarController aStart, bool aOwner, bool aGiveDamage)
    {
        List<CarController> others = _owner.GM.GetOtherPlayers(aStart);
        float max = _range;
        CarController next = null;
        for (int index = 0; index < others.Count; ++index)
        {
            if (oTargets.Contains(others[index]))
            {
                continue;
            }

            float distance = Vector3.Distance(aStart.CarTransform.position, others[index].CarTransform.position);
            if (distance < max)
            {
                next = others[index];
                max = distance;
            }
        }

        if (null == next)
        {
            return null;
        }

        oTargets.Add(next);

        GameObject chainFX = Instantiate(_chainFX, Vector3.zero, Quaternion.identity) as GameObject;
        if (aOwner)
        {
            chainFX.transform.parent = _chainFXParent;
            chainFX.transform.localPosition = Vector3.zero;
        }
        else
        {
            chainFX.transform.parent = aStart.CarTransform;
            chainFX.transform.localPosition = _chainAttachPosition;
        }

        FTME02_LineHitPoint hit = chainFX.GetComponentInChildren<FTME02_LineHitPoint>();
        hit.SetTargetPosition(next, _chainAttachPosition);

        if (aGiveDamage)
        {
            next.UpdateHP(-_damage, _owner, false);
        }
        
        YPLog.Log("find target = " + next);
        return next;
    }

    internal override void Enhance(float aValue)
    {
        _damage = (int)aValue;
    }
}