using UnityEngine;
using Plugins.Audio.Core;

public class AmbienceManager : MonoBehaviour
{
    private SourceAudio audio;

    public static AmbienceManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else DestroyImmediate(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        audio = GetComponent<SourceAudio>();
        audio.Play("officeAmbience");
    }

    public void DecreaseVolume()
    {
        audio.Volume /= 2f;
    }

    public void Disable()
    {
        audio.Stop();
    }
}
