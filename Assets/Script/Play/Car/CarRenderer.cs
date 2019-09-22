using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarRenderer : MonoBehaviour 
{
    public GameObject ItemTop;
    public GameObject ItemBack;
    public GameObject wheel_FL;
    public GameObject wheel_FR;
    public GameObject wheel_RL;
    public GameObject wheel_RR;

    [SerializeField]
    private Material[] bodyMat = new Material[3];

    [SerializeField]
    public BoxCollider _collider;

    // TO DO
    [SerializeField]
    private Animation _carAnimation;
    [SerializeField]
    private AnimationClip[] _carAniclip;

    //[SerializeField]
    //private List<CarStat> _carStat = new List<CarStat>();
    //private GameObject _activeCar = null;

    private float _tireRadius = 0;

    void Start()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        YPLog.Log("child count = " + children.Length);
        for (int index = 0; index < children.Length; ++index)
        {
            Transform child = children[index];
            if ("Dum_top" == child.name)
            {
                ItemTop = child.gameObject;
            }
            else if ("Dum_back" == child.name)
            {
                ItemBack = child.gameObject;
            }
        } 
    }

    void Update()
    {
        RotateCarWheel();
    }

    internal void RotateCarWheel()
    {
        var cc = this.transform.parent.GetComponent<CarController>();
        if (null == cc) return;

        if(true == cc._matchStart && cc._hp > 0 && cc._speed > 0)
        {
            var wheelAngle = Vector3.right * cc._speed / _tireRadius * Time.deltaTime * Mathf.Rad2Deg;
            if (wheel_FL) wheel_FL.transform.Rotate(wheelAngle);
            if (wheel_FR) wheel_FR.transform.Rotate(wheelAngle);
            if (wheel_RL) wheel_RL.transform.Rotate(wheelAngle);
            if (wheel_RR) wheel_RR.transform.Rotate(wheelAngle);
        }
    }
 
    internal void PlayAnimation(string aName)
    {
        _carAnimation.Play(aName);
    }

    internal void StopAnimation()
    {
        _carAnimation.Stop();
    }

    internal IEnumerator DelaySpawnBody(int aBody)
    {
        while (!AssetBundleLoader.Instance.ReadyToUse)
        {
            yield return null;
        }

        var unit = AssetBundleLoader.Instance.GetVehicleResourceUnit(aBody);
        GameObject carBody = Instantiate(unit.BodyModel, Vector3.zero, Quaternion.identity) as GameObject;        
        MeshRenderer mr = carBody.GetComponentInChildren<MeshRenderer>();
        Material[] materials = new Material[mr.materials.Length];
        YPLog.Log("[SpawnBody] BodyMaterial = " + unit.BodyMaterial + ", WindowMaterial = " + unit.WindowMaterial + ", MetalMaterial = " + unit.MetalMaterial);
        for (int mIndex = 0; mIndex < materials.Length; ++mIndex)
        {
            switch (mIndex)
            {
                case 0: materials[mIndex] = unit.BodyMaterial; break;
                case 1: materials[mIndex] = unit.WindowMaterial; break;
                case 2: materials[mIndex] = unit.MetalMaterial; break;
            }

            Debug.Log("ShaderName:" + materials[mIndex].shader.name);
            materials[mIndex].shader = Shader.Find(materials[mIndex].shader.name);

#if UNITY_EDITOR
            // TO DO : check in mobile.
            materials[mIndex].shader = Shader.Find(materials[mIndex].shader.name);
#endif
        }

        mr.sharedMaterials = materials;

        SetDummyInfo(carBody);

        Animation ani = carBody.GetComponent<Animation>();
        if(ani == null)
        {
            ani = carBody.AddComponent<Animation>();
        }
        for (int i = 0; i < _carAniclip.Length; i++)
            ani.AddClip(_carAniclip[i], _carAniclip[i].name);

        ani.clip = _carAniclip[0];
        _carAnimation = ani;

        carBody.name = unit.Name;
        carBody.transform.parent = transform;
        carBody.transform.localPosition = Vector3.zero;
        carBody.transform.localRotation = Quaternion.identity;
    }

    public void SpawnBody(int aBody)
    {
        StartCoroutine(DelaySpawnBody(aBody));
    }

    internal IEnumerator DelayChangeTire(int aTire)
    {
        //YPLog.Trace();
        while (!AssetBundleLoader.Instance.ReadyToUse)
        {
            yield return null;
        }

        var unit = AssetBundleLoader.Instance.GetVehicleResourceUnit(aTire);        
        SpawnTire(unit.TireModel, unit.TireMaterial, wheel_FL.transform, 360f);
        SpawnTire(unit.TireModel, unit.TireMaterial, wheel_FR.transform, 0f);
        SpawnTire(unit.TireModel, unit.TireMaterial, wheel_RL.transform, 360f);
        SpawnTire(unit.TireModel, unit.TireMaterial, wheel_RR.transform, 0f);
    }

    private void SpawnTire(GameObject aTireGO, Material aMat, Transform aParent, float aRotY)
    {
        GameObject tire = Instantiate(aTireGO) as GameObject;
        tire.transform.parent = aParent;
        tire.transform.localPosition = Vector3.zero;
        tire.transform.localRotation = Quaternion.Euler(0f, aRotY, 0f);

        MeshRenderer mr = tire.GetComponent<MeshRenderer>();
        mr.sharedMaterial = aMat;

        _tireRadius = tire.transform.gameObject.GetComponent<Renderer>().bounds.size.y;
#if UNITY_EDITOR
        // TO DO : check in mobile.
        mr.sharedMaterial.shader = Shader.Find(mr.sharedMaterial.shader.name);
#endif
    }

    internal void SetDummyInfo(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        for (int index = 0; index < children.Length; ++index)
        {
            Transform child = children[index];
            if ("Dum_top" == child.name)
            {
                ItemTop = child.gameObject;
            }
            else if ("Dum_back" == child.name)
            {
                ItemBack = child.gameObject;
            }
            else if ("wheel_FL" == child.name)
            {
                wheel_FL = child.gameObject;
            }
            else if ("wheel_FR" == child.name)
            {
                wheel_FR = child.gameObject;
            }
            else if ("wheel_RL" == child.name)
            {
                wheel_RL = child.gameObject;
            }
            else if ("wheel_RR" == child.name)
            {
                wheel_RR = child.gameObject;
            }            
        }
    }

    //internal CarStat GetCarStat(int aCarID)
    //{
    //    var carStat = _carStat.Find(
    //            delegate(CarStat cs)
    //            {
    //                return cs._id == aCarID;
    //            });

    //    if (null != _activeCar)
    //    {
    //        _activeCar.SetActive(false);
    //    }
    //    _activeCar = carStat._go;
    //    _animation = _activeCar.GetComponentInChildren<Animation>();
    //    SetDummyInfo();
    //    MoveItems();

    //    _activeCar.SetActive(true);

    //    return carStat;
    //}

    //private void SetDummyInfo()
    //{
    //    Transform[] children = _activeCar.GetComponentsInChildren<Transform>();        
    //    for (int index = 0; index < children.Length; ++index)
    //    {
    //        Transform child = children[index];
    //        if ("Dum_top" == child.name)
    //        {   
    //            ItemTop = child.gameObject;
    //        }
    //        else if ("Dum_back" == child.name)
    //        {
    //            ItemBack = child.gameObject;
    //        }
    //    }
    //}

    //private void MoveItems()
    //{
    //    Item[] haveItems = gameObject.GetComponentsInChildren<Item>(true);
    //    //YPLog.Trace();
    //    //YPLog.Log("item count = " + haveItems.Length);

    //    for (int index = 0; index < haveItems.Length; ++index)
    //    {
    //        haveItems[index].gameObject.transform.parent = null;
    //        haveItems[index].gameObject.transform.localPosition = Vector3.zero;
    //        haveItems[index].gameObject.transform.localScale = Vector3.one;

    //        haveItems[index].AttachToCar();
    //    }
    //}
}
