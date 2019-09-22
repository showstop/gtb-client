using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SpriteInfo
{
    public int _id;
    public Sprite _sprite;
}

[RequireComponent(typeof(Image))]
public class ImageSelector : MonoBehaviour 
{
    [SerializeField]
    private List<SpriteInfo> _sprites = new List<SpriteInfo>();

    [SerializeField]
    private Image _image;

    internal void SetImage(int aID)
    {
        SpriteInfo target = _sprites.Find(delegate (SpriteInfo si) { return si._id == aID; });
        if (target != null)
        {
            _image.sprite = target._sprite;
        }
    }
        
}