using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShineImage : MonoBehaviour
{
    [SerializeField]
    private Material _shineMaterial;

    [SerializeField]
    private float _duration;

    void OnEnable()
    {
        // change material     
        Renderer render = gameObject.GetComponent<Renderer>();
        if (null != render)
        {
            render.material = _shineMaterial;
        }
        else
        {
            Image img = gameObject.GetComponent<Image>();
            if (null != img)
            {
                img.material = _shineMaterial;
            }
            else
            {
                YPLog.LogWarning("cannot get the render or image component!");
            }
        }

        StartCoroutine(Shine(_shineMaterial, _duration));
    }

    private IEnumerator Shine(Material aMat, float aDuration)
    {
        if (null != aMat)
        {
            float location = 0f;
            float interval = 0.04f;
            float offsetVal = interval / aDuration;
            while (true)
            {
                yield return new WaitForSeconds(interval);
                aMat.SetFloat("_ShineLocation", location);
                location += offsetVal;
                if (location > 1f)
                {
                    location = 0f;
                }
            }
        }
        else
        {
            YPLog.LogWarning("there is no material parameter!");
        }
    }
}