using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;

public class GrandPrixRankingUnit : MonoBehaviour
{
    [SerializeField]
    private Text _rank;

    [SerializeField]
    private string _rankKeyword;

    [SerializeField]
    private ImageSelector _leagueMark;

    [SerializeField]
    private ImageSelector _country;

    [SerializeField]
    private Text _nickName;

    [SerializeField]
    private Text _level;

    [SerializeField]
    private Text _gpPoint;

//    internal void UpdateInfo(protocol.grandprix_rank_type aType, ulong aRank)
//    {
//        protocol.grandprix_rank_unit info = PlayerDataRepository.Instance.GetGrandPrixRankInfo(aType, aRank);
//        if (null == info)
//        {
//            return;
//        }
//
//        // TO DO : league mark
//        _rank.text = string.Format("{0}{1}", info.GetRank(), LanguageManager.Instance.GetTextValue(_rankKeyword));
//        //_leagueMark.SetImage(info.);
//        _country.SetImage(info.GetNationCode());
//        _nickName.text = info.GetNick();
//        _level.text = StringFormat.LevelString(info.GetLevel());
//        _gpPoint.text = StringFormat.GPPointString(info.GetGpPoint());
//    }
}