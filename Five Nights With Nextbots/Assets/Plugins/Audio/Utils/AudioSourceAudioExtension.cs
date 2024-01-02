using Plugins.Audio.Core;
using UnityEngine;

namespace Plugins.Audio.Utils
{
    public static class AudioSourceAudioExtension
    {
        public static void Play(this SourceAudio sourceAudio, AudioDataProperty audioDataProperty)
        {
            sourceAudio.Play(audioDataProperty.Key);
        }
        
        public static void PlayOneShot(this SourceAudio sourceAudio, AudioDataProperty audioDataProperty)
        {
            sourceAudio.PlayOneShot(audioDataProperty.Key);
        }

        public static void SetData(this AudioSource audioSource, SourceAudio.AudioSettings settings)
        {
            audioSource.outputAudioMixerGroup = settings.mixerGroup;

            audioSource.bypassEffects = settings.bypassEffects;
            audioSource.bypassListenerEffects = settings.bypassListenerEffects;
            audioSource.bypassReverbZones = settings.bypassReverbZones;
            
            audioSource.panStereo = settings.stereoPan;
            audioSource.spatialBlend = settings.spatialBlend;
            audioSource.reverbZoneMix = settings.reverbZoneMix;
            
            audioSource.dopplerLevel = settings.dopplerLevel;
            audioSource.spread = settings.spread;
            audioSource.rolloffMode = settings.volumeRolloff;
            audioSource.minDistance = settings.minDistance;
            audioSource.maxDistance = settings.maxDistance;
        }
    }
}