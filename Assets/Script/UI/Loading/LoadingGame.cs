using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LoadingGame : MonoBehaviour
{
    [SerializeField]
    private ImageSelector _map;

    [SerializeField]
    private TextSelector _gameMode;

    [SerializeField]
    private List<PlayerInfo> _playerInfo = new List<PlayerInfo>();

    [SerializeField]
    private float _sceneChangeDelay = 2f;

    void Start()
    {
        //_map.SetImage(PlayerDataRepository.Instance.MatchNotify.GetMapNo());
        _gameMode.SetText((int)PlayerDataRepository.Instance.CurrentGameMode);

        int index = 0;
        // TODO temp remove
//        Dictionary<ulong, protocol.player_profile_info> profile = PlayerDataRepository.Instance.MatchNotify.GetProfiles();        
//        foreach (ulong playerNo in profile.Keys)
//        {
//            _playerInfo[index].SetInfo(playerNo);
//            ++index;
//        }

        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(_sceneChangeDelay);
        
        // TODO temp remove
        string mapName = StringFormat.MapSceneName(0);
        var result = SceneManager.LoadSceneAsync(mapName);        
        while (!result.isDone)
        {
            yield return null;
        }
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mapName));
    }
}
