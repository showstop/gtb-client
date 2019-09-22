using UnityEngine;
using System.Collections.Generic;

public class TRPanel_PlayerStatus : TRPanel
{
    [SerializeField]
    private TRUnit_PlayerStatus[] _status = new TRUnit_PlayerStatus[Constants.MAX_PLAYER_NUM];

    private TRUnit_PlayerStatus _mine;

    //public void SetupPlayerStatus(TRCarController aPlayer)
    internal void SetupPlayerStatus(CarController aCar)
    {
        int index = aCar._laneNO - 1;
        if (0 > index || index >= Constants.MAX_PLAYER_NUM)
        {
            YPLog.Log("SetupPlayerStatus, over player!, index = " + index);
            return;
        }
        
        _status[index].SetupPlayerStatus(aCar);
        if( aCar.isLocalPlayer)
        {
            _mine = _status[index];
            StartCoroutine(_mine.Indicate());
        }
    }

    //public void ShowMyCar(string aPlayerUUID)
    //{
    //    for (int index = 0; index < _status.Length; ++index)
    //    {
    //        if (aPlayerUUID == _status[index].PlayerUUID)
    //            StartCoroutine(_status[index].ShowMyCar());
    //    }
    //}
}
