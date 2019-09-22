using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ItemPhantom : Item
{
    [SerializeField]
    private float _lifeTime = 0f;

    internal override void Use()
    {
        YPLog.Trace();
        base.Use();

        if (NetworkServer.active)
        {
            StartCoroutine(CheckLifeTime());
        }
    }

    private IEnumerator CheckLifeTime()
    {
        while (_lifeTime > 0f)
        {
            yield return null;
            _lifeTime -= Time.deltaTime;
        }

        NetworkServer.Destroy(gameObject);
    }

    internal override void Collide(CarController aCar)
    {   
        ApplyEffect(aCar);
    }

    [Command]
    public void CmdCollideItem(NetworkInstanceId aNetID)
    {
        Item item = NetworkServer.FindLocalObject(aNetID).GetComponent<Item>();
        if (item != null)
        {
            item.CollideOtherItem();
        }
    }

    [Command]
    public void CmdCollideItemFireUnit(NetworkInstanceId aNetID)
    {
        ItemFireUnit unit = NetworkServer.FindLocalObject(aNetID).GetComponent<ItemFireUnit>();
        if (unit != null)
        {
            unit.CollideOtherItem();
        }
    }

    internal override void Enhance(float aValue)
    {
        _lifeTime = aValue;
    }
}