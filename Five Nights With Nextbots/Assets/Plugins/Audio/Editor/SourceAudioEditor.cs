using System;
using System.Reflection;
using Plugins.Audio.Core;
using UnityEditor;
using UnityEngine;

namespace Plugins.Audio.Editor
{
    [CustomEditor(typeof(SourceAudio))]
    public class SourceAudioEditor : UnityEditor.Editor
    {
        private bool _foldOut = false;
        
        private AudioSource _audioSource;
        private UnityEditor.Editor _audioEditor;
        
        private void OnEnable()
        {
            if (_audioSource != null)
            {
                return;
            }
            
            GameObject audioSourceGO = new GameObject("source");
            audioSourceGO.hideFlags = HideFlags.HideInHierarchy;
            
            _audioSource = audioSourceGO.AddComponent<AudioSource>();
            
            
            _audioEditor = CreateEditor(_audioSource);
            
            
        }

        private void OnDisable()
        {
            DestroyImmediate(_audioEditor);

            if (_audioSource != null)
            {
                DestroyImmediate(_audioSource.gameObject);
            }
        }
        
        public override void OnInspectorGUI()
        {
            SerializedProperty providerTypeProp = serializedObject.FindProperty("_provider");
            AudioProviderType providerType =
                Enum.Parse<AudioProviderType>(providerTypeProp.enumNames[providerTypeProp.enumValueIndex]);
            
            SerializedProperty volumeProp = serializedObject.FindProperty("_volume");
            SerializedProperty loopProp = serializedObject.FindProperty("_loop");
            SerializedProperty muteProp = serializedObject.FindProperty("_mute");
            SerializedProperty pitchProp = serializedObject.FindProperty("_pitch");
            SerializedProperty mixerGroupProp = serializedObject.FindProperty("_output");
            
            SerializedProperty spatialBlend = serializedObject.FindProperty("_spatialBlend");
            SerializedProperty reverbZoneMix = serializedObject.FindProperty("_reverbZoneMix");
            SerializedProperty bypassEffectsProp = serializedObject.FindProperty("_bypassEffects");
            SerializedProperty bypassListenerEffectsProp = serializedObject.FindProperty("_bypassListenerEffects");
            SerializedProperty bypassReverbZonesProp = serializedObject.FindProperty("_bypassReverbZones");
            SerializedProperty stereoPanProp = serializedObject.FindProperty("_stereoPan");

            SerializedProperty dopplerLevelProp = serializedObject.FindProperty("_dopplerLevel");
            SerializedProperty spreadProp = serializedObject.FindProperty("_spread");
            SerializedProperty minDistanceProp = serializedObject.FindProperty("_minDistance");
            SerializedProperty maxDistanceProp = serializedObject.FindProperty("_maxDistance");
            SerializedProperty volumeRolloffProp = serializedObject.FindProperty("_volumeRolloff");

            serializedObject.Update();
            
            EditorGUILayout.PropertyField(providerTypeProp);
            EditorGUILayout.Slider(volumeProp, 0, 1, Styles.volumeLabel);
            EditorGUILayout.PropertyField(loopProp);
            EditorGUILayout.PropertyField(muteProp);
            
            EditorGUILayout.Slider(pitchProp, -3f, 3, Styles.pitchLabel);
            
            if (providerType == AudioProviderType.JS)
            {
                EditorGUILayout.HelpBox("Pitch is not supported in JS Provider", MessageType.Info);
            }
            
            if (providerType == AudioProviderType.Unity)
            {
                EditorGUILayout.PropertyField(mixerGroupProp, Styles.outputMixerGroupLabel);
                
                EditorGUILayout.PropertyField(bypassEffectsProp);
                EditorGUILayout.PropertyField(bypassListenerEffectsProp);
                EditorGUILayout.PropertyField(bypassReverbZonesProp);
                
                EditorGUILayout.Slider(stereoPanProp, -1, 1, Styles.panStereoLabel);
                DrawSlider(Styles.panLeftLabel, Styles.panRightLabel);
                
                EditorGUILayout.Slider(spatialBlend, 0, 1, Styles.spatialBlendLabel);
                DrawSlider(Styles.spatialLeftLabel, Styles.spatialRightLabel);

                EditorGUILayout.Slider(reverbZoneMix, 0, 1.1f, Styles.reverbZoneMixLabel);
                EditorGUILayout.Space();
                
                _foldOut = EditorGUILayout.Foldout(_foldOut, "3D Sound Settings", true);
                if (_foldOut)
                {
                    EditorGUI.indentLevel++;

                    _audioSource.dopplerLevel = dopplerLevelProp.floatValue;
                    _audioSource.spread = spreadProp.floatValue;
                    _audioSource.rolloffMode = (AudioRolloffMode)volumeRolloffProp.enumValueIndex;
                    _audioSource.minDistance = minDistanceProp.floatValue;
                    _audioSource.maxDistance = maxDistanceProp.floatValue;
                    
                    _audioEditor.serializedObject.Update();
                    
                    MethodInfo methodDrawAudio3D = _audioEditor.GetType().GetMethod("Audio3DGUI", BindingFlags.NonPublic | BindingFlags.Instance);
                    methodDrawAudio3D.Invoke(_audioEditor, null);

                    _audioEditor.serializedObject.ApplyModifiedProperties();
                    
                    EditorGUI.indentLevel--;
                    
                    dopplerLevelProp.floatValue = _audioSource.dopplerLevel;
                    spreadProp.floatValue = _audioSource.spread;
                    volumeRolloffProp.enumValueIndex = (int)_audioSource.rolloffMode;
                    minDistanceProp.floatValue = _audioSource.minDistance;
                    maxDistanceProp.floatValue = _audioSource.maxDistance;
                }
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSlider(GUIContent a, GUIContent b)
        {
            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);

            Rect rect = GUILayoutUtility.GetLastRect();
            rect.x += EditorGUIUtility.labelWidth;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.LabelField(rect, a, b);
            
            EditorGUI.EndDisabledGroup();
        }

        internal static class Styles
        {
          public static GUIStyle labelStyle = (GUIStyle) "ProfilerBadge";
          public static GUIContent rolloffLabel = EditorGUIUtility.TrTextContent("Volume Rolloff", "Which type of rolloff curve to use");
          public static string controlledByCurveLabel = "Controlled by curve";
          public static GUIContent audioClipLabel = EditorGUIUtility.TrTextContent("AudioClip", "The AudioClip asset played by the AudioSource. Can be undefined if the AudioSource is generating a live stream of audio via OnAudioFilterRead.");
          public static GUIContent panStereoLabel = EditorGUIUtility.TrTextContent("Stereo Pan", "Only valid for Mono and Stereo AudioClips. Mono sounds will be panned at constant power left and right. Stereo sounds will have each left/right value faded up and down according to the specified pan value.");
          public static GUIContent spatialBlendLabel = EditorGUIUtility.TrTextContent("Spatial Blend", "Sets how much this AudioSource is treated as a 3D source. 3D sources are affected by spatial position and spread. If 3D Pan Level is 0, all spatial attenuation is ignored.");
          public static GUIContent reverbZoneMixLabel = EditorGUIUtility.TrTextContent("Reverb Zone Mix", "Sets how much of the signal this AudioSource is mixing into the global reverb associated with the zones. [0, 1] is a linear range (like volume) while [1, 1.1] lets you boost the reverb mix by 10 dB.");
          public static GUIContent dopplerLevelLabel = EditorGUIUtility.TrTextContent("Doppler Level", "Specifies how much the pitch is changed based on the relative velocity between AudioListener and AudioSource.");
          public static GUIContent spreadLabel = EditorGUIUtility.TrTextContent("Spread", "Sets the spread of a 3d sound in speaker space");
          public static GUIContent outputMixerGroupLabel = EditorGUIUtility.TrTextContent("Output", "Set whether the sound should play through an Audio Mixer first or directly to the Audio Listener");
          public static GUIContent volumeLabel = EditorGUIUtility.TrTextContent("Volume", "Sets the overall volume of the sound.");
          public static GUIContent pitchLabel = EditorGUIUtility.TrTextContent("Pitch", "Sets the frequency of the sound. Use this to slow down or speed up the sound.");
          public static GUIContent priorityLabel = EditorGUIUtility.TrTextContent("Priority", "Sets the priority of the source. Note that a sound with a larger priority value will more likely be stolen by sounds with smaller priority values.");
          public static GUIContent spatializeLabel = EditorGUIUtility.TrTextContent("Spatialize", "Enables or disables custom spatialization for the AudioSource.");
          public static GUIContent spatializePostEffectsLabel = EditorGUIUtility.TrTextContent("Spatialize Post Effects", "Determines if the custom spatializer is applied before or after the effect filters attached to the AudioSource. This flag only has an effect if the spatialize flag is enabled on the AudioSource.");
          public static GUIContent priorityLeftLabel = EditorGUIUtility.TrTextContent("High");
          public static GUIContent priorityRightLabel = EditorGUIUtility.TrTextContent("Low");
          public static GUIContent spatialLeftLabel = EditorGUIUtility.TrTextContent("2D");
          public static GUIContent spatialRightLabel = EditorGUIUtility.TrTextContent("3D");
          public static GUIContent panLeftLabel = EditorGUIUtility.TrTextContent("Left");
          public static GUIContent panRightLabel = EditorGUIUtility.TrTextContent("Right");
          public static string xAxisLabel = L10n.Tr("Distance");
        }
    }
}