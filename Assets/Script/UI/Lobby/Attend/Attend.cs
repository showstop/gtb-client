using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Attend : PopupComponent
{
    [SerializeField]
    private List<AttendRewardUnit> _list = new List<AttendRewardUnit>();

    [SerializeField]
    private List<AttendRewardUnit> _monthlyList = new List<AttendRewardUnit>();

    [SerializeField]
    private Button _monlthyReward;

    private int _requestRewardDay = -1;
    private int _rewardCount = 0;
    private const int Monthly_Attendance = 28;

    void Start()
    {
        // TO DO : current attend day sequence..
        int monthyRewardCount = 0;
//        foreach(protocol.attendance_info ai in PlayerDataRepository.Instance.AttendanceInfo.GetInfos())
//        {   
//            if (ai.GetDaySeq() > Monthly_Attendance)
//            {
//                monthyRewardCount++;
//            }
//
//            AttendRewardUnit target = FindRewardUnit(ai.GetDaySeq());
//            target.UpdateState(ai.GetReceiveReward());
//            if( ai.GetReceiveReward())
//            {
//                ++_rewardCount;
//            }
//        }
//        
//        if (monthyRewardCount > 0)
//        {
//            _monlthyReward.interactable = false;
//            for (int index = 0; index < _monthlyList.Count; ++index)
//            {
//                _monthlyList[index].UpdateState(true);
//            }            
//        }
//        else
//        {
//            _monlthyReward.interactable = (Monthly_Attendance == _rewardCount);
//        }
    }

    public override void OnHandleEvent(GameEventType gameEventType, params object[] args)
    {
        switch (gameEventType)
        {
            case GameEventType.RequestAttendReward:
                _requestRewardDay = (int)args[0];
                //LSConnector.Instance.AttendanceReceiveRewardReq(_requestRewardDay);

                break;

            case GameEventType.AttendanceReceiveRewardAnsOK:
                FindRewardUnit(_requestRewardDay).UpdateState(true);
                _requestRewardDay = -1;

                ++_rewardCount;
                _monlthyReward.interactable = (Monthly_Attendance == _rewardCount);

                break;
        }
    }

    private AttendRewardUnit FindRewardUnit(int aDay)
    {
        return _list.Find(delegate (AttendRewardUnit unit) { return unit.DaySequence == aDay; });
    }
}