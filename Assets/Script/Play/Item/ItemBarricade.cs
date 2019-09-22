using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemBarricade : Item 
{
    [SyncVar]
    public int _laneNO;

    public override void OnStartClient()
    {
        YPLog.Log("show = " + _show + ", playerUUID = " + _carPlayerUUID);
        _owner = GameObject.FindWithTag(Constants.GAME_MANAGER_TAG_NAME).GetComponent<GameManager>().FindCarWithPlayerUUID(_carPlayerUUID);
        _swc.Spline = _owner._swc.Spline;
        _swc.InitialF = _owner._swc.TF;

        _itemGO.transform.localPosition = new Vector3(_swc.Spline.GetLaneOffsetX(_laneNO), _itemGO.transform.localPosition.y + _spawnOffsetY, _itemGO.transform.localPosition.z);

        _currentOffsetY = _itemGO.transform.localPosition.y;
        _jumpState = Constants.JumpState.GoingDown;

        ToggleItemFX(true);
        _owner.UseItemFX(_useItemFX, _useItemSound);
    }

    internal override void GiveTo(CarController aCar)
    {   
        _owner = aCar;
        _carPlayerUUID = aCar._playerNo;
    }
}