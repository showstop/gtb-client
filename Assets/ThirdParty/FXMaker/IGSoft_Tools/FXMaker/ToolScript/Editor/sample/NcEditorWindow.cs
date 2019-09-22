// Attribute ------------------------------------------------------------------------
// Property -------------------------------------------------------------------------
// Loop Function --------------------------------------------------------------------
// Control Function -----------------------------------------------------------------
// Event Function -------------------------------------------------------------------
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;

// [CustomEditor(typeof(NcAnimationCurve))]

public class NcEditorWindow : EditorWindow
{
	// ---------------------------------------------------------------------
	public static EditorWindow Init()
	{
		EditorWindow window = GetWindow(typeof(NcEditorWindow));

		window.minSize	= new Vector2(300, 500);
		window.Show();
		return window;
	}

	void OnEnable()
    {
		YPLog.Log("OnEnable");
    }

    void OnDisable()
    {
		YPLog.Log("OnDisable");
    }

	void OnDestroy()
	{
		YPLog.Log("OnDestroy");
	}

	void OnInspectorUpdate()
	{
		YPLog.Log("OnInspectorUpdate");
	}

	void OnHierarchyChange()
	{
		YPLog.Log("OnHierarchyChange");
	}

	void OnProjectChange()
	{
		YPLog.Log("OnProjectChange");
	}

	void OnSelectionChange()
	{
		YPLog.Log("OnSelectionChange");
	}

	void OnFocus()
	{
		YPLog.Log("OnFocus");
	}

	void OnLostFocus()
	{
		YPLog.Log("OnLostFocus");
	}

	// ----------------------------------------------------------------------------------
}
