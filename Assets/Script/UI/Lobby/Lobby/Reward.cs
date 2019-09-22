using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Reward : PopupComponent
{
    [SerializeField]
    private ImageSelector _icon;

    [SerializeField]
    private Text _amount;

    [SerializeField]
    private TextSelector _name;

//    internal void SetInfo(protocol.reward_info aInfo)
//    {
//        YPLog.Log("id = " + aInfo.GetSpecificId() + ", amount = " + aInfo.GetQuantity());
//        // TO DO :
//        //_icon.SetImage(aInfo.GetSpecificId());
//        //_name.SetText(aInfo.GetSpecificId());
//        _amount.text = string.Format("x {0}", aInfo.GetQuantity());
//    }
}
