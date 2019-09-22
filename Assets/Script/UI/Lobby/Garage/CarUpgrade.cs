using UnityEngine;
using System.Collections.Generic;

public class CarUpgrade : MonoBehaviour
{
    [SerializeField]
    private GameObject _page;

    [SerializeField]
    private GameObject _base;

    [SerializeField]
    private List<ImageSelector> _icons = new List<ImageSelector>();

    [SerializeField]
    private ImageSelector _class;

    [SerializeField]
    private ImageSelector _name;

    [SerializeField]
    private GameObject _upgradeFX;

    void OnEnable()
    {
        _page.SetActive(false);
        _base.SetActive(false);
    }

    void OnDisable()
    {
        _page.SetActive(true);
        _base.SetActive(true);
        _upgradeFX.SetActive(false);
    }

    internal void SetInfo(PlayerDataRepository.CarInfo aInfo)
    {
        Debug.Log(_class);
        Debug.Log(aInfo);
        _class.SetImage((int)aInfo.Level);
        _name.SetImage(aInfo.VehicleNo);
        for (int index = 0; index < _icons.Count; ++index)
        {
            _icons[index].SetImage(aInfo.VehicleNo);
        }
    }

    // call from animation
    public void ShowFX()
    {
        _upgradeFX.SetActive(true);
    }

    // call from animation
    public void EndPresentation()
    {
        EventManager.Instance.SendGameEvent(GameEventType.UpgradeCarPresentationEnd);
    }
}