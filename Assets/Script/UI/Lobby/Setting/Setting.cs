using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SmartLocalization;

public class Setting : PopupComponent
{   
    public const string KEY_STRING_BGM = "BGM";
    public const string KEY_STRING_EFFECTSOUND = "EffectSound";
    public const string KEY_STRING_AIMATCHING = "AIMatching";
    public const string KEY_STRING_PUSHALARM = "PushAlarm";
    public const string KEY_STRING_LANGUAGE = "Language";
    
    public enum SoundVolume
    {
        Mute,
        Half,
        Full,
    };

    [SerializeField]
    private List<Toggle> _bgm = new List<Toggle>();

    [SerializeField]
    private List<Toggle> _effectSound = new List<Toggle>();

    [SerializeField]
    private Toggle _aiMatching;

    [SerializeField]
    private Toggle _pushAlarm;

    [SerializeField]
    private TextSelector _language;

    [SerializeField]
    private GameObject _guestLogin;

    [SerializeField]
    private Text _guestID;

    [SerializeField]
    private GameObject _facebookLogin;

    [SerializeField]
    private Text _playerID;

    [SerializeField]
    private Text _gameVersion;

    private static string[] _languageString = { "en", "ko", "zh-CN", "ja", "es", "de" };    

    void Start()
    {
        for (int index = 0; index < _bgm.Count; ++index)
        {
            _bgm[index].isOn = false;
        }
        _bgm[PlayerPrefs.GetInt(KEY_STRING_BGM, 0)].isOn = true;

        for (int index = 0; index < _effectSound.Count; ++index)
        {
            _effectSound[index].isOn = false;
        }
        _effectSound[PlayerPrefs.GetInt(KEY_STRING_EFFECTSOUND, 0)].isOn = true;

        _aiMatching.isOn = (PlayerPrefs.GetInt(KEY_STRING_AIMATCHING, 0) == 1) ? true : false;
        _pushAlarm.isOn = (PlayerPrefs.GetInt(KEY_STRING_PUSHALARM, 0) == 1) ? true : false;
        _language.SetText(PlayerPrefs.GetInt(KEY_STRING_LANGUAGE, 0));

        // TO DO : loginType
        Constants.LoginType loginType = Constants.LoginType.FACEBOOK;
        if (Constants.LoginType.FACEBOOK == loginType)
        {
            _guestLogin.SetActive(false);

            //_playerID.text = PlayerDataRepository.Instance.PlayerProfileInfo.GetGameNick();
            _facebookLogin.SetActive(true);
        }
        else
        {
            _facebookLogin.SetActive(false);

            //_guestID.text = PlayerDataRepository.Instance.PlayerProfileInfo.GetGameNick();
            _guestLogin.SetActive(true);
        }
    }

    public override void OnHandleEvent(GameEventType gameEventType, params System.Object[] args)
    {
        switch (gameEventType)
        {
            case GameEventType.ChangeLanguage:
                int languageIndex = (int)args[0];
                PlayerPrefs.SetInt(KEY_STRING_LANGUAGE, languageIndex);

                _language.SetText(languageIndex);
                string language = GetLanguageString(languageIndex);
                LanguageManager.Instance.ChangeLanguage(language);                

                break;
        }
    }

    public void BGM(int aVolume)
    {
        PlayerDataRepository.Instance.BGMVolume = (SoundVolume)aVolume;
        PlayerPrefs.SetInt(KEY_STRING_BGM, aVolume);
    }

    public void EffectSound(int aVolume)
    {
        PlayerDataRepository.Instance.EffectSoundVolume = (SoundVolume)aVolume;
        PlayerPrefs.SetInt(KEY_STRING_EFFECTSOUND, aVolume);
    }

    public void AIMatching()
    {
        PlayerDataRepository.Instance.AIMatching = _aiMatching.isOn;
        PlayerPrefs.SetInt(KEY_STRING_AIMATCHING, _aiMatching.isOn ? 1 : 0);
    }

    public void PushAlarm()
    {
        PlayerPrefs.SetInt(KEY_STRING_PUSHALARM, _pushAlarm.isOn ? 1 : 0);
        // TO DO : send info to server.
    }

    public void LanguageSelect()
    {
        EventManager.Instance.SendGameEvent(GameEventType.ShowLanguageSelection);
    }

    public void FacebookLogin()
    {
        // TO DO :
    }

    public void FacebookLogout()
    {
        // TO DO :
    }

    public static string GetLanguageString(int aIndex)
    {
        if (0 > aIndex || aIndex >= _languageString.Length)
        {
            return _languageString[0];
        }

        return _languageString[aIndex];
    }
}