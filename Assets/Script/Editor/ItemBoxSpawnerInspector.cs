using UnityEngine;
using UnityEditor;
using System.Collections;

[CanEditMultipleObjects]
[CustomEditor(typeof(ItemBoxSpawner))]
public class ItemBoxSpawnerInspector : Editor
{
    private ItemBoxSpawner _target;

    private void OnEnable()
    {
        _target = target as ItemBoxSpawner;
    }

    public override void OnInspectorGUI()
    {
        _target._itemBoxPrefab = EditorGUILayout.ObjectField("ItemBox Prefab", _target._itemBoxPrefab, typeof(GameObject), true) as GameObject;

        _target._spline = EditorGUILayout.ObjectField("Spawn Spline", _target._spline, typeof(CurvySpline), true) as CurvySpline;
        _target._spawnDelay = EditorGUILayout.FloatField("Spawn Delay", _target._spawnDelay);
        if (null != _target._spline)
        {
            _target._spawnDistance = EditorGUILayout.Slider("Spawn Distance", _target._spawnDistance, 0f, _target._spline.Length);            
        }
        else
        {
            _target._spawnDistance = EditorGUILayout.Slider("Spawn Distance", _target._spawnDistance, 0f, 0f);
        }
        
        _target.UpdateLocation();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(_target);
        }
    }
}