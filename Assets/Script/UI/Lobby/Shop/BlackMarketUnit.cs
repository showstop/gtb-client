using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlackMarketUnit : MonoBehaviour 
{
    [SerializeField]
    private ImageSelector _rarity;

    [SerializeField]
    private ImageSelector _icon;

    [SerializeField]
    private Text _amount;

    [SerializeField]
    private GameObject _soldOut;

    [SerializeField]
    private Button _buy;

    [SerializeField]
    private Color _soldOutColor;

    [SerializeField]
    private ImageSelector _moneyType;

    [SerializeField]
    private Text _price;

    internal void SetInfo()
    {
        bool soldOut = Random.Range(0, 2) == 0 ? false : true;
        _soldOut.SetActive(!soldOut);
        _buy.interactable = !_soldOut;
        if (soldOut)
        {
            _price.color = _soldOutColor;
        }
    }
}