using UnityEngine;
using System.Collections;

public class TRPanel : MonoBehaviour {

	[SerializeField]
	protected GameObject m_mainPanel;

	public bool m_openAnimation = false;
	public float m_openAnimationTime = 0.3f;
	public bool m_closeAnimation = false;
	public float m_closeAnimationTime = 0.3f;

	public bool m_closeBottom = false;
	public bool m_closeMain = false;

	/** Panel Open */
    public virtual void OpenPanel()
    {
        if (IsPanelOpen())
        {
            return;
        }

		if ( true == m_openAnimation )
		{
			OpenPanelAnimation ();
		}else
		{
			if ( null != m_mainPanel)
			{
				m_mainPanel.transform.localScale = Vector3.one;
			}
        	gameObject.SetActive( true );
		}

	}

	protected void OpenPanelAnimation()
	{
		gameObject.SetActive( true );

		OpenTween ();
	}

    /** Panel Close */
    public virtual void ClosePanel()
    {
        if (!IsPanelOpen())
        {
            return;
        }

		if ( true == m_closeAnimation )
		{
			StartCoroutine( ClosePanelAnimation () );
		}else
		{
            // need to?   
            // ResetPanel();

			gameObject.SetActive( false );
		}

    }

	private IEnumerator ClosePanelAnimation()
	{
		CloseTween();
        yield return new WaitForSeconds(m_closeAnimationTime - 0.05f);

        ResetPanel();
		yield return new WaitForSeconds(0.05f);

		gameObject.SetActive(false);
	}

    protected virtual void ResetPanel() { }

	public void OpenTween()
	{
		if ( null != m_mainPanel )
		{
			m_mainPanel.transform.localScale = Vector3.zero;
			TweenScale tc = TweenScale.Begin( m_mainPanel, m_openAnimationTime, Vector3.one );
			if ( null != tc )
			{
				tc.method = UITweener.Method.EaseInOut;
				EventDelegate.Add ( tc.onFinished, OnOpenTweenFinished, true );
			}
		}
	}

	public void CloseTween()
	{
		if ( null != m_mainPanel )
		{
			m_mainPanel.transform.localScale = Vector3.one;
			TweenScale tc = TweenScale.Begin( m_mainPanel, m_closeAnimationTime, Vector3.zero );
			if ( null != tc )
			{
				tc.method = UITweener.Method.EaseInOut;
				EventDelegate.Add ( tc.onFinished, OnCloseTweenFinished, true );
			}
		}
	}

	protected virtual void OnOpenTweenFinished()
	{
		/* 비어있음 */
	}

	protected virtual void OnCloseTweenFinished()
	{
		/* 비어있음 */
	}

    internal bool IsPanelOpen()
    {
        return gameObject.activeInHierarchy;
    }
}
