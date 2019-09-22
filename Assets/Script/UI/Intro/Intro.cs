using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SmartLocalization;
using Facebook.Unity;
using Network;

public enum LoadingState
{
    CheckVersion,
    DownloadUpdate,
    LoadingClientData,
    LoadingClientDataEnd,
}

public class Intro : GUIComponent
{
    [SerializeField]
    protected Constants.LoginType _loginType;

    [SerializeField] private bool _isLocal;

    [SerializeField]
    private GameObject _abilityDB;

    [SerializeField]
    private GameObject _loading;    

    [SerializeField]
    private TextSelector _loadingStateText;

    [SerializeField]
    private Slider _loadingProgress;

    [SerializeField]
    private GameObject _login;

    [SerializeField]
    private Button _startGameButton;
    
    void Start()
    {
        LobbyRequest.Instance.SetDevMode(_isLocal);
        
        string language = Setting.GetLanguageString(PlayerPrefs.GetInt(Setting.KEY_STRING_LANGUAGE, 0));
        Debug.LogFormat("Language:{0}", language);
        
        LanguageManager.Instance.ChangeLanguage(language);
        LanguageManager.SetDontDestroyOnLoad();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        StartCoroutine(AssetBundleLoader.Instance.LoadAssetBundle(_abilityDB));
    }        

    public override void OnHandleEvent(GameEventType gameEventType, params System.Object[] args)
    {
        switch (gameEventType)
        {
            case GameEventType.DownloadUpdate:
            {
                _loadingStateText.SetText((int) LoadingState.DownloadUpdate);
                _loadingProgress.value = (float) args[0];

                break;
            }
            case GameEventType.LoadingClientData:
            {
                _loadingStateText.SetText((int) LoadingState.LoadingClientData);
                _loadingProgress.value = (float) args[0];

                break;
            }
            case GameEventType.LoadingClientDataEnd:
            {
                _loadingProgress.value = 1f;
                _loading.SetActive(false);

                _startGameButton.interactable = true;
                _login.SetActive(true);

                break;
            }
            case GameEventType.LobbyLoginAns:
            {
                var loginResult = (bool) args[0];
                if (loginResult)
                {
                    SceneManager.LoadScene("loading_lobby");
                }
                else
                {
                    // login failed.
                    
                }
                break;
            }   
        }
    }
    
    // Inter Button Event
    public void StartGame()
    {
        var playerUUID = SystemInfo.deviceUniqueIdentifier;
        var deviceType = SystemInfo.deviceType.ToString();
        var pushToken = "test token";         // TODO
        LobbyRequest.Instance.LoginReq(Constants.LoginType.GUEST, playerUUID, deviceType, pushToken);
    }
    
}