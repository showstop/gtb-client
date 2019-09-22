using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GrandPrixSeasonRewardList : PopupComponent
{
    [SerializeField]
    private List<Toggle> _leagueButton = new List<Toggle>();

    [SerializeField]
    private List<GameObject> _leagueReward = new List<GameObject>();

    void OnEnable()
    {
        UpdateInfo();
    }

    internal void UpdateInfo()
    {   
        int leagueIndex = Random.Range(0, 3);
        YPLog.Log("index = " + leagueIndex);
        _leagueButton[leagueIndex].isOn = true;
        ChangeLeague(leagueIndex);
    }

    private void HideAllLeagueReward()
    {
        for (int index = 0; index < _leagueReward.Count; ++index)
        {
            _leagueReward[index].SetActive(false);
        }
    }

    public void ChangeLeague(int aLeagueIndex)
    {
        HideAllLeagueReward();
        _leagueReward[aLeagueIndex].SetActive(true);
    }
}