using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SmartLocalization;

public class GrandPrix : GUIComponent 
{
    [SerializeField]
    private ImageSelector _leagueMark;

    [SerializeField]
    private Text _rank;

    [SerializeField]
    private Text _point;

    [SerializeField]
    private Text _repeatCount;

    [SerializeField]
    private Button _repeatReward;

    [SerializeField]
    private Text _seasonRemainTime;

    [SerializeField]
    private Button _seasonReward;

    [SerializeField]
    private List<Toggle> _rankToggle = new List<Toggle>();

    [SerializeField]
    private List<GameObject> _rankList = new List<GameObject>();

    private List<GrandPrixRankingUnit> _leagueRanking = new List<GrandPrixRankingUnit>();
    private List<GrandPrixRankingUnit> _totalRanking = new List<GrandPrixRankingUnit>();

    private const int MAX_REPEAT_COUNT = 5;

    void Start()
    {
        for (int index = 0; index < _rankList.Count; ++index)
        {
//            List<GrandPrixRankingUnit> target = null;
//            protocol.grandprix_rank_type type = (protocol.grandprix_rank_type)(index + 1);
//            if (protocol.grandprix_rank_type.LEAGUE == type)
//            {
//                target = _leagueRanking;
//            }
//            else
//            {
//                target = _totalRanking;
//            }
//
//            GrandPrixRankingUnit[] list = _rankList[index].GetComponentsInChildren<GrandPrixRankingUnit>();
//            foreach (GrandPrixRankingUnit unit in list)
//            {
//                target.Add(unit);
//            }
        }

        HideAllRankingList();
    }

    private void HideAllRankingList()
    {
        for (int index = 0; index < _rankList.Count; ++index)
        {
            _rankList[index].SetActive(false);
        }
    }


    internal void UpdateInfo()
    {
        // TO DO : league mark
//        _leagueMark.SetImage(1);
//        _rank.text = string.Format("<size=30><color=#ffff00ff>{0}{1}</color></size>({2}%)", 
//                                    StringFormat.NumberWithComma(PlayerDataRepository.Instance.GrandPrixInfo.GetMyRank()), 
//                                    LanguageManager.Instance.GetTexture("GrandPrix.Placing"), 
//                                    PlayerDataRepository.Instance.GrandPrixInfo.GetMyRankPercentile());
//        _point.text = StringFormat.GPPointString(PlayerDataRepository.Instance.GrandPrixInfo.GetGpPoint());
//
//        int repeatCount = PlayerDataRepository.Instance.GrandPrixInfo.GetRepeatCount();
//        _repeatCount.text = StringFormat.CurrentWithMax(repeatCount, MAX_REPEAT_COUNT);
//        _repeatReward.interactable = (MAX_REPEAT_COUNT == repeatCount);
//
//        // TO DO : season reward
//        _seasonRemainTime.text = StringFormat.HourMinuteSecond(PlayerDataRepository.Instance.GrandPrixInfo.GetRewardDate());
//        //_seasonReward.interactable = ;
//
//        _rankList[0].SetActive(true);
    }

    public void LeagueInfo()
    {
        EventManager.Instance.SendGameEvent(GameEventType.ShowGrandPrixLeagueInfo);
    }

    public void RepeatRewardList()
    {
        EventManager.Instance.SendGameEvent(GameEventType.ShowGrandPrixRepeatRewardList);
    }

    public void SeasonRewardList()
    {
        EventManager.Instance.SendGameEvent(GameEventType.ShowGrandPrixSeasonRewardList);
    }

    public void ChangeRankingType(int aType)
    {
        HideAllRankingList();
        _rankList[aType].SetActive(true);
    }
}