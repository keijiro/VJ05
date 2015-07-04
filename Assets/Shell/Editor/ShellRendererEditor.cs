using UnityEngine;
using UnityEditor;

/*
[CustomEditor(typeof(ShellRenderer))]
public class ShellRendererEditor : Editor
{
    SerializedProperty _subdivision;
    SerializedProperty _speed;
    SerializedProperty _cutoff;
    SerializedProperty _material;

    void OnEnable()
    {
        _subdivision = serializedObject.FindProperty("_subdivision");
        _speed = serializedObject.FindProperty("_speed");
        _cutoff = serializedObject.FindProperty("_cutoff");
        _material = serializedObject.FindProperty("_material");
    }

    public override void OnInspectorGUI()
    {
        var instance = (ShellRenderer)target;

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(_subdivision);

        if (EditorGUI.EndChangeCheck())
            instance.NotifyConfigChange();

        EditorGUILayout.PropertyField(_speed);
        EditorGUILayout.PropertyField(_cutoff);
        EditorGUILayout.PropertyField(_material);

        serializedObject.ApplyModifiedProperties();
    }
}
*/
