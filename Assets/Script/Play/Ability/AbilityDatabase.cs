using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityDatabase : Singleton<AbilityDatabase>
{
    private Dictionary<int, Ability> _abilityDB = new Dictionary<int, Ability>();

    // prevent new constructor
    protected AbilityDatabase() { }

    internal void Load(GameObject aGO)
    {
        GameObject go = Instantiate(aGO, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.parent = gameObject.transform;

        Ability[] abilities = go.GetComponentsInChildren<Ability>();
        for (int index = 0; index < abilities.Length; ++index)
        {
            if (_abilityDB.ContainsKey(abilities[index]._id))
                _abilityDB[abilities[index]._id] = abilities[index];
            else
                _abilityDB.Add(abilities[index]._id, abilities[index]);

            // TO DO : fix me later
            EventManager.Instance.SendGameEvent(GameEventType.LoadingClientData, (float)index / (float)abilities.Length);
        }
        
        EventManager.Instance.SendGameEvent(GameEventType.LoadingClientDataEnd);
    }

    private Ability FindAbility(int aID)
    {
        if (_abilityDB.ContainsKey(aID))
        {
            return _abilityDB[aID];
        }
        
        return null;
    }

    internal void ApplyAbilityAtStartGame(int aID, int aLevel, CarController aCar)
    {
        Ability target = FindAbility(aID);
        if (null != target)
        {
            target.ApplyAtStartGame(aLevel - 1, aCar);
        }
    }

    internal void ApplyAbilityAtEndGame(int aID, int aLevel, CarController aCar)
    {
        Ability target = FindAbility(aID);
        if (null != target)
        {
            target.ApplyAtEndGame(aLevel - 1, aCar);
        }
    }

    internal void ApplyAbilityAtLapCount(int aID, int aLevel, CarController aCar)
    {
        Ability target = FindAbility(aID);
        if (null != target)
        {
            target.ApplyAtLapCount(aLevel - 1, aCar);
        }
    }

    internal void ApplyAbilityAtZeroHP(int aID, int aLevel, CarController aCar)
    {
        Ability target = FindAbility(aID);
        if (null != target)
        {
            target.ApplyAtZeroHP(aLevel - 1, aCar);
        }
    }

    internal int ChangeItem(int aAbilityID, int aLevel, int aItemID, CarController aCar)
    {
        Ability target = FindAbility(aAbilityID);
        if (null == target)
        {
            return -1;
        }

        return target.ChangeItem(aLevel, aItemID, aCar);
    }

    internal void EnhanceItem(int aAbilityID, int aLevel, ref Item oItem)
    {
        Ability target = FindAbility(aAbilityID);
        if (null == target)
        {
            return;
        }

        target.EnhanceItem(aLevel - 1, ref oItem);
    }

    internal void ChangeDamage(int aID, int aLevel, ref int oDamage, bool aCollide, bool aAttack, CarController aCar)
    {
        Ability target = FindAbility(aID);
        if (null != target)
        {
            target.ChangeDamage(aLevel - 1, ref oDamage, aCollide, aAttack, aCar);
        }
    }

    internal float ChangeSlowTime(int aID, int aLevel, float aTime, bool aAttack)
    {
        Ability target = FindAbility(aID);
        if (null == target)
        {
            return aTime;            
        }

        return target.ChangeSlowTime(aLevel - 1, aTime, aAttack);
    }

    internal void GetItemBox(int aID, int aLevel, CarController aCar)
    {
        Ability target = FindAbility(aID);
        if (null != target)
        {   
            target.GetItemBox(aLevel - 1, aCar);
        }
    }

    internal GameObject GetApplyFX(int aID)
    {
        Ability target = FindAbility(aID);
        if( null == target)
        {
            return null;
        }

        return target._applyFX;
    }

    internal AudioClip GetApplySound(int aID)
    {
        Ability target = FindAbility(aID);
        if (null == target)
        {
            return null;
        }

        return target._applySound;
    }

    internal void StopAbility(int aID)
    {
        Ability target = FindAbility(aID);
        if (null != target)
        {
            target.Stop();
        }
    }
}
