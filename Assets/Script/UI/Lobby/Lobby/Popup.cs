using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Popup : GUIComponent
{
    [SerializeField]
    private Attend _attend;

    [SerializeField]
    private Setting _setting;

    [SerializeField]
    private Quest _quest;

    [SerializeField]
    private Advertise _advertise;

    [SerializeField]
    private EventPopup _event;

    [SerializeField]
    private MailPopup _mail;

    [SerializeField]
    private Language _language;

    [SerializeField]
    private QuickMatchStart _quickMatchStart;

    [SerializeField]
    private ChangeSelectCar _changeSelectCar;

    [SerializeField]
    private PopupComponent _grandPrixLeagueInfo;

    [SerializeField]
    private PopupComponent _grandPrixRepeatRewardInfo;

    [SerializeField]
    private GrandPrixSeasonRewardList _grandPrixSeasonRewardInfo;

    [SerializeField]
    private MedalReward _medalReward;

    [SerializeField]
    private CarUpgrade _carUpgrade;

    [SerializeField]
    private PartsTuning _partsTuning;

    [SerializeField]
    private SpecialAbilityUpgrade _specialAbilityUpgrade;

    [SerializeField]
    private SpecialAbilityBuy _specialAbilityBuy;

    [SerializeField]
    private PackageShop _packageShop;

    [SerializeField]
    private GachaShop _gachaShop;

    [SerializeField]
    private PopupComponent _cacheShop;

    [SerializeField]
    private PopupComponent _goldShop;

    [SerializeField]
    private TicketShop _ticketShop;

    [SerializeField]
    private JackInTheBox _jackInTheBox;

    [SerializeField]
    private BlackMarket _blackMarket;

    [SerializeField]
    private GachaConfirm _gachaConfirm;

    [SerializeField]
    private Confirm _confirm;

    [SerializeField]
    private Reward _reward;

    [SerializeField]
    private WaitMatchMaking _waitMatchMaking;

    [SerializeField]
    private PopupComponent _waitServerResponse;

    public override void OnHandleEvent(GameEventType gameEventType, params System.Object[] args)
    {
        switch (gameEventType)
        {
            case GameEventType.ShowLanguageSelection:
                _language.Open();

                break;
            
            case GameEventType.UnlockSpeicalAbilitySlot:
                OpenConfirm((int)args[0]);

                break;

            case GameEventType.BuySpecialAbility:
                OpenConfirm((int)args[0]);

                break;

            case GameEventType.LevelUpSpecialAbility:
                _specialAbilityUpgrade.SetInfo((int)args[0], (int)args[1]);
                _specialAbilityUpgrade.Open();

                break;

            case GameEventType.ChangeSelectCar:

                break;

            case GameEventType.UpgradeCarPresentationStart:                
                _carUpgrade.SetInfo((PlayerDataRepository.CarInfo)args[0]);
                _carUpgrade.gameObject.SetActive(true);

                break;

            case GameEventType.UpgradeCarPresentationEnd:                
                _carUpgrade.gameObject.SetActive(false);

                break;

            case GameEventType.ShowGrandPrixLeagueInfo:
                _grandPrixLeagueInfo.Open();

                break;

            case GameEventType.ShowGrandPrixRepeatRewardList:
                _grandPrixRepeatRewardInfo.Open();

                break;

            case GameEventType.ShowGrandPrixSeasonRewardList:
                _grandPrixSeasonRewardInfo.Open();

                break;

            case GameEventType.AchievementReceiveRewardAnsOK:
//                protocol.achievement_receive_reward_ans ans = (protocol.achievement_receive_reward_ans)args[0];
//                _reward.SetInfo(ans.GetReward());
//                _reward.Open();

                break;

            case GameEventType.AttendanceReceiveRewardAnsOK:                
//                _reward.SetInfo((protocol.reward_info)args[0]);
//                _reward.Open();

                break;

            case GameEventType.MatchInfoAnsOK:                
                if (Constants.GameMode.QUICK == PlayerDataRepository.Instance.CurrentGameMode)
                {
                    _quickMatchStart.UpdateList();
                    _quickMatchStart.Open();
                }
                else
                {

                }

                break;

            case GameEventType.MatchStartAnsOK:
//                protocol.match_start_ans startAns = (protocol.match_start_ans)args[0];
//                if (protocol.GameMode.QUICK == PlayerDataRepository.Instance.CurrentGameMode)
//                {
//                    _quickMatchStart.Close();
//                }
//                else
//                {
//
//                }

//                _waitMatchMaking.SetTime(startAns.GetEstimatedSec());
//                _waitMatchMaking.Open();

                break;

            case GameEventType.MatchStopAnsOK:                
                _waitMatchMaking.Close();
                break;

            case GameEventType.StartWaitServerResponse:
                _waitServerResponse.Open();
                break;

            case GameEventType.EndWaitServerResponse:
                _waitServerResponse.Close();
                break;

            case GameEventType.MatchStartAnsFailed:
//                protocol.match_start_ans startAnsFail = (protocol.match_start_ans)args[0];
//                YPLog.Log("====== MatchStartAnsFailed!! result[" + startAnsFail.GetResult() + "]");
                break;
        }
    }

    public void OpenAttend()
    {
        _attend.Open();
    }

    public void OpenSetting()
    {
        _setting.Open();
    }

    public void OpenQuest()
    {
        _quest.Open();
    }

    public void OpenAdvertise()
    {
        _advertise.Open();
    }

    public void OpenEvent()
    {
        _event.Open();
    }

    public void OpenMail()
    {
        _mail.Open();
    }

    public void OpenPartsTuning(int aPartsID)
    {
        _partsTuning.SetInfo((Constants.PARTS_CATEGORY)aPartsID);
        _partsTuning.Open();
    }

    public void OpenSpecialAbilityBuy(int aID)
    {
        _specialAbilityBuy.SetInfo(aID);
        _specialAbilityBuy.Open();
    }

    public void OpenMedalReward()
    {
        //_medalReward.SetInfo();
        //_medalReward.Open();
    }

    public void OpenPackageShop()
    {        
        _packageShop.Open();
    }

    public void OpenGachaShop()
    {
        _gachaShop.Open();
    }

    public void OpenCacheShop()
    {
        _cacheShop.Open();
    }

    public void OpenGoldShop()
    {
        _goldShop.Open();
    }

    public void OpenTicketShop()
    {
        //_ticketShop.SetInfo();
        _ticketShop.Open();
    }

    public void OpenJackInTheBox()
    {
        //_jackInTheBox.SetInfo();
        _jackInTheBox.Open();
    }

    public void OpenBlackMarket()
    {
        _blackMarket.UpdateProducts();
        _blackMarket.Open();
    }

    public void OpenConfirm(int aProductID)
    {
        _confirm.SetInfo(aProductID);
        _confirm.Open();
    }

    public void OpenGachaConfirm(int aType)
    {
        _gachaConfirm.SetInfo((GachaType)aType);
        _gachaConfirm.Open();        
    }

}