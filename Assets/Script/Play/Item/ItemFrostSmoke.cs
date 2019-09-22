using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemFrostSmoke : ItemPeriod
{
    [SerializeField]
    private float _range;

    private List<CarController> apply = new List<CarController>();

    protected override void ApplyEffect(CarController aCar)
    {
        StartCoroutine(Frost());
    }

    private IEnumerator Frost()
    {
        List<CarController> otherPlayers = _owner.GM.GetOtherPlayers(_owner);
        float otherTF = 0f;
        float diff = 0f;
        float distance = 0f;
        while (true)
        {
            float tf = _owner._swc.TF;
            for (int index = 0; index < otherPlayers.Count; ++index)
            {
                if (apply.Contains(otherPlayers[index]))
                {
                    continue;
                }

                otherTF = otherPlayers[index]._swc.TF;
                if (otherTF < tf)
                {
                    diff = tf - otherTF;
                    distance = _owner._swc.Spline.TFToDistance(diff);
                    if (distance < _range)
                    {
                        apply.Add(otherPlayers[index]);

                        if (otherPlayers[index].ShieldDefence())
                        {
                            continue;
                        }

                        if (otherPlayers[index].DefenceSpeeding())
                        {
                            continue;
                        }

                        otherPlayers[index].ApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
                        otherPlayers[index].RpcApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
                    }
                }
                else if (0.95f < otherTF && 0.05f > tf)
                {
                    diff = 1f - otherTF + tf;
                    distance = _owner._swc.Spline.TFToDistance(diff);
                    if (distance < _range)
                    {
                        apply.Add(otherPlayers[index]);

                        if (otherPlayers[index].ShieldDefence())
                        {
                            continue;
                        }

                        if (otherPlayers[index].DefenceSpeeding())
                        {
                            continue;
                        }

                        otherPlayers[index].ApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
                        otherPlayers[index].RpcApplyItemBonusSpeed(_bonusSpeed, _bonusSpeedApplyTime, false);
                    }
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    internal override void Enhance(float aValue)
    {
        _bonusSpeedApplyTime = aValue;
    }
}