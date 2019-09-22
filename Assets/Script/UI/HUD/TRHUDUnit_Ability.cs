using UnityEngine;
using System.Collections;

public class TRHUDUnit_Ability : MonoBehaviour {

	[SerializeField]
	private GameObject 		m_active;

    [SerializeField]
    private GameObject      m_passive;

    [SerializeField]
    private GameObject      m_activated;

    [SerializeField]
    private UISprite        m_activatedAnimationSprite1;

    [SerializeField]
    private UISprite        m_activatedAnimationSprite2;

    [SerializeField]
    private UISprite        m_activatedBGSprite;

    [SerializeField]
    private Animation       m_activatedAnimation1;

    [SerializeField]
    private Animation       m_activatedAnimation2;

	[SerializeField]
	private UISprite 		m_progressBar;

	[SerializeField]
	private UITexture		m_AbilityImage = null;    

	private float 			m_durationTime = 1.0f;
	private float			m_currentValue = 0.0f;
	private float			m_Value = 0.0f;
	public float Value
	{
		get
        { 
            return m_Value; 
        }
		set
        {
			m_Value = value;
            if (1f == m_Value)
            {
                Activated();
            }

			StopCoroutine( "TweenValue" );
			StartCoroutine( "TweenValue" );
		}
	}

    public int              AbilityID           { get; set; }    

	/** 어빌리티 등록 */
	public void Ability( int abilityID )
	{
        YPLog.Trace();
        YPLog.Log("before = " + AbilityID + ", current = " + abilityID);

        AbilityID = abilityID;

		Constants.ABILITY_CATEGORY categoty = TRStatic.GetAbilityCatagory(abilityID);
		if ( Constants.ABILITY_CATEGORY.PASSIVE == categoty )
		{
			InsertPassive( abilityID );
		}else if ( Constants.ABILITY_CATEGORY.ACTIVE == categoty )
		{
			InsertActive( abilityID );
		}else
		{
			YPLog.Log ( "분류 되어 있지 않는 특성입니다.");
		}
	}

	/*! 액티브 등록 */
	private void InsertActive( int abilityID )
	{
		if ( null != m_passive )
			m_passive.SetActive( false );

		if ( null != m_active )
		{
			m_active.SetActive( true );
		}

		if ( null != m_AbilityImage )
		{
            Texture texture = TRResourceManager.GetAbilityImage(abilityID);
			if ( null != texture )
			{
				m_AbilityImage.mainTexture = texture;
			}
		}
	}

	/*! 패시브 등록 */
	private void InsertPassive( int abilityID )
	{
		if ( null != m_active )
			m_active.SetActive( false );
		
		if ( null != m_passive )
		{
			m_passive.SetActive( true );
		}

		if ( null != m_AbilityImage )
		{
            Texture texture = TRResourceManager.GetAbilityImage(abilityID);
			if ( null != texture )
			{
				m_AbilityImage.mainTexture = texture;
			}
		}
	}

	public IEnumerator TweenValue()
	{
		float t = 0.0f;
		float value = 0.0f;
		float fromValue = m_currentValue;

		while(true)
		{
			t += Time.deltaTime / m_durationTime;
			value = Mathf.Lerp ( fromValue, m_Value, t);

			m_currentValue = value;
			m_progressBar.fillAmount = (1.0f - value);

			if ( value == m_Value)
				break;

			yield return null;
		}

		yield return null;
	}

    internal void Activated()
    {
        m_activated.SetActive(true);
        if (null != m_activatedAnimation1)
        {
            m_activatedAnimation1.Play();
        }
        
        StartCoroutine(ActivateFirstSprite());
        StartCoroutine(ActivateSecondSprite());
    }

    private IEnumerator ActivateFirstSprite()
    {
        TweenAlpha.Begin(m_activatedAnimationSprite1.gameObject, 0.01f, 1f);
        TweenAlpha.Begin(m_activatedBGSprite.gameObject, 0.01f, 1f);

        yield return new WaitForSeconds(0.01f);

        TweenAlpha.Begin(m_activatedAnimationSprite1.gameObject, 0.49f, 0f);
        TweenAlpha.Begin(m_activatedBGSprite.gameObject, 0.75f, 0f);
    }

    private IEnumerator ActivateSecondSprite()
    {
        yield return new WaitForSeconds(0.25f);

        m_activatedAnimationSprite2.gameObject.SetActive(true);
        TweenAlpha.Begin(m_activatedAnimationSprite2.gameObject, 0.01f, 1f);
        if (null != m_activatedAnimation2)
        {
            m_activatedAnimation2.Play();
        }

        yield return new WaitForSeconds(0.01f);

        TweenAlpha.Begin(m_activatedAnimationSprite2.gameObject, 0.49f, 0f);

        while (null != m_activatedAnimation2 && m_activatedAnimation2.isPlaying)
        {
            yield return null;
        }

        m_activatedAnimationSprite2.gameObject.SetActive(false);
        m_activated.SetActive(false);
    }
}