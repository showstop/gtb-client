using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Language : PopupComponent
{
    [SerializeField]
    private List<Toggle> _languageToggle = new List<Toggle>();

    private int _languageIndex;

    void OnEnable()
    {
        int languageIndex = PlayerPrefs.GetInt(Setting.KEY_STRING_LANGUAGE, 0);
        for (int index = 0; index < _languageToggle.Count; ++index)
        {
            _languageToggle[index].isOn = false;
        }

        _languageToggle[languageIndex].isOn = true;
    }

    public void SelectLanguage(int aLanguageIndex)
    {
        _languageIndex = aLanguageIndex;
    }

    public void Confirm()
    {
        EventManager.Instance.SendGameEvent(GameEventType.ChangeLanguage, _languageIndex);
        base.Close();
    }
}