using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FurballRenderer))]
public class FurballRendererEditor : Editor
{
    SerializedProperty _subdivision;
    SerializedProperty _noiseFrequency;
    SerializedProperty _noiseAmplitude;
    SerializedProperty _noisePowerScale;
    SerializedProperty _noiseSpeed;
    SerializedProperty _material;

    void OnEnable()
    {
        _subdivision = serializedObject.FindProperty("_subdivision");
        _noiseFrequency = serializedObject.FindProperty("_noiseFrequency");
        _noiseAmplitude = serializedObject.FindProperty("_noiseAmplitude");
        _noisePowerScale = serializedObject.FindProperty("_noisePowerScale");
        _noiseSpeed = serializedObject.FindProperty("_noiseSpeed");
        _material = serializedObject.FindProperty("_material");
    }

    public override void OnInspectorGUI()
    {
        var instance = (FurballRenderer)target;

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(_subdivision);
        EditorGUILayout.PropertyField(_noiseFrequency);
        EditorGUILayout.PropertyField(_noiseAmplitude);
        EditorGUILayout.PropertyField(_noisePowerScale);
        EditorGUILayout.PropertyField(_noiseSpeed);

        if (EditorGUI.EndChangeCheck())
            instance.NotifyConfigChange();

        EditorGUILayout.PropertyField(_material);

        serializedObject.ApplyModifiedProperties();
    }
}
