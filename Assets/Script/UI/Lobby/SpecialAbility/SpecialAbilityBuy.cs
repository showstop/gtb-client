using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpecialAbilityBuy : PopupComponent 
{
    [SerializeField]
    private ImageSelector _icon;

    [SerializeField]
    private TextSelector _name;

    [SerializeField]
    private ImageSelector _money;

    [SerializeField]
    private Text _price;

    private int _id;

	internal void SetInfo(int aID)
    {
        // by anemos
        // TO DO

        _id = aID;

        _icon.SetImage(aID);
        _name.SetText(aID);
        //_by.SetImage();
        //_price.text = StringFormat.NumberWithComma(price);
    }

    public void BuySpecialAbility()
    {
        // by anemos
        // TO DO
    }
}