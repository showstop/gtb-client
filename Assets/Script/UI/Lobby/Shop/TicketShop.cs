using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TicketShop : PopupComponent 
{
    [SerializeField]
    private Slider _chargeSlider;

    [SerializeField]
    private Text _chargeCount;

    private const int MAX_CHARGE_COUNT = 10;

    internal void SetInfo()
    {

    }
}