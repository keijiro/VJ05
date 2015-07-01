// Image Sequence Output Utility
// http://github.com/keijiro/SequenceOut
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ImageSequenceOut))]
public class ImageSequenceOutEditor : Editor
{
    SerializedProperty _frameRate;
    SerializedProperty _superSampling;
    SerializedProperty _recordOnStart;

    void OnEnable()
    {
        _frameRate     = serializedObject.FindProperty("_frameRate");
        _superSampling = serializedObject.FindProperty("_superSampling");
        _recordOnStart = serializedObject.FindProperty("_recordOnStart");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_frameRate);
        EditorGUILayout.PropertyField(_superSampling);

        if (Application.isPlaying)
        {
            var iso = (ImageSequenceOut)target;
            var buttonStyle = GUILayout.Height(30);
            if (iso.isRecording)
            {
                var time = (float)iso.frameCount / iso.frameRate;
                var label = "STOP  (" + time.ToString("0.0") + "s)";
                if (GUILayout.Button(label, buttonStyle)) iso.StopRecording();
                EditorUtility.SetDirty(target); // force repaint
            }
            else
            {
                if (GUILayout.Button("REC", buttonStyle)) iso.StartRecording();
            }
        }
        else
        {
            EditorGUILayout.PropertyField(_recordOnStart);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
