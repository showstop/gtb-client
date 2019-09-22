using UnityEngine;
using System.Collections;

public class CarSkill : MonoBehaviour 
{
    [SerializeField]
    private ImageSelector _bg;

    [SerializeField]
    private ImageSelector _icon;

    private int _level;
    private int _id;

    internal void SetImage(int aCarLevel, int aSkillID)
    {
        _level = aCarLevel;
        _id = aSkillID;

        _bg.SetImage(_level);
        _icon.SetImage(_id);
    }

    public void ShowDescription()
    {

    }
}