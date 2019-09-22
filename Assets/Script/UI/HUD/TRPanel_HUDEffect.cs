using UnityEngine;
using System.Collections;

public class TRPanel_HUDEffect : TRPanel
{
    [SerializeField]
    private UISprite            m_countDownSprite;

    private const string        COUNT_DOWN_SPRITE_NAME_FORMAT       = "start_{0}";

	public void ShowCountDown(int aCountDown)
    {
        StopCoroutine("HideCountDown");

        string fileName = string.Format(COUNT_DOWN_SPRITE_NAME_FORMAT, aCountDown);
        m_countDownSprite.gameObject.SetActive(true);
        m_countDownSprite.spriteName = fileName;
        m_countDownSprite.gameObject.GetComponent<Animation>().Play("countdown");
        m_countDownSprite.MakePixelPerfect();

        StartCoroutine("HideCountDown");
    }

    IEnumerator HideCountDown()
    {
        yield return new WaitForSeconds(1f);

        m_countDownSprite.gameObject.SetActive(false);
    }
}
