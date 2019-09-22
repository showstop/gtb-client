using UnityEngine;
using System.Collections;
using Network;

public class QuickMatchStart : PopupComponent
{
    [SerializeField]
    private GameObject _unit;

    [SerializeField]
    private RectTransform _listTransform;

    void Awake()
    {
        //UpdateList();
    }

    internal void UpdateList()
    {
        ClearListUnit();

        int count = PlayerDataRepository.Instance.GetOwnCarCount();
        float unitWidth = _unit.GetComponent<RectTransform>().rect.width;
        float scrollWidth = count * unitWidth;
        _listTransform.offsetMin = new Vector2(-scrollWidth / 2, _listTransform.offsetMin.y);
        _listTransform.offsetMax = new Vector2(scrollWidth / 2 + unitWidth / 4f, _listTransform.offsetMax.y);

        // selected car
        GameObject select = Instantiate(_unit) as GameObject;
        select.transform.SetParent(_listTransform.gameObject.transform);
        select.transform.localScale = Vector3.one;
        CarListUnit selectUnit = select.GetComponent<CarListUnit>();
        selectUnit.UpdateInfo(PlayerDataRepository.Instance.GetSelectedCarInfo());

        // others
        var entireList = PlayerDataRepository.Instance.GetCarList();
        for (int index = 0; index < entireList.Count; ++index)
        {
            var vehicle = entireList[index];
            if (Constants.VehicleLevel.LOCKED == (Constants.VehicleLevel)vehicle.Level)
                continue;

            if (PlayerDataRepository.Instance.SelectCarNo == vehicle.VehicleNo)
                continue;

            GameObject unit = Instantiate(_unit) as GameObject;
            unit.transform.SetParent(_listTransform.gameObject.transform);
            unit.transform.localScale = Vector3.one;

            CarListUnit listUnit = unit.GetComponent<CarListUnit>();
            listUnit.UpdateInfo(vehicle);
        }

        Transform transform = _listTransform.gameObject.transform;
        transform.localPosition = new Vector3(_listTransform.rect.width / -2f, 
            transform.localPosition.y, transform.localPosition.y);
    }

    public override void OnHandleEvent(GameEventType gameEventType, params System.Object[] args)
    {
        switch (gameEventType)
        {
            case GameEventType.VehicleSelectAnsOK:
                UpdateList();

                break;
        }
    }

    private void ClearListUnit()
    {
        CarListUnit[] units = _listTransform.GetComponentsInChildren<CarListUnit>();
        for (int index = units.Length - 1; index > 0; --index)
        {
            Destroy(units[index].gameObject);
        }
    }

    public void StartQuickMatch()
    {
        LobbyRequest.Instance.MatchStartReq();
        
        System.Diagnostics.Process ps = new System.Diagnostics.Process();
        ps.StartInfo.FileName = "GTB_20170111_ps.exe";
        ps.StartInfo.Arguments = "-playserver -ps_port=7071 -track=1002 -ls_port=1018";
        ps.Start();
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("scene_map_1002");
    }
}