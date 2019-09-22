using UnityEngine;
using System.Collections;

public class GUIComponent : MonoBehaviour, IEventListener 
{
    void OnEnable()
    {
        EventManager.Instance.AddEventListener(this);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveEventListener(this);
    }

    public virtual void OnHandleEvent(GameEventType gameEventType, params System.Object[] args)
    {
    }
}