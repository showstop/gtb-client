using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarSelectPopup : GUIComponent
{
    //minicoop
    public Slider _minicoopSPD;
    public Slider _minicoopACC;
    public Slider _minicoopPOW;
    public Slider _minicoopHP;
    //buggy
    public Slider _buggySPD;
    public Slider _buggyACC;
    public Slider _buggyPOW;
    public Slider _buggyHP;
    //turbin
    public Slider _turbinSPD;
    public Slider _turbinACC;
    public Slider _turbinPOW;
    public Slider _turbinHP;

    private float _minimaxSPD = 44f;
    private float _minimaxACC = 59f;
    private float _minimaxPOW = 44f;
    private float _minimaxHP = 164f;

    private float _buggymaxSPD = 51f;
    private float _buggymaxACC = 66f;
    private float _buggymaxPOW = 37f;
    private float _buggymaxHP = 157f;

    private float _turbinmaxSPD = 37f;
    private float _turbinmaxACC = 52f;
    private float _turbinmaxPOW = 51f;
    private float _turbinmaxHP = 171f;

    void Start ()
    {
        CarBaseState();
    }

    void CarBaseState()
    {
        //protocol.vehicle carInfo = PlayerDataRepository.Instance.GetSelectCarInfo();
        _minicoopSPD.value = PercentCalculation(24f, _minimaxSPD);
        _minicoopACC.value = PercentCalculation(34f, _minimaxACC);
        _minicoopPOW.value = PercentCalculation(24f, _minimaxPOW);
        _minicoopHP.value = PercentCalculation(104f, _minimaxHP);

        _buggySPD.value = PercentCalculation(26f, _buggymaxSPD);
        _buggyACC.value = PercentCalculation(36f, _buggymaxACC);
        _buggyPOW.value = PercentCalculation(22f, _buggymaxPOW);
        _buggyHP.value = PercentCalculation(102f, _buggymaxHP);

        _turbinSPD.value = PercentCalculation(22f, _turbinmaxSPD);
        _turbinACC.value = PercentCalculation(32f, _turbinmaxACC);
        _turbinPOW.value = PercentCalculation(26f, _turbinmaxPOW);
        _turbinHP.value = PercentCalculation(106f, _turbinmaxHP);

        //_minicoopSPD.value = (float)Calculator.CurrentUIUnit(carInfo.GetLevel()+1, 4, Calculator.BASE_UI_UNIT_SPEED, 0) / (float)Calculator.MaxUIUnit(4, Calculator.BASE_UI_UNIT_SPEED);
        //_minicoopACC.value = (float)Calculator.CurrentUIUnit(carInfo.GetLevel()+1, 4, Calculator.BASE_UI_UNIT_ACCELERATION, 0) / (float)Calculator.MaxUIUnit(4, Calculator.BASE_UI_UNIT_ACCELERATION);
        //_minicoopPOW.value = (float)Calculator.CurrentUIUnit(carInfo.GetLevel()+1, 4, Calculator.BASE_UI_UNIT_POWER, 0) / (float)Calculator.MaxUIUnit(4, Calculator.BASE_UI_UNIT_POWER);
        //_minicoopHP.value = (float)Calculator.CurrentUIUnit(carInfo.GetLevel()+1, 4, Calculator.BASE_UI_UNIT_HP, 0) / (float)Calculator.MaxUIUnit(4, Calculator.BASE_UI_UNIT_HP);

        //_buggySPD.value = (float)Calculator.CurrentUIUnit(carInfo.GetLevel() + 1, 6, Calculator.BASE_UI_UNIT_SPEED, 0) / (float)Calculator.MaxUIUnit(6, Calculator.BASE_UI_UNIT_SPEED);
        //_buggyACC.value = (float)Calculator.CurrentUIUnit(carInfo.GetLevel()+1, 6, Calculator.BASE_UI_UNIT_ACCELERATION, 0) / (float)Calculator.MaxUIUnit(6, Calculator.BASE_UI_UNIT_ACCELERATION);
        //_buggyPOW.value = (float)Calculator.CurrentUIUnit(carInfo.GetLevel()+1, 2, Calculator.BASE_UI_UNIT_POWER, 0) / (float)Calculator.MaxUIUnit(2, Calculator.BASE_UI_UNIT_POWER);
        //_buggyHP.value = (float)Calculator.CurrentUIUnit(carInfo.GetLevel()+1, 2, Calculator.BASE_UI_UNIT_HP, 0) / (float)Calculator.MaxUIUnit(2, Calculator.BASE_UI_UNIT_HP);

        //_turbinSPD.value = (float)Calculator.CurrentUIUnit(carInfo.GetLevel()+1, 2, Calculator.BASE_UI_UNIT_SPEED, 0) / (float)Calculator.MaxUIUnit(2, Calculator.BASE_UI_UNIT_SPEED);
        //_turbinACC.value = (float)Calculator.CurrentUIUnit(carInfo.GetLevel()+1, 2, Calculator.BASE_UI_UNIT_ACCELERATION, 0) / (float)Calculator.MaxUIUnit(2, Calculator.BASE_UI_UNIT_ACCELERATION);
        //_turbinPOW.value = (float)Calculator.CurrentUIUnit(carInfo.GetLevel()+1, 6, Calculator.BASE_UI_UNIT_POWER, 0) / (float)Calculator.MaxUIUnit(6, Calculator.BASE_UI_UNIT_POWER);
        //_turbinHP.value = (float)Calculator.CurrentUIUnit(carInfo.GetLevel()+1, 6, Calculator.BASE_UI_UNIT_HP, 0) / (float)Calculator.MaxUIUnit(6, Calculator.BASE_UI_UNIT_HP);


    }

    private float PercentCalculation(float aBase, float aMax)
    {
        float percent = 100f;
        return (aBase / aMax) * percent;
    }
}
