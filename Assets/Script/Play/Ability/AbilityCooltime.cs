using UnityEngine;
using System.Collections;

public class AbilityCooltime : Ability 
{
    [SerializeField]
    protected int _itemID;

    internal override void ApplyAtStartGame(int aLevel, CarController aCar)
    {
        YPLog.Trace();
        StopAllCoroutines();
        StartCoroutine(CheckCooltime(_levelValue[aLevel], aCar));
    }

    protected virtual IEnumerator CheckCooltime(float aCooltime, CarController aCar)
    {
        YPLog.Trace();
        YPLog.Log("cooltime = " + aCooltime);
        while (!aCar._matchStart)
        {
            yield return null;
        }

        YPLog.Log("check!!");
        float cooltime = 0f;
        while (aCar._matchStart)
        {
            cooltime += 0.1f;            
            float gauge = cooltime / aCooltime;
            if (1f <= gauge)
            {
                aCar.GM.GiveSpecifiedItem(aCar, _itemID);
                cooltime = 0f;
            }

            aCar.RpcAbilityActivated(_id, gauge, false);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
