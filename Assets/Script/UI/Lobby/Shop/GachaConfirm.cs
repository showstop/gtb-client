using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum GachaType
{
    NormalMaterialOnce,
    NomralMaterialTenTimes,
    RareMaterialOnce,
    RareMaterialTenTimes,
    CarPieceOnce,
    CarPieceTenTimes,
}

[System.Serializable]
public class GachaInfo
{
    public GachaType _type;
    public int _moneyType;
    public int _price;
}

public class GachaConfirm : PopupComponent
{
    [SerializeField]
    private ImageSelector _moneyType;

    [SerializeField]
    private Text _price;

    [SerializeField]
    private List<GachaInfo> _gachaInfo = new List<GachaInfo>();

    private GachaType _type;

    internal void SetInfo(GachaType aType)
    {
        _type = aType;

        GachaInfo info = _gachaInfo.Find(delegate(GachaInfo gi) { return gi._type == _type; });        
        _moneyType.SetImage(info._moneyType);
        _price.text = StringFormat.NumberWithComma(info._price);
    }

    public void ConfirmGacha()
    {
        switch (_type)
        {
            case GachaType.NormalMaterialOnce:
                break;
            case GachaType.NomralMaterialTenTimes:
                break;
            case GachaType.RareMaterialOnce:
                break;
            case GachaType.RareMaterialTenTimes:
                break;
            case GachaType.CarPieceOnce:
                break;
            case GachaType.CarPieceTenTimes:
                break;
        }
    }
}