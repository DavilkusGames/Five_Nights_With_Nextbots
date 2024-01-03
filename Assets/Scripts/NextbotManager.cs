using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Plugins.Audio.Core;

[System.Serializable]
public class Nextbot
{
    public string name;
    public GameObject obj;
    public GameObject screamer;
    public float screamerTime;
    public int[] perNightAI;

    private int nodeId = 0;
}

public class NextbotManager : MonoBehaviour
{
    public Nextbot[] nextbots;
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
        for (int i = 0; i < nextbots.Length; i++)
        {
            if (nextbots[i].perNightAI[GameData.SelectedNightId] > 0)
            {
                nextbots[i].obj.SetActive(true);
            }
        }
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
        isEnabled = false;
        CancelInvoke();
    }
}
