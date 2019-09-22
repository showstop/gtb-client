using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lobby : GUIComponent 
{
    [SerializeField]
    private Transform _carParent;

    [SerializeField]
    private GameObject _blackPanel;

    [SerializeField]
    private GameObject _tutorialLobby;

    private Dictionary<int, GameObject> _cars = new Dictionary<int, GameObject>();
    private int _currentID = -1;

    void Awake()
    {
        if (TutorialConstants._tutorialPlaying)
        {
            _blackPanel.SetActive(true);
            _tutorialLobby.SetActive(true);
        }
        else
        {
            _tutorialLobby.SetActive(false);
        }

        if (PlayerDataRepository.Instance.GetCarList().Count > 0)
        {
            LoadLobbyCar();
        }
    }

    void LoadLobbyCar()
    {
        var carList = PlayerDataRepository.Instance.GetCarList();
        for (int i = 0; i < carList.Count; i++)
        {
            bool isSelected = false;
            if (carList[i].VehicleNo == PlayerDataRepository.Instance.SelectCarNo)
            {
                _currentID = carList[i].VehicleNo;
                isSelected = true;
            }

            SpawnCar(carList[i].VehicleNo, isSelected);
        }
    }
    
    void Start()
    {
        StartCoroutine(DelayTutorial());
    }

    IEnumerator DelayTutorial()
    {
        yield return new WaitForSeconds(1f);

        if (PlayerDataRepository.Instance.TutorialProgress != Constants.TutorialProgress.ALL_COMPLETE)
        {
            TutorialManager.Instance.LobbyInit();
            _blackPanel.SetActive(false);
        }
    }

    public override void OnHandleEvent(GameEventType gameEventType, params object[] args)
    {
        switch (gameEventType)
        {
            case GameEventType.ChangeShowRoomCar:
                _cars[_currentID].SetActive(false);
                _currentID = (int)args[0];
                _cars[_currentID].SetActive(true);

                break;

            case GameEventType.VehicleSelectAnsOK:
                _cars[_currentID].SetActive(false);
                PlayerDataRepository.Instance.SelectCarNo = TutorialManager.Instance._firstCarNumber;
                _currentID = PlayerDataRepository.Instance.SelectCarNo;
                _cars[_currentID].SetActive(true);
                break;
        }
    }

    private void SpawnCar(int aID, bool aSelected)
    {
        YPLog.Log("=== [SpawnCar] id = " + aID + ", selected = " + aSelected);
        var unit = AssetBundleLoader.Instance.GetVehicleResourceUnit(aID);

        YPLog.Log("=== [SpawnCar] id = " + aID + ", bodyM = " + unit.BodyMaterial + ", tireM = " + unit.TireMaterial + ", windowM = " + unit.WindowMaterial + ", metalM = " + unit.MetalMaterial);
        GameObject car = Instantiate(unit.BodyModel, Vector3.zero, Quaternion.identity) as GameObject;
        Transform[] children = car.GetComponentsInChildren<Transform>();
        MeshRenderer mr = car.GetComponentInChildren<MeshRenderer>();
        for (int index = 0; index < children.Length; ++index)
        {
            Transform child = children[index];
            if ("wheel_FL" == child.name)
            {
                SpawnTire(unit.TireModel, unit.TireMaterial, 360f, child);
            }
            else if ("wheel_FR" == child.name)
            {
                SpawnTire(unit.TireModel, unit.TireMaterial, 0f, child);
            }
            else if ("wheel_RL" == child.name)
            {
                SpawnTire(unit.TireModel, unit.TireMaterial, 360f, child);
            }
            else if ("wheel_RR" == child.name)
            {
                SpawnTire(unit.TireModel, unit.TireMaterial, 0f, child);
            }
            else if (mr.name == child.name)
            {
                int count = mr.sharedMaterials.Length;                
                if (1 < count)
                {
                    Material[] materials = new Material[count];
                    for (int mIndex = 0; mIndex < count; ++mIndex)
                    {
                        Material targetM = null;
                        switch (mIndex)
                        {
                            case 0: targetM = unit.BodyMaterial;    break;
                            case 1: targetM = unit.WindowMaterial;  break;
                            case 2: targetM = unit.MetalMaterial;   break;

                            default:
                                break;
                        }

                        materials[mIndex] = targetM;
#if UNITY_EDITOR
                        // TO DO : check in mobile.                        
                        materials[mIndex].shader = Shader.Find(materials[mIndex].shader.name);
#endif
                    }

                    mr.sharedMaterials = materials;
                }
                else
                {
                    mr.sharedMaterial = unit.BodyMaterial;
#if UNITY_EDITOR
                    // TO DO : check in mobile.
                    mr.sharedMaterial.shader = Shader.Find(mr.sharedMaterial.shader.name);
#endif
                }
            }
        }

        car.layer = LayerMask.NameToLayer(Constants.LAYER_NAME_CAR);
        car.name = unit.Name;
        car.transform.parent = _carParent;
        car.transform.localPosition = Vector3.zero;
        car.transform.localRotation = Quaternion.identity;
        car.SetActive(aSelected);        

        _cars.Add(aID, car);
    }

    private void SpawnTire(GameObject aModel, Material aMaterial, float aRotY, Transform aParent)
    {
        GameObject tire = Instantiate(aModel) as GameObject;
        MeshRenderer mr = tire.GetComponent<MeshRenderer>();
        mr.sharedMaterial = aMaterial;
#if UNITY_EDITOR
        // TO DO : check in mobile.
        mr.sharedMaterial.shader = Shader.Find(mr.sharedMaterial.shader.name);
#endif

        tire.transform.parent = aParent;
        tire.transform.localPosition = Vector3.zero;
        tire.transform.localRotation = Quaternion.Euler(0, aRotY, 0);
    }
}