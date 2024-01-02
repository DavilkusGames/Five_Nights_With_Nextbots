using UnityEngine;
using UnityEngine.SceneManagement;
using Plugins.Audio.Core;

public class SixAmSceneManager : MonoBehaviour
{
    public GameObject alarmTxt;
    public float sceneTime = 5.0f;
    public float blinkDelay = 0.5f;

    private SourceAudio alarmAudio;
    private float nextBlinkTime = 0f;
    private bool isLblActive = true;

    void Update()
    {
        if (Time.time >= nextBlinkTime)
        {
            isLblActive = !isLblActive;
            if (isLblActive) alarmAudio.PlayOneShot("clockAlarm");
            alarmTxt.SetActive(isLblActive);
            nextBlinkTime = Time.time + blinkDelay;
        }
    }

    void Start()
    {
        GameData.data.nightId++;
        GameData.data.survivedNightsCount++;
        GameData.SaveData();
        YandexGames.Instance.SaveToLeaderboard(GameData.data.survivedNightsCount);
        nextBlinkTime = Time.time + blinkDelay;
        alarmAudio = GetComponent<SourceAudio>();
        Invoke(nameof(StartExiting), sceneTime);
    }

    private void StartExiting()
    {
        BlackPanel.Instance.FadeIn(ExitFromScene);
    }

    public void ExitFromScene()
    {
        SceneManager.LoadScene(0);
    }
}
