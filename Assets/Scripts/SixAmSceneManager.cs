using UnityEngine;
using UnityEngine.SceneManagement;
using Plugins.Audio.Core;
using System.Linq;

public class SixAmSceneManager : MonoBehaviour
{
    public GameObject alarmTxt;
    public float clockTime = 5.0f;
    public float blinkDelay = 0.5f;

    public ScoreTxtAnim scoreTxt;
    public int[] scoreForEachNight;

    private SourceAudio alarmAudio;
    private float nextBlinkTime = 0f;
    private bool isLblActive = true;
    private bool showClock = true;
    private int completedNightId = -1;

    void Update()
    {
        if (showClock && Time.time >= nextBlinkTime)
        {
            isLblActive = !isLblActive;
            if (isLblActive) alarmAudio.PlayOneShot("clockAlarm");
            alarmTxt.SetActive(isLblActive);
            nextBlinkTime = Time.time + blinkDelay;
        }
    }

    void Start()
    {
        if (GameData.data != null)
        {
            completedNightId = GameData.SelectedNightId;

            int scoreInc = 0;
            if (!GameData.IsCustomNight) scoreInc = scoreForEachNight[GameData.data.nightId];
            else scoreInc = GameData.CustomAI.Sum() * 100;
            scoreTxt.Init(this, GameData.data.score, scoreInc);
            GameData.data.score += scoreInc;

            if (GameData.data.nightId < 5) GameData.data.nightId++;
            GameData.data.survivedNightsCount++;
            GameData.SaveData();

            if (YandexGames.Instance != null) YandexGames.Instance.SaveToLeaderboard(GameData.data.score);
        }
        nextBlinkTime = Time.time + blinkDelay;
        alarmAudio = GetComponent<SourceAudio>();
        Invoke(nameof(ClockTimer), clockTime);
    }

    private void ClockTimer()
    {
        BlackPanel.Instance.FadeIn(SwitchToScore, 2f);
    }

    public void SwitchToScore()
    {
        showClock = false;
        isLblActive = false;
        alarmTxt.SetActive(false);

        scoreTxt.StartAnim();
        Invoke(nameof(StartFadeOut), 0.2f);
    }

    private void StartFadeOut()
    {
        BlackPanel.Instance.FadeOut(null, 2f);
    }

    public void ScoreAnimDone()
    {
        BlackPanel.Instance.FadeIn(ExitFromSceneRequest, 2f);
    }

    public void ExitFromSceneRequest()
    {
        if (YandexGames.Instance != null && GameData.data.nightId < 6) YandexGames.Instance.ShowAd(ExitFromScene);
        else ExitFromScene();
    }

    public void ExitFromScene()
    {
        if (completedNightId < 4) SceneManager.LoadScene(0);
        else if (completedNightId == 4) SceneManager.LoadScene(5);
        else if (GameData.data.nightId == 5) SceneManager.LoadScene(7);
    }
}
