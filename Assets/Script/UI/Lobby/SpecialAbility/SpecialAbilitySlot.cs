using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Network;
using SmartLocalization;

public class SpecialAbilitySlot : MonoBehaviour
{
    [SerializeField]
    private int _slotNO;

    public int SlotNO
    {
        get { return _slotNO; }
        set
        {
            _slotNO = value;
            SetSlotInfo();
        }
    }

    [SerializeField]
    private GameObject _lock;

    [SerializeField] 
    private GameObject _itemBG;

    [SerializeField]
    private int _unlockLevel;

    [SerializeField]
    private Text _unlockText;

    [SerializeField]
    private string _unlockTextKeyword;

    [SerializeField]
    private Text _unlockPrice;

    [SerializeField]
    private Button _buy;

    [SerializeField]
    private GameObject _empty;

    [SerializeField]
    private GameObject _equip;

    [SerializeField]
    private ImageSelector _icon;

    [SerializeField]
    private TextSelector _name;

    [SerializeField]
    private Text _level;

    [SerializeField]
    private TextSelector _desc;

    [SerializeField]
    private GameObject _equipArrow;

    [SerializeField]
    private Button _equipButton;

    private int _equipID = -1;
    public bool Unlock { get { return !_lock.activeInHierarchy; } }
    private int _prepareEquipID = -1;

    private void SetSlotInfo()
    {
        HideAll();

        var slotInfo = PlayerDataRepository.Instance.GetAbilitySlotUnlockInfo(_slotNO);
        _unlockLevel = slotInfo.UnlockLevel;
        _unlockPrice.text = slotInfo.DiamondPrice.ToString();

        var slotMap = PlayerDataRepository.Instance.GetAbilitySlot();
        if (_slotNO >= 2)
        {
            if (slotMap[(short)(_slotNO - 1)] == -1)
            {
                _unlockText.text = string.Format(LanguageManager.Instance.GetTextValue(_unlockTextKeyword), _slotNO -1);
                _unlockText.gameObject.SetActive(true);
            }
        }
    }

    internal void UpdateInfo(int aAbilityID)
    {
        if (-1 == aAbilityID)
        {
            _lock.SetActive(true);
            return;
        }
        else if( 0 == aAbilityID)
        {
            // empty
            _empty.SetActive(true);
            return;
        }

        // equip
        _equip.SetActive(true);
        var specialAbility = PlayerDataRepository.Instance.GetAbilityInfo(aAbilityID);
        _equipID = aAbilityID;
        _icon.SetImage(_equipID);
        _name.SetText(_equipID);
        _level.text = string.Format("{0}{1}", specialAbility.Level, 
            LanguageManager.Instance.GetTextValue(Constants.LOCALIZATION_KEY_GLOSSARY_LEVEL));
        
        string descKey = _desc.GetKey(_equipID);
        var descFormat = LanguageManager.Instance.GetTextValue(descKey);
        var descText = string.Format(descFormat, specialAbility.Level * 10);
        _desc.SetText(descText);
    }

    private void HideAll()
    {
        _itemBG.SetActive(false);
        _lock.SetActive(false);
        _empty.SetActive(false);
        _equip.SetActive(false);
    }

    internal void EnableEquip(int aEquipID)
    {
        if (0 < aEquipID)
        {
            _itemBG.SetActive(true);
            _equipButton.interactable = true;
            _equipArrow.SetActive(true);

            _prepareEquipID = aEquipID;
        }
        else
        {
            _equipButton.interactable = false;
            _itemBG.SetActive(false);
            _equipArrow.SetActive(false);
        }
    }

    public void SelectEquipSlot()
    {
        LobbyRequest.Instance.AbilityEquipReq(_slotNO, _prepareEquipID);
    }

    public void UnlockSlot()
    {
        LobbyRequest.Instance.AbilitySlotOpenReq(_slotNO);
    }
}