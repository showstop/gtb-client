using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class QuickMatchResult : MonoBehaviour
{
    [SerializeField]
    private List<MatchRank> _rank = new List<MatchRank>();

    [SerializeField]
    private Text _exp;

    [SerializeField]
    private Text _gameMoney;

    [SerializeField]
    private ImageSelector _secondReward;

    [SerializeField]
    private List<ImageSelector> _rewards = new List<ImageSelector>();

    [SerializeField]
    private List<GameObject> _rewardGo = new List<GameObject>();

    [SerializeField]
    private GameObject _lobbyText;

    private int _openCardCount = 0;
    private int _currentOpen = 0;

    internal void SetGameResult(CarController aMe, int aFirstReward, int aSecondReward)
    {   
        GameManager gm = GameObject.FindWithTag(Constants.GAME_MANAGER_TAG_NAME).GetComponent<GameManager>();
        if (null == gm)
        {
            return;
        }

        CarController[] players = gm.GetSortPlayers();
        for (int index = 0; index < players.Length; ++index)
        {   
            _rank[index].SetInfo(players[index]._playerNo, players[index]._finalRank, players[index]._lapTime, players[index].isLocalPlayer);
        }

        _exp.text = StringFormat.NumberWithComma(aMe._exp);
        _gameMoney.text = StringFormat.NumberWithComma(aMe._gameMoney);

        _rewards[0].SetImage(aFirstReward);
        if (0 != aSecondReward)
        {
            _rewards[1].SetImage(aSecondReward);
        }

        switch (aMe._finalRank)
        {
            case 1:
            case 2:
            case 3:
                _openCardCount = 2;
                _secondReward.SetImage(aMe._finalRank);
                _secondReward.gameObject.SetActive(true);
                break;

            case 4:
                _openCardCount = 1;
                _secondReward.gameObject.SetActive(false);
                break;
        }
    }

    public void OpenCard(int aIndex)
    {        
        _rewardGo[aIndex].SetActive(true);

        ++_currentOpen;        
        if (_openCardCount == _currentOpen)
        {
            StartCoroutine(BackToLobby());
        }
    }

    private IEnumerator BackToLobby()
    {
        _lobbyText.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("loading_lobby");
    }
}