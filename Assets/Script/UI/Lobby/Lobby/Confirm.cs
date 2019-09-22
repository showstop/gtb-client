using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Network;
using SmartLocalization;

public enum ProductType
{
    Gold_1 = 100,
    Gold_2,
    Gold_3,
    Gold_4,
    Gold_5,
    Gold_6,

    QuickMatchTicket_1 = 200,
    QuickMatchTicket_2,
    QuickMatchTicket_3,

    GrandPrixTicket = 210,

    SpecialAbilitySlot_2 = 300,
    SpecialAbilitySlot_3,

    Cash_1 = 400,
    Cash_2,
    Cash_3,
    Cash_4,
    Cash_5,
    Cash_6,

    SpecialAbility_ResearchGunPowder = 31000001,
    SpecialAbility_ResearchBumper,
    SpecialAbility_AutoCharge,
    SpecialAbility_MultiTools = 31000011,
    SpecialAbility_PowerSteering,
    SpecialAbility_Stacker,
    SpecialAbility_RearSensor = 31000021,
    SpecialAbility_LuckyDice,
}

[System.Serializable]
public class ProductInfo
{
    public ProductType _type;
    public string _keyword;
    public int _moneyType;
    public int _price;
}

public class Confirm : PopupComponent 
{
    [SerializeField]
    private ImageSelector _icon;

    [SerializeField]
    private Text _product;

    [SerializeField]
    private ImageSelector _money;

    [SerializeField]
    private Text _price;

    [SerializeField]
    private List<ProductInfo> _productInfo = new List<ProductInfo>();

    private ProductType _type;

    internal void SetInfo(int aProductID)
    {
        _type = (ProductType)aProductID;
        ProductInfo info = _productInfo.Find(delegate(ProductInfo pi) { return pi._type == _type; });        
        _icon.SetImage(aProductID);
        _product.text = LanguageManager.Instance.GetTextValue(info._keyword);
        _money.SetImage(info._moneyType);
        _price.text = StringFormat.NumberWithComma(info._price);
    }

    public void ConfirmBuy()
    {
        switch (_type)
        {
            case ProductType.Gold_1:
                break;
            case ProductType.Gold_2:
                break;
            case ProductType.Gold_3:
                break;
            case ProductType.Gold_4:
                break;
            case ProductType.Gold_5:
                break;
            case ProductType.Gold_6:
                break;

            case ProductType.QuickMatchTicket_1:
                break;
            case ProductType.QuickMatchTicket_2:
                break;
            case ProductType.QuickMatchTicket_3:
                break;
            case ProductType.GrandPrixTicket:
                break;

            case ProductType.SpecialAbilitySlot_2:
                LobbyRequest.Instance.AbilitySlotOpenReq(2);
                break;
            case ProductType.SpecialAbilitySlot_3:
                LobbyRequest.Instance.AbilitySlotOpenReq(3);
                break;
                
            case ProductType.Cash_1:
                break;
            case ProductType.Cash_2:
                break;
            case ProductType.Cash_3:
                break;
            case ProductType.Cash_4:
                break;
            case ProductType.Cash_5:
                break;
            case ProductType.Cash_6:
                break;

            case ProductType.SpecialAbility_ResearchGunPowder:
            case ProductType.SpecialAbility_ResearchBumper:
            case ProductType.SpecialAbility_AutoCharge:
            case ProductType.SpecialAbility_MultiTools:
            case ProductType.SpecialAbility_PowerSteering:
            case ProductType.SpecialAbility_Stacker:
            case ProductType.SpecialAbility_RearSensor:
            case ProductType.SpecialAbility_LuckyDice:
                LobbyRequest.Instance.AbilityAcquireReq((int)_type);
                break;
        }
    }
}