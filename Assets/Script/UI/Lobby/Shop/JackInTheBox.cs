using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SmartLocalization;

public class JackInTheBox : PopupComponent 
{
    [SerializeField]
    private Button _normalBoxOpenButton;

    [SerializeField]
    private Text _normalBoxOpenText;

    [SerializeField]
    private Button _rareBoxOpenButton;

    [SerializeField]
    private Text _rareBoxOpenText;

    [SerializeField]
    private string _boxOpenKeyword;

    private const int NORMAL_OPEN_MAX = 5;
    private const int RARE_OPEN_MAX = 1;

    internal void SetInfo()
    {
        // TO DO 
        //bool canOpenNormalBox = true;
        //_normalBoxOpenButton.interactable = canOpenNormalBox;
        //if (_normalBoxOpenButton.interactable)
        //{
        //    _normalBoxOpenText.text = string.Format("{0} {1}", LanguageManager.Instance.GetTextValue(_boxOpenKeyword), StringFormat.CurrentWithMax(3, 5));
        //}
        //else
        //{
        //    _normalBoxOpenText.text = StringFormat.HourMinuteSecond();
        //}

        //bool canOpenRareBox = true;
        //_rareBoxOpenButton.interactable = canOpenRareBox;
        //if (_rareBoxOpenButton.interactable)
        //{
        //    _rareBoxOpenText.text = string.Format("{0} {1}", LanguageManager.Instance.GetTextValue(_boxOpenKeyword), StringFormat.CurrentWithMax(3, 5));
        //}
        //else
        //{
        //    _rareBoxOpenText.text = StringFormat.HourMinuteSecond();
        //}
    }

    public void OpenNormalBox()
    {

    }

    public void OpenRareBox()
    {

    }
}