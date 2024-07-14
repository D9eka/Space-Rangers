using System;
using UnityEditor;
using static Lazers.LazerGun;

namespace Lazers
{
    [CustomEditor(typeof(LazerGun))]
    public class LazerGunEditor : UnityEditor.Editor
    {
        private SerializedProperty _lazerPrefabProperty;
        private SerializedProperty _soundsProperty;

        private SerializedProperty _typeProperty;
        private SerializedProperty _delayProperty;
        private SerializedProperty _lazerVelocityProperty;

        private void OnEnable()
        {
            _lazerPrefabProperty = serializedObject.FindProperty("_lazerPrefab");
            _soundsProperty = serializedObject.FindProperty("_sounds");

            _typeProperty = serializedObject.FindProperty("_type");
            _delayProperty = serializedObject.FindProperty("_delay");
            _lazerVelocityProperty = serializedObject.FindProperty("_lazerVelocity");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_lazerPrefabProperty);
            EditorGUILayout.PropertyField(_soundsProperty);

            EditorGUILayout.PropertyField(_typeProperty);
            LazerGunType lazerType = (LazerGunType)_typeProperty.intValue;
            switch (lazerType)
            {
                case LazerGunType.Primary:
                    EditorGUILayout.PropertyField(_delayProperty);
                    EditorGUILayout.PropertyField(_lazerVelocityProperty);
                    break;
                case LazerGunType.Additional:
                    EditorGUILayout.PropertyField(_delayProperty);
                    EditorGUILayout.PropertyField(_lazerVelocityProperty);
                    break;
                case LazerGunType.Enemy:
                    break;
                default:
                    throw new NotImplementedException();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}