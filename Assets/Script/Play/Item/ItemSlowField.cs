using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSlowField : ItemTurboVariation
{
    [SerializeField]
    private float _range;

    [SerializeField]
    private float _slowFieldBonusSpeed;

    [SerializeField]
    private float _slowFieldBonusSpeedApplyTime;

    internal override void Use()
    {
        base.Use();
        
        StartCoroutine(SlowField());
    }

    private IEnumerator SlowField()
    {
        List<CarController> apply = new List<CarController>();
        while (true)
        {
            yield return null;

            for (int index = 0; index < Constants.MAX_PLAYER_NUM; ++index)
            {
                CarController target = _owner.GM.GetCarWithRank(index + 1);
                if (null == target || _owner == target || apply.Contains(target))
                {
                    continue;
                }

                float distance = Vector3.Distance(target.CarTransform.position, _owner.CarTransform.position);
                if (_range < distance)
                {
                    continue;
                }

                if (target.ShieldDefence())
                {
                    continue;
                }

                if (target.DefenceSpeeding())
                {
                    continue;
                }

                apply.Add(target);
                target.ApplyItemBonusSpeed(_slowFieldBonusSpeed, _slowFieldBonusSpeedApplyTime, true);
                target.RpcApplyItemBonusSpeed(_slowFieldBonusSpeed, _slowFieldBonusSpeedApplyTime, true);
            }
        }
    }

    internal override void Enhance(float aValue)
    {
        _slowFieldBonusSpeedApplyTime = aValue;
    }
}