using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Top : GUIComponent
{
    [SerializeField]
    private Text _gameTicket;

    [SerializeField]
    private Text _cash;

    [SerializeField]
    private Text _gameMoney;

    [SerializeField]
    private GameObject _ticketChargeRemainTimeGO;

    [SerializeField]
    private Text _ticketChargeRemainTime;

    void Awake()
    {
        UpdateAssetInfo();
        InvokeRepeating("UpdateTicketTime", 0f, 1.0f);
    }

    public override void OnHandleEvent(GameEventType gameEventType, params object[] args)
    {
        switch (gameEventType)
        {
            case GameEventType.PlayerIntegratedInfoAnsOK:
            case GameEventType.ChangeGameMode:
                UpdateAssetInfo();
                    
                break;
        }
    }

    private void UpdateAssetInfo()
    {
        if (Constants.GameMode.QUICK == PlayerDataRepository.Instance.CurrentGameMode)
        {   
            _gameTicket.text = "-";
            _ticketChargeRemainTimeGO.SetActive(false);
        }
        else
        {
            _gameTicket.text = StringFormat.CurrentWithMax(PlayerDataRepository.Instance.MyAssetInfo.GrandprixTicket, 5);
            if (5 > PlayerDataRepository.Instance.MyAssetInfo.GrandprixTicket)
            {
                _ticketChargeRemainTime.text = StringFormat.MinuteSecond(PlayerDataRepository.Instance.MyAssetInfo.GrandprixTimestamp);
                _ticketChargeRemainTimeGO.SetActive(true);
            }
        }

        _cash.text = StringFormat.NumberWithComma(PlayerDataRepository.Instance.MyAssetInfo.Diamond);
        _gameMoney.text = StringFormat.NumberWithComma(PlayerDataRepository.Instance.MyAssetInfo.Gold);
    }

    private void UpdateTicketTime()
    {
        if (Constants.GameMode.GRANDPRIX == PlayerDataRepository.Instance.CurrentGameMode &&
            5 > PlayerDataRepository.Instance.MyAssetInfo.GrandprixTicket)
        {
            DateTime dt = new DateTime(PlayerDataRepository.Instance.MyAssetInfo.GrandprixTimestamp);
            var remainSec = DateTime.Now.ToBinary() - PlayerDataRepository.Instance.MyAssetInfo.GrandprixTimestamp;
            _ticketChargeRemainTime.text = StringFormat.MinuteSecond(remainSec);
        }
    }
}