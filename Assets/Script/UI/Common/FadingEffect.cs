using UnityEngine;
using System.Collections;

public class FadingEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject _blackBG;

    private void OnEnable()
    {
        StartCoroutine(EffectFade());
    }

    private void OnDisable()
    {

    }

    IEnumerator EffectFade()
    {
        yield return new WaitForSeconds(1.25f);
        _blackBG.SetActive(false);
        
        yield return new WaitForSeconds(1.25f);
        this.gameObject.SetActive(false);
    }

}
