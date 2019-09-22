using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorMenu : MonoBehaviour 
{
    [MenuItem("GameObject/Create Other/YP/Jump Trigger")]
    static void CreateJumpTrigger()
    {
        JumpTrigger.Create();
    }
}
