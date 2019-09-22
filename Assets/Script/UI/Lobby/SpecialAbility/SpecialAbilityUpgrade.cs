using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;

public class SpecialAbilityUpgrade : PopupComponent 
{
    [SerializeField]
    private Text _currentLevel;

    [SerializeField]
    private TextSelector _currentDesc;

    [SerializeField]
    private Text _nextLevel;

    [SerializeField]
    private TextSelector _nextDesc;

    [SerializeField]
    private TextSelector _notice;

    [SerializeField]
    private Text _price;

    private int _id;

	internal void SetInfo(int aID, int aLevel)
    {
        _id = aID;
        
        _currentLevel.text = string.Format("{0}{1}", aLevel, LanguageManager.Instance.GetTextValue(Constants.LOCALIZATION_KEY_GLOSSARY_LEVEL));
        string format = _currentDesc.GetKey(_id);
        _currentDesc.SetText(string.Format(format, aLevel));

        _nextLevel.text = string.Format("{0}{1}", aLevel + 1, LanguageManager.Instance.GetTextValue(Constants.LOCALIZATION_KEY_GLOSSARY_LEVEL));
        format = _nextDesc.GetKey(_id);
        _nextDesc.SetText(string.Format(format, aLevel + 1));

        _notice.SetText(string.Format(LanguageManager.Instance.GetTextValue(Constants.LOCALIZATION_KEY_CAR_SPECIALABILITY_SHOULDUPGRADE), LanguageManager.Instance.GetTextValue(_notice.GetKey(_id))));        
        _price.text = StringFormat.NumberWithComma(AssetBundleLoader.Instance.GetSpecialAbilityLevelUpPrice(_id, aLevel));
    }

    public void LevelUp()
    {
        //LSConnector.Instance.AbilityAcquireReq(_id);
    }
}