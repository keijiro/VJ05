using UnityEngine;
using UnityEditor;

namespace Furball
{
    [CustomEditor(typeof(FurballRenderer))]
    public class FurballEditor : Editor
    {
        SerializedProperty _subdivision;

        void OnEnable()
        {
            _subdivision = serializedObject.FindProperty("_subdivision");
        }

        public override void OnInspectorGUI()
        {
            var furball = (FurballRenderer)target;

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_subdivision);

            if (EditorGUI.EndChangeCheck())
                furball.NotifyConfigChange();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
