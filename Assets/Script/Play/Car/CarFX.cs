using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CarExplosionFX
{
    public int          _carID;
    public GameObject   _explosionFX;
}

public class CarFX : MonoBehaviour 
{
    [SerializeField]
    private CarController _owner;

    [SerializeField]
    private GameObject[] _collideFX = new GameObject[(int)CollideLoc.Max];

    [SerializeField]
    private GameObject _landingFX;

    [SerializeField]
    private GameObject _boostSpeedUpFX;

    [SerializeField]
    private GameObject _boostSpeedDownFX;

    [SerializeField]
    private GameObject _burnFX;

    [SerializeField]
    private float[] _burnFXMaxEmissionRate;

    [SerializeField]
    private GameObject _explosionFX;

    [SerializeField]
    private List<CarExplosionFX> _carExplosionFX = new List<CarExplosionFX>();
    private Dictionary<int, GameObject> _carExplosionFXDic = new Dictionary<int, GameObject>();

    [SerializeField]
    private GameObject _itemRangeFX;

    [SerializeField]
    private GameObject _getItemFX;    

    private GameObject _hpFX = null;
    private GameObject _rangeFX = null;
    private Quaternion _rangeFXRotation;
    private Material _rangeFXMaterial = null;
    [SerializeField]
    private Color[] m_rangeFXColor;

    [SerializeField]
    private GameObject _jammingFX;
    private GameObject _jammingFXGO = null;

    [SerializeField]
    private GameObject _oneWayFX;
    private GameObject _oneWayFXGO = null;

    private GameObject _strongWillFX = null;

    private class FXList
    {
        public float _startTime;
        public float _applyTime;
        public GameObject _fx;
    }
    private List<FXList> _applyFX = new List<FXList>();

    void Awake()
    {
        for (int index = 0; index < _carExplosionFX.Count; ++index)
        {
            _carExplosionFXDic.Add(_carExplosionFX[index]._carID, _carExplosionFX[index]._explosionFX);
        }
    }

    void Update()
    {
        for (int index = 0; index < _applyFX.Count; ++index)
        {
            FXList list = _applyFX[index];
            if (list._applyTime <= Time.time - list._startTime)
            {
                if (null != list._fx)
                {
                    Destroy(list._fx);
                    list._fx = null;
                }

                _applyFX.Remove(list);
            }
        }
    }

    internal void ShowItemRangeFX(float aRange, int aColorIndex, bool aShow)
    {
        if (null == _rangeFX)
        {
            _rangeFX = Instantiate(_itemRangeFX) as GameObject;

            Projector proj = _rangeFX.GetComponent<Projector>();
            _rangeFXMaterial = proj.material;

            _rangeFXRotation = _rangeFX.transform.rotation;
            _rangeFX.transform.position = Vector3.zero;
            _rangeFX.transform.parent = _owner.CarTransform;
        }

        if (aShow)
        {
            if (0 <= aColorIndex && aColorIndex < m_rangeFXColor.Length)
            {
                _rangeFXMaterial.color = m_rangeFXColor[aColorIndex];
            }

            _rangeFX.transform.localPosition = new Vector3(0f, aRange * 3f, 0f);
            _rangeFX.transform.localRotation = _rangeFXRotation;
        }

        _rangeFX.SetActive(aShow);
    }

    internal void PlayCollideFX(CollideLoc aLoc)
    {
        PlayEffectParticle(_collideFX[(int)aLoc]);
    }

    internal void PlayLandingFX()
    {
        PlayEffectParticle(_landingFX);
    }

    internal void PlayBurnFX(float aRatio)
    {
        //if (!aCar.CarRenderInit)
        //{
        //    return;
        //}

        if (null == _hpFX)
        {
            _hpFX = PlayEffectParticle(_burnFX);
        }

        float rate = 1f - aRatio;
        ParticleSystem[] spawned = _hpFX.GetComponentsInChildren<ParticleSystem>();
        for (int index = 0; index < spawned.Length; ++index)
        {   
            // TO DO : check this out.
            //spawned[index].emission.rate = _burnFXMaxEmissionRate[index] * rate;
        }
    }

    internal void PlayExplosionFX(int aID)
    {
        if (_carExplosionFXDic.ContainsKey(aID))
        {
            PlayEffectParticle(_carExplosionFXDic[aID]);
        }
        else
        {
            PlayEffectParticle(_explosionFX);
        }
    }

    internal void PlayGetItemFX()
    {
        PlayEffectParticle(_getItemFX);
    }

    internal void PlayJammingFX(bool aShow)
    {
        if (aShow)
        {
            _jammingFXGO = PlayEffectParticle(_jammingFX);
        }
        else
        {
            Destroy(_jammingFXGO);
        }
    }

    internal void PlayOneWayFX(bool aShow)
    {
        if (aShow)
        {
            _oneWayFXGO = PlayEffectParticle(_oneWayFX);
        }
        else
        {
            Destroy(_oneWayFXGO);
        }
    }

    internal void PlayStrongWillFX(bool aShow, GameObject aGO)
    {
        if (aShow)
        {
            if (null == _strongWillFX)
            {
                _strongWillFX = PlayEffectParticle(aGO);
            }
        }
        else
        {
            Destroy(_strongWillFX);
            _strongWillFX = null;
        }
    }

    internal GameObject PlayEffectParticle(GameObject aGO)
    {
        if (null == aGO)
        {
            return null;
        }

        GameObject go = Instantiate(aGO, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.parent = _owner.CarTransform;

        TRCarEffectParticleLocalTransform localTransform = go.GetComponent<TRCarEffectParticleLocalTransform>();
        if (null != localTransform)
        {
            go.transform.localPosition = localTransform.m_localPosition;
            go.transform.localRotation = Quaternion.Euler(localTransform.m_localRotation);
        }

        return go;
    }

    internal void PlayEffectParticle(GameObject aGO, float aApplyTime)
    {
        if (null == aGO)
        {
            return;
        }
        
        GameObject go = Instantiate(aGO, Vector3.zero, Quaternion.identity) as GameObject;
        go.transform.parent = _owner.CarTransform;

        TRCarEffectParticleLocalTransform localTransform = go.GetComponent<TRCarEffectParticleLocalTransform>();
        if (null != localTransform)
        {
            go.transform.localPosition = localTransform.m_localPosition;
            go.transform.localRotation = Quaternion.Euler(localTransform.m_localRotation);
        }

        FXList list = new FXList();
        list._fx = go;
        list._startTime = Time.time;
        list._applyTime = aApplyTime;
        _applyFX.Add(list);
    }
}