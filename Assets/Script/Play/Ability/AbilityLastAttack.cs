using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class AbilityLastAttack : Ability
{
    [SerializeField]
    private GameObject _missle;

    [SerializeField]
    private Vector3 _startLoc;

    internal override void ApplyAtZeroHP(int aLevel, CarController aCar)
    {
        YPLog.Trace();

        aCar.RpcAbilityActivated(_id, 1f, false);

        List<CarController> others = aCar.GM.GetOtherPlayers(aCar);        
        for (int index = 0; index < others.Count; ++index)
        {
            Vector3 addVector = _startLoc;
            switch (index)
            {
                case 0: break;
                case 1: addVector += aCar.CarTransform.right * -0.2f; break;
                case 2: addVector += aCar.CarTransform.right * 0.2f; break;
            }

            GameObject go = Instantiate(_missle, Vector3.zero, Quaternion.identity) as GameObject;
            ItemLastAttack item = go.GetComponent<ItemLastAttack>();
            item.SetInfo(aCar, others[index], addVector);

            NetworkServer.Spawn(go);
        }
    }
}
