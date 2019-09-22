using UnityEngine;
using System.Collections;

public class HUDChangeCar : MonoBehaviour 
{
    private int _carID;

    [SerializeField]
    private UITexture _carImage;

    [SerializeField]
    private UISprite _cooltime;

    [SerializeField]
    private Collider _collider;

    

    internal IEnumerator Cooltime(float aCooltime)
    {
        _collider.enabled = false;

        float current = aCooltime;
        _cooltime.fillAmount = 1f;        
        while (_cooltime.fillAmount > 0f)
        {
            current -= Time.deltaTime;
            _cooltime.fillAmount = current / aCooltime;            
            if (_cooltime.fillAmount < 0f)
            {
                _cooltime.fillAmount = 0f;
            }

            yield return null;
        }

        _collider.enabled = true;
    }
}
