using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpriteAnimation : MonoBehaviour
{
    public List<Sprite> _animtionSprite = new List<Sprite>();
    public Image _focusImage;

    public float _animationSpeed;
    
    void OnEnable()
    {
        StartCoroutine(AnimationSprite(true));
    }

    private IEnumerator AnimationSprite(bool aShow)
    {
        while(aShow)
        {
            for (int i = 0; i < _animtionSprite.Count; i++)
            {
                _focusImage.sprite = _animtionSprite[i];
                yield return new WaitForSeconds(_animationSpeed);
            }
        }
    }
}
