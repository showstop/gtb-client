using UnityEngine;
using System.Collections;

public class ItemChaser : ItemMultiFire 
{
    internal override void AddFireUnit(ItemFireUnit aUnit, int aIndex)
    {
        aUnit.gameObject.transform.parent = _fireUnitLoc[aIndex];
        aUnit.gameObject.transform.localPosition = Vector3.zero;
        aUnit.gameObject.transform.localRotation = Quaternion.Euler(aUnit.GetAttachLocalRotation());

        _fireUnit[aIndex] = aUnit;
        _fireUnit[aIndex].SetOwner(_owner);
        _fireUnit[aIndex].SetItemInfo(_damage, _speed, _bonusSpeed, _bonusSpeedApplyTime);
        _fireUnit[aIndex].SetFXInfo(_useItemFX, _useItemSound, _applyItemFX, _applyItemSound);
    }
}
