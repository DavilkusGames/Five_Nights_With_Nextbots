using UnityEngine;
using UnityEngine.SceneManagement;
using Plugins.Audio.Core;

public class GameOverManager : MonoBehaviour
{
    public static int killerId = -1;

    public GameObject noise;
    public GameObject gameOverPanel;
    public TextTranslator tipTxt;
    public string[] rusTips;
    public string[] engTips;

    public float noiseTime = 2.0f;

    void Start()
    {
        gameOverPanel.SetActive(false);
        noise.SetActive(true);
        Invoke(nameof(HideNoise), noiseTime);
        GetComponent<SourceAudio>().Play("static");

        if (killerId < 0) tipTxt.gameObject.SetActive(false);
        else
        {
            if (YandexGames.Instance == null) tipTxt.AddAdditionalText("<br><br>" + rusTips[killerId]);
            else tipTxt.AddAdditionalText("<br><br>" + (YandexGames.IsRus ? rusTips[killerId] : engTips[killerId]));
        }
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
