using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SmartLocalization.Editor;

[System.Serializable]
public class TextInfo
{
    public int _id;
    public string _key;
}

[RequireComponent(typeof(LocalizedText))]
public class TextSelector : MonoBehaviour 
{
    [SerializeField]
    private bool _randomSelect;

    [SerializeField]
    private List<TextInfo> _keyList = new List<TextInfo>();

    [SerializeField]
    private LocalizedText _localized;    

    private void Start()
    {
        if (_randomSelect)
        {
            int random = Random.Range(0, _keyList.Count);
            _localized.localizedKey = _keyList[random]._key;            
        }
    }

    internal void SetText(int aID)
    {
        TextInfo target = _keyList.Find(delegate(TextInfo ti) { return ti._id == aID; });
        _localized.ChangeLocalizedKey(target._key);   
    }

    internal void SetText(string aText)
    {
        _localized.ChangeText(aText);
    }

    internal string GetKey(int aID)
    {
        TextInfo target = _keyList.Find(delegate(TextInfo ti) { return ti._id == aID; });
        return target._key;
    }
}