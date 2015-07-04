using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShellRenderer))]
public class ShellRendererEditor : Editor
{
    SerializedProperty _subdivision;
    SerializedProperty _material;

    void OnEnable()
    {
        _subdivision = serializedObject.FindProperty("_subdivision");
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

        EditorGUILayout.PropertyField(_material);

        serializedObject.ApplyModifiedProperties();
    }
}
