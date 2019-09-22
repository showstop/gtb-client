using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Network;

public class PartsTuning : PopupComponent 
{
    [SerializeField]
    private ImageSelector _icon;

    [SerializeField]
    private TextSelector _name;

    [SerializeField]
    private TextSelector _desc;

    [SerializeField]
    private List<ImageSelector> _normalMaterials = new List<ImageSelector>();

    [SerializeField]
    private List<Text> _normalCount = new List<Text>();

    [SerializeField]
    private ImageSelector _specialMaterial;

    [SerializeField]
    private Text _specialCount;

    [SerializeField]
    private TextSelector _statName;

    [SerializeField]
    private Slider _current;

    [SerializeField]
    private Slider _next;

    [SerializeField]
    private Button _tunning;

    [SerializeField]
    private List<GameObject> _presentationGO = new List<GameObject>();

    [SerializeField]
    private List<GameObject> _backgroundFX = new List<GameObject>();

    [SerializeField]
    private float _backgroundFXShowTime;

    private Constants.PARTS_CATEGORY _id;
    private const string Animator_Param_PartsTuningPresentation = "PartsTuningPresentation";

    internal void SetInfo(Constants.PARTS_CATEGORY aID)
    {
        _id = aID;

        ToggleBackgroundFX(false);

        var carInfo = PlayerDataRepository.Instance.GetCarInfo(PlayerDataRepository.Instance.DisplayCarNo);
        var partsLevel = carInfo.Parts[_id];

        int partsID = (int)_id;
        int iconID = partsID * 10 + partsLevel;
        _icon.SetImage(iconID);
        _name.SetText(partsID);
        _desc.SetText(partsID);

        int count = 0;
        bool canTuning = true;
        var stuffInfo = PlayerDataRepository.Instance.GetPartsTuningStuff(_id, partsLevel + 1);
        Dictionary<int, int> materials = stuffInfo.Stuff;
        foreach (KeyValuePair<int, int> kv in materials)
        {
            int have = PlayerDataRepository.Instance.GetStuffCount(kv.Key);
            if (count < 3)
            {
                _normalMaterials[count].SetImage(kv.Key);
                _normalCount[count].text = StringFormat.CurrentWithMax(have, kv.Value);
                if (have < kv.Value)
                {
                    canTuning = false;
                }

                ++count;
            }
            else
            {
                _specialMaterial.SetImage(kv.Key);
                _specialCount.text = StringFormat.CurrentWithMax(have, kv.Value);
                if (have < kv.Value)
                {
                    canTuning = false;
                }
            }
        }

        _statName.SetText(partsID);

        var carRegInfo = AssetBundleLoader.Instance.GetVehicleData(PlayerDataRepository.Instance.DisplayCarNo);
        int basicUnit = 0;
        int baseUnit = 0;
        switch (_id)
        {
            case Constants.PARTS_CATEGORY.PC_MOTOR:
                basicUnit = carRegInfo.SPD;
                baseUnit = Calculator.BASE_UI_UNIT_SPEED;
                break;

            case Constants.PARTS_CATEGORY.PC_SUSPENSION:
                basicUnit = carRegInfo.ACC;
                baseUnit = Calculator.BASE_UI_UNIT_ACCELERATION;
                break;

            case Constants.PARTS_CATEGORY.PC_BODY_KIT:
                basicUnit = carRegInfo.POW;
                baseUnit = Calculator.BASE_UI_UNIT_POWER;
                break;

            case Constants.PARTS_CATEGORY.PC_BATTERY:
                basicUnit = carRegInfo.HP;
                baseUnit = Calculator.BASE_UI_UNIT_HP;
                break;
        }

        int maxUnit = Calculator.MaxUIUnit((short)basicUnit, baseUnit);
        int currentUnit = Calculator.CurrentUIUnit(
            (Constants.VehicleLevel)carInfo.Level, 
            (short)basicUnit, baseUnit, partsLevel);
        int nextUnit = Calculator.CurrentUIUnit(
            (Constants.VehicleLevel)carInfo.Level, 
            (short)basicUnit, baseUnit, (short)(partsLevel + 1));

        _current.value = (float)currentUnit / (float)maxUnit;
        _next.value = (float)nextUnit / (float)maxUnit;

        _tunning.interactable = canTuning;
    }

    private void ToggleBackgroundFX(bool aShow)
    {
        for (int index = 0; index < _backgroundFX.Count; ++index)
        {
            _backgroundFX[index].SetActive(aShow);
        }
    }

    public override void OnHandleEvent(GameEventType gameEventType, params System.Object[] args)
    {
        switch (gameEventType)
        {
            case GameEventType.VehiclePartsTuningAnsOK:
                _tunning.interactable = false;

                foreach (GameObject go in _presentationGO)
                {
                    go.SetActive(true);
                }
                _animator.SetBool(Animator_Param_PartsTuningPresentation, true);

                break;

            case GameEventType.PartsTuningPresentationEnd:
                SetInfo(_id);                
                _animator.SetBool(Animator_Param_PartsTuningPresentation, false);

                StartCoroutine(ShowBackgroundFX());

                break;
        }
    }

    private IEnumerator ShowBackgroundFX()
    {
        ToggleBackgroundFX(true);
        yield return new WaitForSeconds(_backgroundFXShowTime);

        ToggleBackgroundFX(false);
    }

    public void Tuning()
    {
        LobbyRequest.Instance.CarPartsTuningReq(PlayerDataRepository.Instance.DisplayCarNo, _id);
    }
}