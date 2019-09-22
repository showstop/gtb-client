using UnityEngine;
using System.Collections;

public class PartsTuningAnimationEventHandler : MonoBehaviour
{
    public void PresentationEndNotify()
    {
        EventManager.Instance.SendGameEvent(GameEventType.PartsTuningPresentationEnd);
    }
}