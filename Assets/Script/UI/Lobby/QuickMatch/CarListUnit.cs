using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarListUnit : MonoBehaviour
{
    [SerializeField]
    private ImageSelector _rank;

    [SerializeField]
    private ImageSelector _name;

    [SerializeField]
    private GameObject _selected;

    [SerializeField]
    private ImageSelector _icon;

    [SerializeField]
    private Button _select;

    private int _id;

    internal void UpdateInfo(PlayerDataRepository.CarInfo aInfo)
    {
        _id = aInfo.VehicleNo;

        _rank.SetImage((int)aInfo.Level);
        _name.SetImage(_id);
        _selected.SetActive(_id == PlayerDataRepository.Instance.SelectCarNo);
        _select.interactable = (_id != PlayerDataRepository.Instance.SelectCarNo);
        _icon.SetImage(_id);
    }

    public void ChangeSelectCar()
    {
        EventManager.Instance.SendGameEvent(GameEventType.ChangeSelectCar, _id);
    }
}