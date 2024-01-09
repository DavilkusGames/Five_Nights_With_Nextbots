using UnityEngine;
using UnityEngine.SceneManagement;
using Plugins.Audio.Core;

public class GameOverManager : MonoBehaviour
{
    public GameObject noise;
    public GameObject gameOverPanel;
    public float noiseTime = 2.0f;

    void Start()
    {
        gameOverPanel.SetActive(false);
        noise.SetActive(true);
        Invoke(nameof(HideNoise), noiseTime);
        GetComponent<SourceAudio>().Play("static");
    }

    private void HideNoise()
    {
        noise.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void RetryRequest()
    {
        if (YandexGames.Instance != null) YandexGames.Instance.ShowAd(Retry);
        else Retry();
    }

    public void ToMainMenuRequest()
    {
        if (YandexGames.Instance != null) YandexGames.Instance.ShowAd(ToMainMenu);
        else ToMainMenu();
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
