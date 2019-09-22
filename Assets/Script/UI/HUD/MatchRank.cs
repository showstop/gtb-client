using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MatchRank : MonoBehaviour
{
    [SerializeField]
    private GameObject _mine;

    [SerializeField]
    private ImageSelector _rank;

    [SerializeField]
    private ImageSelector _carIcon;

    [SerializeField]
    private ImageSelector _carName;

    [SerializeField]
    private ImageSelector _nation;

    [SerializeField]
    private Text _name;

    [SerializeField]
    private List<SpecialAbilityInfo> _specialAbility = new List<SpecialAbilityInfo>();

    [SerializeField]
    private List<GameObject> _specialAbilityGO = new List<GameObject>();

    [SerializeField]
    private Text _lapTime;

    internal void SetInfo(string aPlayerNo, int aRank, float aLapTime, bool aMine)
    {
        _mine.SetActive(aMine);
        //_rank.SetImage(aRank);
        
        // TODO temp remove
//        ulong playerNo = System.Convert.ToUInt64(aPlayerNo);
//        protocol.player_profile_info profile = PlayerDataRepository.Instance.GetMatchPlayerProfile(playerNo);
//        //_nation.SetImage(profile.GetNationCode());        
//        _name.text = profile.GetGameNick();
//
//        protocol.vehicle vehicle = PlayerDataRepository.Instance.GetMatchPlayerVehicle(playerNo);
//        //_carClass.SetImage((int)vehicle.GetLevel());
//        _carIcon.SetImage(vehicle.GetVehicleNo());
//        _carName.SetImage(vehicle.GetVehicleNo());
//
//        int index = 0;
//        Dictionary<protocol.ability_slot_no, int> abilitySlot = PlayerDataRepository.Instance.GetMatchPlayerEquippedSpecialAbility(playerNo).GetSlots();
//        foreach (int id in abilitySlot.Values)
//        {
//            protocol.ability ability = PlayerDataRepository.Instance.GetMatchPlayerSpecialAbility(playerNo, id);
//            _specialAbility[index]._level.SetImage(ability.GetLevel());
//            _specialAbility[index]._icon.SetImage(id);
//            _specialAbilityGO[index].SetActive(true);
//
//            ++index;
//        }
//
//        _lapTime.text = StringFormat.MinuteSecond((long)aLapTime);
    }
}