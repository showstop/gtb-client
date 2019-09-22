using UnityEngine;
using System.Collections;

public class UIScreenStretch : MonoBehaviour 
{	
	void Start () 
    {
        UIRoot root = GetComponent<UIRoot>();
        if (null != root)
        {
            root.manualHeight = Screen.height;
            root.minimumHeight = 480;
            root.maximumHeight = 1080;
        }

        Camera childCamera = gameObject.GetComponentInChildren<Camera>();
        if (null != childCamera)
        {
            float perx = 1280.0f / Screen.width;
            float pery = 720.0f / Screen.height;
            float v = (perx > pery) ? perx : pery;

            childCamera.orthographicSize = v;
        }
	}
}
