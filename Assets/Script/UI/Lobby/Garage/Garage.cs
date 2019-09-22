using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Network;

public class Garage : GUIComponent 
{
    [SerializeField]
    private ImageSelector _class;

    [SerializeField]
    private GameObject _lock;

    [SerializeField]
    private ImageSelector _name;

    [SerializeField]
    private Slider _piece;

    [SerializeField]
    private Text _pieceCount;

    [SerializeField]
    private Button _makeButton;

    [SerializeField]
    private Button _upgradeButton;

    [SerializeField]
    private List<CarSkill> _skills = new List<CarSkill>();

    [SerializeField]
    private ImageSelector _selected;

    [SerializeField]
    private ImageSelector _motor;

    [SerializeField]
    private ImageSelector _suspension;

    [SerializeField]
    private ImageSelector _bodykit;

    [SerializeField]
    private ImageSelector _battery;

    [SerializeField]
    private Slider _spd;

    [SerializeField]
    private Slider _acc;

    [SerializeField]
    private Slider _pow;

    [SerializeField]
    private Slider _hp;

    [SerializeField]
    private List<int> _carOrder = new List<int>();
    private int _currentIndex = -1;

    void Awake()
    {
        var carList = PlayerDataRepository.Instance.GetCarList();
        for (int i=0;i<carList.Count;i++)
            _carOrder.Add(carList[i].VehicleNo);
        
        _currentIndex = _carOrder.IndexOf(PlayerDataRepository.Instance.SelectCarNo);
        PlayerDataRepository.Instance.DisplayCarNo = PlayerDataRepository.Instance.SelectCarNo;
        UpdateInfo();
    }

    private void UpdateInfo()
    {    
        var carInfo = PlayerDataRepository.Instance.GetCarInfo(_carOrder[_currentIndex]);
        if (carInfo.Level == 0)
        {
            _class.gameObject.SetActive(false);
            _selected.gameObject.SetActive(false);

            // by anemos
            // TO DO : check current car piece
            _makeButton.interactable = true;
            _lock.SetActive(true);
        }
        else
        {
            // by anemos
            // TO DO : check current car piece
            _upgradeButton.interactable = true;
            _class.SetImage((int)carInfo.Level);            
            _class.gameObject.SetActive(true);
            
            _selected.SetImage(PlayerDataRepository.Instance.SelectCarNo == carInfo.VehicleNo ? 1 : 0);
            _selected.gameObject.SetActive(true);

            _lock.SetActive(false);            
        }
        
        // by anemos
        // TO DO : check next level car piece
        Debug.Log("VehicleNo:" + carInfo.VehicleNo);
        var carRegInfo = AssetBundleLoader.Instance.GetVehicleData(carInfo.VehicleNo);

        int pieceMax = PlayerDataRepository.Instance.GetVehicleUpgradeInfo(
            Constants.GetVehicleGrade(carInfo.VehicleNo),
            (Constants.VehicleLevel) carInfo.Level + 1);
        _piece.value = (float)carInfo.HoldCard / (float)pieceMax;
        _pieceCount.text = StringFormat.CurrentWithMax(carInfo.HoldCard, pieceMax);
        _name.SetImage(carInfo.VehicleNo);
        
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

        var motorLv = (int)carInfo.Parts[Constants.PARTS_CATEGORY.PC_MOTOR];
        _motor.SetImage(motorLv);
        _spd.value = (float)Calculator.CurrentUIUnit(
                         (Constants.VehicleLevel)carInfo.Level, 
                         (short)carRegInfo.SPD, 
                         Calculator.BASE_UI_UNIT_SPEED, 
                         (short)(motorLv / (float)Calculator.MaxUIUnit((short)carRegInfo.SPD, 
                                     Calculator.BASE_UI_UNIT_SPEED))) / 100.0f;     
        
        var susLv = (int)carInfo.Parts[Constants.PARTS_CATEGORY.PC_SUSPENSION];
        _suspension.SetImage(susLv);
        _acc.value = (float)Calculator.CurrentUIUnit(
            (Constants.VehicleLevel)carInfo.Level, 
            (short)carRegInfo.ACC, 
            Calculator.BASE_UI_UNIT_ACCELERATION, 
            (short)(susLv / (float)Calculator.MaxUIUnit((short)carRegInfo.ACC, 
                        Calculator.BASE_UI_UNIT_ACCELERATION))) / 100.0f;    
        
        var bodykitLv = (int)carInfo.Parts[Constants.PARTS_CATEGORY.PC_BODY_KIT];
        _bodykit.SetImage(bodykitLv);
        _pow.value = (float)Calculator.CurrentUIUnit(
            (Constants.VehicleLevel)carInfo.Level, 
            (short)carRegInfo.POW, 
            Calculator.BASE_UI_UNIT_POWER, 
            (short)(bodykitLv / (float)Calculator.MaxUIUnit((short)carRegInfo.POW, 
                        Calculator.BASE_UI_UNIT_POWER))) / 100.0f;  
        
        var batteryLv = (int)carInfo.Parts[Constants.PARTS_CATEGORY.PC_BATTERY];
        _battery.SetImage(batteryLv);
        _hp.value = (float)Calculator.CurrentUIUnit(
            (Constants.VehicleLevel)carInfo.Level, 
            (short)carRegInfo.HP, 
            Calculator.BASE_UI_UNIT_HP, 
            (short)(batteryLv / (float)Calculator.MaxUIUnit((short)carRegInfo.HP, 
                        Calculator.BASE_UI_UNIT_HP))) / 150.0f; 
     
        Debug.LogFormat("SPD:{0}, ACC:{1}, POW:{2}, HP:{3}", _spd.value, _acc.value, _pow.value, _hp.value);
    }

    private void HideAllSkills()
    {
        foreach (CarSkill cs in _skills)
        {
            cs.gameObject.SetActive(false);
        }
    }

    public override void OnHandleEvent(GameEventType gameEventType, params System.Object[] args)
    {
        switch (gameEventType)
        {
            case GameEventType.VehicleUpgradeAnsOK:
                UpdateInfo();
                var upgrade = PlayerDataRepository.Instance.GetCarInfo(_carOrder[_currentIndex]);
                EventManager.Instance.SendGameEvent(GameEventType.UpgradeCarPresentationStart, upgrade);
                break;

            case GameEventType.VehiclePartsTuningAnsOK:
                UpdateInfo();
                break;
        }
    }

    public void Upgrade()
    {        
        LobbyRequest.Instance.UpgradeCarReq(_carOrder[_currentIndex]);
    }

    public void Prev()
    {
        --_currentIndex;
        if (0 > _currentIndex)
        {
            _currentIndex = _carOrder.Count - 1;
        }

        PlayerDataRepository.Instance.DisplayCarNo = _carOrder[_currentIndex];
        EventManager.Instance.SendGameEvent(GameEventType.ChangeShowRoomCar, _carOrder[_currentIndex]);
        UpdateInfo();
    }

    public void Next()
    {
        ++_currentIndex;
        if (_carOrder.Count <= _currentIndex)
        {
            _currentIndex = 0;
        }

        PlayerDataRepository.Instance.DisplayCarNo = _carOrder[_currentIndex];
        EventManager.Instance.SendGameEvent(GameEventType.ChangeShowRoomCar, _carOrder[_currentIndex]);
        UpdateInfo();
    }
}