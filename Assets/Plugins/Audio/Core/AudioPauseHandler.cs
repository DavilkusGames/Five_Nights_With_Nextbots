using System;
using UnityEngine;

namespace Plugins.Audio.Core
{
    public class AudioPauseHandler : MonoBehaviour
    {
        public static AudioPauseHandler Instance => _instance;
        
        private static AudioPauseHandler _instance;
        
        private bool _isFocused = true;
        private bool _isAds = false;

        public static event Action OnPause;
        public static event Action OnUnpause;

        public static bool IsAudioPause { get; private set; }
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            IsAudioPause = false;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            AppFocusHandle.OnFocus += Focus;
            AppFocusHandle.OnUnfocus += UnFocus;
        }

        private void OnDisable()
        {
            AppFocusHandle.OnFocus -= Focus;
            AppFocusHandle.OnUnfocus -= UnFocus;
        }

        private void Focus()
        {
            if (_isFocused == true)
            {
                return;
            }
            
            _isFocused = true;

            if (_isAds == false)
            {
                Unpause();
            }
        }

        private void UnFocus()
        {
            if (_isFocused == false)
            {
                return;
            }
            
            _isFocused = false;
            Pause();
        }

        public void PauseAudio()
        {
            if (_isAds == true)
            {
                return;
            }

            _isAds = true;
            Pause();
        }

        public void UnpauseAudio()
        {
            if (_isAds == false)
            {
                return;
            }

            _isAds = false;

            if (_isFocused == true)
            {
                Unpause();
            }
        }

        private void Pause()
        {
            if (IsAudioPause == true)
            {
                return;
            }
            
            AudioListener.pause = true;
            WebAudio.Mute(true);

            IsAudioPause = true;
            
            OnPause?.Invoke();
            AudioManagement.Instance.Log("Pause Audio");
        }
        
        private void Unpause()
        {
            if (IsAudioPause == false)
            {
                return;
            }
            
            AudioListener.pause = false;
            WebAudio.Mute(false);
            
            IsAudioPause = false;

            OnUnpause?.Invoke();
            AudioManagement.Instance.Log("Unpause Audio");
        }
    }
}
