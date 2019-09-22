using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AttendRewardUnit : GUIComponent
{
    [SerializeField]
    private Button _receiveReward;

    [SerializeField]
    private GameObject _received;    

    public int DaySequence;    

    public void UpdateState(bool aReceiveReward)
    {
        _receiveReward.interactable = !aReceiveReward;
        _received.SetActive(aReceiveReward);
    }

    public void RequestReward()
    {
        EventManager.Instance.SendGameEvent(GameEventType.RequestAttendReward, DaySequence);        
    }
}