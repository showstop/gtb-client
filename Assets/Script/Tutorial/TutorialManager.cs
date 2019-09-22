using UnityEngine;
using SmartLocalization;

public class TutorialManager : Singleton<TutorialManager>
{
    public GameObject _tutorial;
    public GameObject _lobbyObject;
    public GameObject _ingameObject;

    public TutorialIngame _ingameHUD;
    public TutorialLobby _lobbyHUD;

    public int _firstCarNumber = -1;

    internal void Init()
    {
        var progress = PlayerDataRepository.Instance.TutorialProgress;
        if(progress == Constants.TutorialProgress.ALL_COMPLETE)
        {
            TutorialConstants.LobbyState = TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_NULL;
            TutorialConstants.IngameState = TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_NULL;
            TutorialConstants._tutorialPlaying = false;
        }
        else if (progress == Constants.TutorialProgress.CONTROL_TUTORIAL_END ||
                 progress == Constants.TutorialProgress.LOBBY_TUTORIAL_BEGIN)
        {
            TutorialConstants.LobbyState = TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_START;
            TutorialConstants.IngameState = TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_END;
            TutorialConstants._tutorialPlaying = true;
        }
        else if(progress == Constants.TutorialProgress.INITIALIZE || 
                progress == Constants.TutorialProgress.CONTROL_TUTORIAL_BEGIN)
        {
            TutorialConstants.LobbyState = TutorialConstants.TUTORIAL_LOBBY_STATE.TUTO_LOBBY_START;
            TutorialConstants.IngameState = TutorialConstants.TUTORIAL_INGAME_STATE.TUTO_INGAME_START;
            TutorialConstants._tutorialPlaying = true;
        }
    }

    internal void LobbyInit()
    {
        GameObject tutorialObj = GameObject.FindGameObjectWithTag(TutorialConstants.TUTORIAL_TAG);
        _tutorial = tutorialObj;

        GameObject lobbyObj = GameObject.FindGameObjectWithTag(TutorialConstants.TUTORIAL_TAG_LOBBYHUD);
        _lobbyObject = lobbyObj;

        _lobbyHUD = _lobbyObject.GetComponent<TutorialLobby>();
        _lobbyHUD.Init();
    }

    internal void IngameInit()
    {
        PlayerDataRepository.Instance.TutorialProgress = Constants.TutorialProgress.CONTROL_TUTORIAL_BEGIN;
        GameObject ingameObj = GameObject.FindGameObjectWithTag(TutorialConstants.TUTORIAL_TAG_INGAMEHUD);
        _ingameObject = ingameObj;

        _ingameHUD = ingameObj.GetComponent<TutorialIngame>();
        _ingameHUD.Init();
    }

    internal void TutorialIngameEnd(bool aShow)
    {
        if (PlayerDataRepository.Instance.TutorialProgress == Constants.TutorialProgress.CONTROL_TUTORIAL_END)
        {
            TutorialConstants._tutorialAbility = aShow;
            _ingameObject.SetActive(aShow);
        }
    }

    internal void TutorialLobbyEnd(bool aShow)
    {
        if (PlayerDataRepository.Instance.TutorialProgress == Constants.TutorialProgress.ALL_COMPLETE)
        {
            TutorialConstants._tutorialAbility = aShow;
            TutorialConstants._tutorialPlaying = aShow;
            _tutorial.SetActive(aShow);
            _lobbyObject.SetActive(aShow);
        }
    }
}
