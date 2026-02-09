#if VFX_OUTPUTEVENT_AUDIO
using UnityEngine;
using UnityEngine.VFX.Utility;
using UnityEditor;

namespace UnityEditor.VFX.Utility
{
    [CustomEditor(typeof(VFXOutputEventPlayAudio))]
    class VFXOutputEventPlayAudioEditor : VFXOutputEventHandlerEditor
    {
        SerializedProperty m_AudioSource;
        SerializedProperty m_FmodEvent;
        SerializedProperty m_UseFMOD;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_AudioSource = serializedObject.FindProperty(nameof(VFXOutputEventPlayAudio.audioSource));
            m_FmodEvent = serializedObject.FindProperty(nameof(VFXOutputEventPlayAudio.fmodEvent));
            m_UseFMOD = serializedObject.FindProperty(nameof(VFXOutputEventPlayAudio.useFMOD));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            DrawOutputEventProperties();

            // Draw FMOD toggle + fields
            EditorGUILayout.PropertyField(m_UseFMOD, new GUIContent("Use FMOD"));
            if (m_UseFMOD.boolValue)
            {
                EditorGUILayout.PropertyField(m_FmodEvent, new GUIContent("FMOD Event"));
            }
            else
            {
                EditorGUILayout.PropertyField(m_AudioSource);
            }

            HelpBox("Attribute Usage", "VFX Attributes are not used for this Output Event Handler");

            if (EditorGUI.EndChangeCheck())
            {
                // Prefab-check only matters for UnityEngine.Object fields (audio source)
                var newAudioSource = m_AudioSource.objectReferenceValue;
                if (newAudioSource != null
                    && PrefabUtility.GetPrefabAssetType(newAudioSource) != PrefabAssetType.NotAPrefab
                    && PrefabUtility.GetPrefabInstanceStatus(newAudioSource) != PrefabInstanceStatus.Connected)
                {
                    m_AudioSource.objectReferenceValue = null;
                }

                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif
