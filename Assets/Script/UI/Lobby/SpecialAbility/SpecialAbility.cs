using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpecialAbility : GUIComponent 
{
    [SerializeField]
    private List<SpecialAbilitySlot> _slots = new List<SpecialAbilitySlot>();

    [SerializeField]
    private List<SpecialAbilityListUnit> _specialAbilitylist = new List<SpecialAbilityListUnit>();

    void Awake()
    {
        UpdateSlotInfo();
        SetupList();
    }

    private void UpdateSlotInfo()
    {
        var slots = PlayerDataRepository.Instance.GetAbilitySlot();
        for (short index = 0; index < _slots.Count; ++index)
        {
            short slotNo = (short)(index + 1);
            int abilityID = slots[slotNo];
            _slots[index].SlotNO = slotNo;
            _slots[index].UpdateInfo(abilityID);
        }
    }

    private void SetupList()
    {
        var list = PlayerDataRepository.Instance.GetAbilityList();        
        for (int index = 0; index < list.Count; ++index)
        {
            _specialAbilitylist[index].UpdateInfo(list[index]);
        }
    }

    public override void OnHandleEvent(GameEventType gameEventType, params System.Object[] args)
    {
        switch(gameEventType)
        {
            case GameEventType.AbilitySlotOpenAnsOK:
            case GameEventType.AbilityEquipAnsOK:
                UpdateSlotInfo();

                break;

            case GameEventType.AbilityAcquireAnsOK:
                var abilityId = (int)args[0];
                SpecialAbilityListUnit target = _specialAbilitylist.Find(
                    delegate (SpecialAbilityListUnit unit) { return unit.ID == abilityId; });

                var abilityInfo = PlayerDataRepository.Instance.GetAbilityInfo(abilityId);
                target.UpdateInfo(abilityInfo);

                break;

            case GameEventType.EquipSpecialAbility:
                for(int index = 0; index < _slots.Count; ++index)
                {
                    if (!_slots[index].Unlock)
                    {
                        continue;
                    }

                    _slots[index].EnableEquip((int)args[0]);
                }

                break;
        }
    }

    public void SelectEquipSlot()
    {
        for (int index = 0; index < _slots.Count; ++index)
        {
            _slots[index].EnableEquip(0);
        }
    }

    public void Unlock(int aIndex)
    {
        
        // TO DO : request buy special ability slot.
    }
}