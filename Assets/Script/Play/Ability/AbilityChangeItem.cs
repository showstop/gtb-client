using UnityEngine;
using System.Collections;

public class AbilityChangeItem : Ability 
{
    [SerializeField]
    private int _sourceItemID;

    [SerializeField]
    private int _changeItemID;

    internal override int ChangeItem(int aLevel, int aItemID, CarController aCar)
    {
        if (_sourceItemID != aItemID)
        {
            return -1;
        }

        aCar.RpcAbilityActivated(_id, 1f, false);
        return _changeItemID;
    }

    internal override void EnhanceItem(int aLevel, ref Item oItem)
    {
        oItem.Enhance(_levelValue[aLevel]);
    }
}