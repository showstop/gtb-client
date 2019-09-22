using UnityEngine;
using System.Collections;

public enum CollideLoc
{
    Left,
    Right,
    Front,
    Back,
    Max,
}

public class TRCarEffectParticle : MonoBehaviour 
{
    [SerializeField]
    private GameObject[]    m_carCollideEffect              = new GameObject[(int)CollideLoc.Max];

    [SerializeField]
    private GameObject      m_landingEffect;

    [SerializeField]
    private GameObject      m_boostSpeedUpEffect;

    [SerializeField]
    private GameObject      m_boostSpeedDownEffect;

    [SerializeField]
    private GameObject      m_burnEffect;

    [SerializeField]
    private float[]         m_burnEffectMaxEmissionRate;

    [SerializeField]
    private GameObject      m_zeroHPExplosionEffect;

    [SerializeField]
    private GameObject      m_itemRangeEffect;

    [SerializeField]
    private GameObject      m_getItemEffect;

    private GameObject      m_HPEffect                      = null;
    private GameObject      m_rangeEffect                   = null;
    private Quaternion      m_rangeEffectRotation;
    private Material        m_rangeEffectMaterial           = null;

    [SerializeField]
    private Color[]         m_rangeEffectColor;

    internal void ShowItemRangeEffectParticle(CarController aCar, float aRange, bool aShow, int aColorIndex)
    {
        if (null == m_rangeEffect)
        {
            m_rangeEffect = Instantiate(m_itemRangeEffect) as GameObject;

            Projector proj = m_rangeEffect.GetComponent<Projector>();
            m_rangeEffectMaterial = proj.material;

            m_rangeEffectRotation = m_rangeEffect.transform.rotation;
            m_rangeEffect.transform.position = Vector3.zero;
            m_rangeEffect.transform.parent = aCar.CarTransform;
        }

        if (aShow)
        {
            if (0 <= aColorIndex && aColorIndex < m_rangeEffectColor.Length)
            {
                m_rangeEffectMaterial.color = m_rangeEffectColor[aColorIndex];
            }
   
            m_rangeEffect.transform.localPosition = new Vector3(0f, aRange * 3f, 0f);
            m_rangeEffect.transform.localRotation = m_rangeEffectRotation;
        }
        
        m_rangeEffect.SetActive(aShow);
    }

    internal void PlayPlayerCarCollideEffectParticle(CarController aCar, CollideLoc aLoc)
    {
        PlayEffectParticle(m_carCollideEffect[(int)aLoc], aCar);
    }

    internal void PlayLandingEffectParticle(CarController aCar)
    {
        PlayEffectParticle(m_landingEffect, aCar);
    }

    internal void PlayBurnEffectParticle(CarController aCar, float aRatio)
    {
        //if (!aCar.CarRenderInit)
        //{
        //    return;
        //}

        if (null == m_HPEffect)
        {
            m_HPEffect = PlayEffectParticle(m_burnEffect, aCar);
        }

        float rate = 1f - aRatio;
        ParticleSystem[] spawned = m_HPEffect.GetComponentsInChildren<ParticleSystem>();
        for (int index = 0; index < spawned.Length; ++index)
        {
            spawned[index].emissionRate = m_burnEffectMaxEmissionRate[index] * rate;
        }
    }

    internal void PlayZeroHPExplosionEffectParticle(CarController aCar)
    {
        PlayEffectParticle(m_zeroHPExplosionEffect, aCar);
    }

    internal void PlayGetItemEffectParticle(CarController aCar)
    {
        PlayEffectParticle(m_getItemEffect, aCar);
    }

    internal GameObject PlayEffectParticle(GameObject aGO, CarController aCar)
    {
        if (null == aGO)
        {
            return null;
        }

        //YPLog.Log("TRCarEffectParticle::PlayEffectParticle, aGO = " + aGO);

        GameObject go = Instantiate(aGO, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.parent = aCar.CarTransform;

        TRCarEffectParticleLocalTransform localTransform = go.GetComponent<TRCarEffectParticleLocalTransform>();
        if (null != localTransform)
        {
            go.transform.localPosition = localTransform.m_localPosition;
            go.transform.localRotation = Quaternion.Euler(localTransform.m_localRotation);
        }

        return go;
    }
}
