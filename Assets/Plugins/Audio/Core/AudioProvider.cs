using System;

namespace Plugins.Audio.Core
{
    public abstract class AudioProvider : IDisposable
    {
        public abstract float Volume { get; set; }
        public abstract bool Mute { get; set; }
        public abstract bool Loop { get; set; }
        public abstract float Pitch { get; set; }
        public abstract float Time { get; set; }
        public abstract bool IsPlaying { get; }

        public abstract void Play(string key);
        public abstract void PlayOneShot(string key);
        public abstract void Stop();

        public virtual void OnAudioUnpaused(){}
        public virtual void OnAudioPaused(){}
        
        public virtual void Update(){}

        public virtual void Dispose(){}

        public virtual void RefreshSettings(SourceAudio.AudioSettings settings)
        {
            Volume = settings.volume;
            Loop = settings.loop;
            Pitch = settings.pitch;
            Mute = settings.mute;
        }
    }
}