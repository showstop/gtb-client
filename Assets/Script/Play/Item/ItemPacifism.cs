﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ItemPacifism : ItemPeriod
{
    protected override IEnumerator CheckApplyTime()
    {
        yield return new WaitForSeconds(_applyTime);
        RpcEndApply();
        List<CarController> others = _owner.GM.GetOtherPlayers(_owner);
        for (int index = 0; index < others.Count; ++index)
        {
            others[index].RpcPacifism(false);
        }

        // hide animation length = 0.5 sec
        yield return new WaitForSeconds(0.5f);
        _owner.RemoveItem();
        NetworkServer.Destroy(gameObject);
    }

    protected override void ApplyEffect(CarController aCar)
    {
        List<CarController> others = _owner.GM.GetOtherPlayers(_owner);
        for (int index = 0; index < others.Count; ++index)
        {
            others[index].RpcPacifism(true);
        }
    }
}