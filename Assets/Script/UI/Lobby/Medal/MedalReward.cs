using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MedalReward : PopupComponent 
{
    [SerializeField]
    private ImageSelector _icon;

    [SerializeField]
    private Text _count;

    [SerializeField]
    private TextSelector _name;
    
    internal void SetInfo()
    {

    }
}