using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SmartLocalization;

public class BlackMarket : PopupComponent
{
    [SerializeField]
    private Text _timeRemain;

    [SerializeField]
    private string _timeRemainKeyword;

    [SerializeField]
    private List<BlackMarketUnit> _products = new List<BlackMarketUnit>();

    internal void UpdateProducts()
    {
        long time = 1000;
        _timeRemain.text = string.Format(LanguageManager.Instance.GetTextValue(_timeRemainKeyword), StringFormat.HourMinuteSecond(time));

        for (int index = 0; index < _products.Count; ++index)
        {
            _products[index].SetInfo();
        }
    }

    public void Refresh()
    {

    }

    public override void OnHandleEvent(GameEventType gameEventType, params System.Object[] args)
    {
    }
}