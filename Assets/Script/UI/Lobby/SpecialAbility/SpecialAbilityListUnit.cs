using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SmartLocalization;

public class SpecialAbilityListUnit : MonoBehaviour 
{
    [SerializeField]
    private Image _mainFrame;

    [SerializeField]
    private List<Color> _mainFrameColor;        // index 0 : unequip, index 1 : equip

    [SerializeField]
    private ImageSelector _iconBG;              // index 0 : unequip, index 1 : equip

    [SerializeField]
    private ImageSelector _icon;

    [SerializeField]
    private TextSelector _name;

    [SerializeField]
    private Text _level;

    [SerializeField]
    private TextSelector _desc;

    [SerializeField]
    private GameObject _buy;

    [SerializeField]
    private Text _buyPrice;

    [SerializeField]
    private GameObject _levelUp;

    [SerializeField]
    private Text _levelUpPrice;

    [SerializeField]
    private GameObject _maxLevel;

    [SerializeField]
    private GameObject _levelLimit;

    [SerializeField]
    private Text _limitLevel;

    [SerializeField]
    private Button _canEquip;

    [SerializeField]
    private GameObject _equipped;

    public int ID { get; private set; }
    private int _currentLevel;

    internal void UpdateInfo(PlayerDataRepository.AbilityInfo aInfo)
    {
        ID = aInfo.AbilityId;
        _currentLevel = aInfo.Level;

        bool equipped = PlayerDataRepository.Instance.EquippedSpecialAbility(ID);
        _mainFrame.color = equipped ? _mainFrameColor[1] : _mainFrameColor[0];
        _iconBG.SetImage(equipped ? 1 : 0);
        _icon.SetImage(ID);
        _name.SetText(ID);
        _level.text = string.Format("{0}{1}", _currentLevel, LanguageManager.Instance.GetTextValue(Constants.LOCALIZATION_KEY_GLOSSARY_LEVEL));
        
        HideAllLeftButtons();
        HideAllRightButtons();

        var nextLevel = 1;
        SpecialAbilityData data = AssetBundleLoader.Instance.GetSpecialAbilityData(ID);
        if (0 < _currentLevel)
        {   
            if (_currentLevel == data._maxLevel)
            {
                _maxLevel.SetActive(true);
                nextLevel = data._maxLevel;
            }
            else
            {
                _levelUpPrice.text = StringFormat.NumberWithComma(data._levelUpPrice[_currentLevel - 1]);
                _levelUp.SetActive(true);
                nextLevel = _currentLevel + 1;
            }

            if (equipped)
            {
                _equipped.SetActive(true);
            }
            else
            {
                _canEquip.interactable = true;
                _canEquip.gameObject.SetActive(true);
            }
        }
        else
        {
            _buyPrice.text = StringFormat.NumberWithComma(data._price);
            var playerLevel = Constants.GetPlayerLevelByExp(PlayerDataRepository.Instance.MyPlayerInfo.Exp);
            if (data._unlockLevel <= playerLevel)
            {
                _buy.SetActive(true);                
                _levelLimit.SetActive(false);

                _canEquip.interactable = false;
                _canEquip.gameObject.SetActive(true);
            }
            else
            {
                _buy.SetActive(false);
                _limitLevel.text = string.Format("{0}{1}", data._unlockLevel, 
                    LanguageManager.Instance.GetTextValue(Constants.LOCALIZATION_KEY_GLOSSARY_LEVEL));
                _levelLimit.SetActive(true);
            }
        }
        
        string descKey = _desc.GetKey(ID);
        var descFormat = LanguageManager.Instance.GetTextValue(descKey);
        var descText = string.Format(descFormat, nextLevel * 10);
        _desc.SetText(descText);
    }

    private void HideAllLeftButtons()
    {
        _buy.SetActive(false);
        _levelUp.SetActive(false);
        _maxLevel.SetActive(false);
    }

    private void HideAllRightButtons()
    {
        _levelLimit.SetActive(false);
        _canEquip.gameObject.SetActive(false);
        _equipped.SetActive(false);
    }

    public void Buy()
    {
        EventManager.Instance.SendGameEvent(GameEventType.BuySpecialAbility, ID);
    }

    public void LevelUp()
    {
        EventManager.Instance.SendGameEvent(GameEventType.BuySpecialAbility, ID, _currentLevel);
    }

    public void Equip()
    {
        EventManager.Instance.SendGameEvent(GameEventType.EquipSpecialAbility, ID);
    }
}