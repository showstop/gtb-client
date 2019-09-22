using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AbilityCooltimeUse : AbilityCooltime
{
    [SerializeField]
    private GameObject _itemPrefab;

    protected override IEnumerator CheckCooltime(float aCooltime, CarController aCar)
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
                int laneIndex = aCar._laneNO - 1;
                for (int index = 0; index < aCar._swc.Spline.LaneCount; ++index)
                {
                    if (laneIndex == index)
                    {
                        continue;
                    }

                    GameObject go = Instantiate(_itemPrefab, Vector2.zero, Quaternion.identity) as GameObject;
                    ItemBarricade barricade = go.GetComponent<ItemBarricade>();
                    barricade.GiveTo(aCar);
                    barricade._laneNO = index + 1;

                    NetworkServer.Spawn(go);
                }

                cooltime = 0f;
            }

            aCar.RpcAbilityActivated(_id, gauge, false);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
