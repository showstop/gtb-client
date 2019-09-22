using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ItemDrone : ItemPeriod 
{
    [SerializeField]
    private float _range;

    [SerializeField]
    private float _damageInterval;

    [SerializeField]
    private int _damageCount;

    [SerializeField]
    private GameObject _rotationGO;

    protected override void CommonUse()
    {
        _animation.Play(_useAnimName);

        //ToggleItemFX(true);
        //_owner.UseItemFX(_useItemFX, _useItemSound);
    }

    protected override IEnumerator CheckApplyTime()
    {
        yield return null;
    }

    protected override void ApplyEffect(CarController aCar)
    {   
        StartCoroutine(Detecting());
    }

    private IEnumerator Detecting()
    {
        float applyTime = 0f;
        bool findTarget = false;
        while (applyTime < _applyTime && !findTarget)
        {
            applyTime += Time.deltaTime;

            CarController near = _owner.GM.GetNearestCar(_owner);
            if (null != near)
            {
                float distance = Vector3.Distance(_owner.CarTransform.position, near.CarTransform.position);
                if (distance < _range)
                {
                    findTarget = true;
                    SetTarget(near);
                    RpcSetTarget(near.netId);

                    StartCoroutine(Damage(near));
                }
            }

            YPLog.Log("item Rotation = " + _itemGO.transform.localRotation);
            yield return null;
        }
        
        if (!findTarget)
        {
            RpcEndApply();

            // hide animation length = 0.5 sec
            yield return new WaitForSeconds(0.5f);
            _owner.RemoveItem();
            NetworkServer.Destroy(gameObject);
        }
    }

    private IEnumerator Damage(CarController aTarget)
    {
        float distance = Vector3.Distance(_owner.CarTransform.position, aTarget.CarTransform.position);
        float checkTime = 0f;
        int count = 0;
        while (distance < _range && count < _damageCount)
        {
            transform.LookAt(aTarget.CarTransform);

            checkTime += Time.deltaTime;
            if (_damageInterval <= checkTime)
            {
                ++count;
                aTarget.UpdateHP(-_damage, _owner, false);

                checkTime = 0f;                
            }
            
            distance = Vector3.Distance(_owner.CarTransform.position, aTarget.CarTransform.position);
            yield return null;
        }

        RpcEndApply();

        // hide animation length = 0.5 sec
        yield return new WaitForSeconds(0.5f);
        _owner.RemoveItem();
        NetworkServer.Destroy(gameObject);
    }
    
    [ClientRpc]
    public void RpcSetTarget(NetworkInstanceId aID)
    {
        GameObject go = ClientScene.FindLocalObject(aID);
        CarController cc = go.GetComponent<CarController>();
        SetTarget(cc);
        StartCoroutine(UpdateRotation(cc));
    }

    private void SetTarget(CarController aTarget)
    {   
        foreach (GameObject go in _fx)
        {
            if (null != aTarget)
            {
                go.SetActive(true);
                FTME02_LineHitPoint hit = go.GetComponentInChildren<FTME02_LineHitPoint>();
                hit.SetTargetPosition(aTarget, new Vector3(0f, 0.1f, 0f));
            }
            else
            {
                go.SetActive(false);
            }
        }
    }

    private IEnumerator UpdateRotation(CarController aTarget)
    {
        float timelimit = _damageInterval * (float)_damageCount;
        float checkTime = 0f;
        while (checkTime < timelimit)
        {
            transform.LookAt(aTarget.CarTransform);
            yield return null;

            checkTime += Time.deltaTime;
        }
    }

    [ClientRpc]
    public override void RpcEndApply()
    {
        SetTarget(null);
        _animation.Play(_hideAnimName);
    }

    internal override void Enhance(float aValue)
    {
        _damage = (int)aValue;
    }
}