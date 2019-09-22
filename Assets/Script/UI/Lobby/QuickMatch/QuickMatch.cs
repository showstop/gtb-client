using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuickMatch : GUIComponent
{
    [SerializeField]
    private ImageSelector _carClass;

    [SerializeField]
    private ImageSelector _carName;

    [SerializeField]
    private Slider _spd;

    [SerializeField]
    private Slider _acc;

    [SerializeField]
    private Slider _pow;

    [SerializeField]
    private Slider _hp;

    [SerializeField]
    private List<CarSkill> _skills = new List<CarSkill>();

    void Awake()
    {
        UpdateCarInfo();
    }

    public override void OnHandleEvent(GameEventType gameEventType, params System.Object[] args)
    {
        switch (gameEventType)
        {
            case GameEventType.VehicleSelectAnsOK:
                UpdateCarInfo();

                break;
        }
    }

    private void UpdateCarInfo()
    {
        var carInfo = PlayerDataRepository.Instance.GetSelectedCarInfo();
        Debug.Log(carInfo);
        _carClass.SetImage((int)carInfo.Level);
        _carName.SetImage(carInfo.VehicleNo);
        
        var carRegInfo = AssetBundleLoader.Instance.GetVehicleData(carInfo.VehicleNo);
        var motorLv = (int)carInfo.Parts[Constants.PARTS_CATEGORY.PC_MOTOR];
        _spd.value = (float)Calculator.CurrentUIUnit(
                         (Constants.VehicleLevel)carInfo.Level, 
                         (short)carRegInfo.SPD, 
                         Calculator.BASE_UI_UNIT_SPEED, 
                         (short)(motorLv / (float)Calculator.MaxUIUnit((short)carRegInfo.SPD, 
                                     Calculator.BASE_UI_UNIT_SPEED))) / 100.0f;     
        
        var susLv = (int)carInfo.Parts[Constants.PARTS_CATEGORY.PC_SUSPENSION];
        _acc.value = (float)Calculator.CurrentUIUnit(
                         (Constants.VehicleLevel)carInfo.Level, 
                         (short)carRegInfo.ACC, 
                         Calculator.BASE_UI_UNIT_ACCELERATION, 
                         (short)(susLv / (float)Calculator.MaxUIUnit((short)carRegInfo.ACC, 
                                     Calculator.BASE_UI_UNIT_ACCELERATION))) / 100.0f;    
        
        var bodykitLv = (int)carInfo.Parts[Constants.PARTS_CATEGORY.PC_BODY_KIT];
        _pow.value = (float)Calculator.CurrentUIUnit(
                         (Constants.VehicleLevel)carInfo.Level, 
                         (short)carRegInfo.POW, 
                         Calculator.BASE_UI_UNIT_POWER, 
                         (short)(bodykitLv / (float)Calculator.MaxUIUnit((short)carRegInfo.POW, 
                                     Calculator.BASE_UI_UNIT_POWER))) / 100.0f;  
        
        var batteryLv = (int)carInfo.Parts[Constants.PARTS_CATEGORY.PC_BATTERY];
        _hp.value = (float)Calculator.CurrentUIUnit(
                        (Constants.VehicleLevel)carInfo.Level, 
                        (short)carRegInfo.HP, 
                        Calculator.BASE_UI_UNIT_HP, 
                        (short)(batteryLv / (float)Calculator.MaxUIUnit((short)carRegInfo.HP, 
                                    Calculator.BASE_UI_UNIT_HP))) / 100.0f; 

        HideAllSkills();
        if (carRegInfo.SKILL01 > 0)
        {
            _skills[0].SetImage((int)carInfo.Level, carRegInfo.SKILL01);
            _skills[0].gameObject.SetActive(true);
        }
        if (carRegInfo.SKILL02 > 0)
        {
            _skills[1].SetImage((int)carInfo.Level, carRegInfo.SKILL02);
            _skills[1].gameObject.SetActive(true);
        }

        if (carRegInfo.SKILL03 > 0)
        {
            _skills[2].SetImage((int)carInfo.Level, carRegInfo.SKILL03);
            _skills[2].gameObject.SetActive(true);
        }
    }

    private void HideAllSkills()
    {
        foreach (CarSkill cs in _skills)
        {
            cs.gameObject.SetActive(false);
        }
    }
}