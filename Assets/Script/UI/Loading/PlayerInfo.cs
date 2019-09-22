using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class SpecialAbilityInfo
{
    public ImageSelector _level;
    public ImageSelector _icon;
};

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private ImageSelector _nation;

    [SerializeField]
    private Text _name;

    [SerializeField]
    private ImageSelector _carClass;

    [SerializeField]
    private ImageSelector _carIcon;

    [SerializeField]
    private ImageSelector _carName;

    [SerializeField]
    private List<SpecialAbilityInfo> _specialAbility = new List<SpecialAbilityInfo>();

    [SerializeField]
    private List<GameObject> _specialAbilityGo = new List<GameObject>();

    internal void SetInfo(ulong aPlayerNo)
    {
        // TODO temp remove            

//        protocol.player_profile_info profile = PlayerDataRepository.Instance.GetMatchPlayerProfile(aPlayerNo);
//        //_nation.SetImage(profile.GetNationCode());
//        _name.text = profile.GetGameNick();
//
//        protocol.vehicle vehicle = PlayerDataRepository.Instance.GetMatchPlayerVehicle(aPlayerNo);
//        _carClass.SetImage((int)vehicle.GetLevel());
//        _carIcon.SetImage(vehicle.GetVehicleNo());
//        _carName.SetImage(vehicle.GetVehicleNo());
//
//        int index = 0;
//        Dictionary<protocol.ability_slot_no, int> abilitySlot = PlayerDataRepository.Instance.GetMatchPlayerEquippedSpecialAbility(aPlayerNo).GetSlots();
//        foreach (int id in abilitySlot.Values)
//        {
//            protocol.ability ability = PlayerDataRepository.Instance.GetMatchPlayerSpecialAbility(aPlayerNo, id);
//            _specialAbility[index]._level.SetImage(ability.GetLevel());
//            _specialAbility[index]._icon.SetImage(id);
//            _specialAbilityGo[index].SetActive(true);
//
//            ++index;
//        }
    }
}