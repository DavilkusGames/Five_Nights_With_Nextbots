using System;
using UnityEngine;

namespace Plugins.Audio.Core
{
    public class JsAudioProvider : AudioProvider
    {
        public override float Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = Mathf.Clamp(value, 0, 1);
                
                WebAudio.SetSourceVolume(_id, _volume);
            }
        }

        public override bool Loop
        {
            get
            {
                return _loop;
            }
            set
            {
                _loop = value;
                
                WebAudio.SetSourceLoop(_id, _loop);
            }
        }

        public override bool Mute
        {
            get
            {
                return _mute;
            }
            set
            {
                _mute = value;
                
                WebAudio.SetSourceMute(_id, _mute);
            }
        }

        public override float Pitch
        {
            get
            {
                AudioManagement.Instance.Log("JS Provider not support Pitch");
                return 1;
            }
            set
            {
                AudioManagement.Instance.Log("JS Provider not support Pitch");
            }
        }

        public override float Time
        {
            get => WebAudio.GetSourceTime(_id);
            set => WebAudio.SetSourceTime(_id, value);
        }

        private bool _mute;
        private float _volume;
        private bool _loop;

        private readonly string _id;
        
        private readonly SourceAudio _sourceAudio;

        public JsAudioProvider(SourceAudio sourceAudio)
        {
            _sourceAudio = sourceAudio;
            _id = Guid.NewGuid().ToString();
        }

        public override void Dispose()
        {
            WebAudio.DeleteSource(_id);
        }

        public override bool IsPlaying => WebAudio.IsPlayingAudioSource(_id);

        public override void Play(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                AudioManagement.Instance.LogError("key is empty, Source Audio PlaySound: " + _sourceAudio.name);
                return;
            }
            
            string clipPath = AudioManagement.Instance.GetClipPath(key);
                
            WebAudio.PlayAudio(_id, clipPath, _loop, _volume, _mute);

            AudioManagement.Instance.Log("Start play audio: " + key);
        }

        public override void PlayOneShot(string key)
        {
            AudioManagement.Instance.Log("JS Provider not support play one shot");
        }

        public override void Stop()
        {
            WebAudio.StopSource(_id);
        }
    }
}