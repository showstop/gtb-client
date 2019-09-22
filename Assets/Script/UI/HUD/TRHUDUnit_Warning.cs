using UnityEngine;
using System.Collections;

public class TRHUDUnit_Warning : MonoBehaviour 
{
    [SerializeField]
    private GameObject _danger;

    [SerializeField]
    private GameObject _safe;

	[SerializeField]
	private UISprite		m_Warning;

	[SerializeField]
	private UISprite		m_Empty;

	private int 			m_refObjectCount = 0;

    internal void UpdateWarning(bool aDanger)
    {
        _danger.SetActive(aDanger);
        _safe.SetActive(!aDanger);
    }

    internal void UpdateWarning(int aUpdateCount)
    {
        int oldCount = m_refObjectCount;
        m_refObjectCount += aUpdateCount;
        if (0 > m_refObjectCount)
        {
            m_refObjectCount = 0;
        }

        if (0 < oldCount && 0 == m_refObjectCount)
        {
            if (null != m_Warning)
            {
                m_Warning.gameObject.SetActive(false);
            }

            if (null != m_Empty)
            {
                m_Empty.gameObject.SetActive(true);
            }
        }
        else if (0 == oldCount && 0 < m_refObjectCount)
        {
            if (null != m_Warning)
            {
                m_Warning.gameObject.SetActive(true);
            }

            if (null != m_Empty)
            {
                m_Empty.gameObject.SetActive(false);
            }
        }
    }
}