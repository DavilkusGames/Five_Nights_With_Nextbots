using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Plugins.Audio.Core;

public class NextbotManager : MonoBehaviour
{
    public NextbotCntrl[] nextbots;
    public static NextbotManager Instance;

    private SourceAudio screamerAudio;
    private bool isEnabled = true;

    private bool isLightsOff = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else DestroyImmediate(gameObject);
    }

    private void Start()
    {
        screamerAudio = GetComponent<SourceAudio>();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void LightsOff()
    {
        isLightsOff = true;
        Invoke(nameof(LightsOffScreamer), Random.Range(4.0f, 8.0f));
    }

    private void LightsOffScreamer()
    {
        Screamer(0);
    }

    public void Screamer(int id)
    {
        Disable();
        nextbots[id].screamer.SetActive(true);
        screamerAudio.Play("screamer_" + id.ToString());
        UIManager.Instance.GameOver();
        if (!isLightsOff) GameManager.Instance.GameOver();
        Invoke(nameof(ScreamerEnd), nextbots[id].screamerTime);
    }

    public void ScreamerEnd()
    {
        SceneManager.LoadScene(4);
    }

    public void Disable()
    {
        foreach (var nextbot in nextbots) nextbot.Disable();
    }
}
