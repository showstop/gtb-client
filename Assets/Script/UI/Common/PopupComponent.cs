using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupComponent : GUIComponent
{   
    private bool IsOpen { get { return gameObject.activeInHierarchy; } }

    [SerializeField]
    protected Animator _animator;

    private const string Animator_Param_Show = "Show";

    internal void Open()
    {
        if (IsOpen)
        {
            return;
        }
        
        gameObject.SetActive(true);
        if (null != _animator)
        {
            _animator.SetBool(Animator_Param_Show, true);
        }
    }

    public void Close()
    {
        if (!IsOpen)
        {
            return;
        }

        StartCoroutine(CloseAfterHide());        
    }

    private IEnumerator CloseAfterHide()
    {
        if (null != _animator)
        {
            _animator.SetBool(Animator_Param_Show, false);
            yield return new WaitForSeconds(0.8f);
        }

        gameObject.SetActive(false);
    }
}