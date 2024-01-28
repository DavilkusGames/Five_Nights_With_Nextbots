using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TMP_Text timeTxt;
    public TextTranslator nightIdTxt;
    public float nightTimeInSec = 10f;

    private int time = 0;

    void Start()
    {
        nightIdTxt.AddAdditionalText(' ' + (GameData.SelectedNightId+1).ToString());
        if (YandexGames.Instance != null && YandexGames.IsRus) timeTxt.text = "00:00";
        StartCoroutine(nameof(NightTimer));
    }

    private IEnumerator NightTimer()
    {
        while (time < 6)
        {
            yield return new WaitForSeconds(nightTimeInSec);
            time++;

            if (YandexGames.Instance == null || !YandexGames.IsRus) timeTxt.text = time.ToString() + " AM";
            else timeTxt.text = ToTwoDigits(time.ToString()) + ":00";
        }
        BlackPanel.Instance.SetUIBlock(true);
        BlackPanel.Instance.SetFadeSpeed(10f);
        BlackPanel.Instance.FadeIn(LoadSixAmScene);
    }

    private string ToTwoDigits(string str)
    {
        if (str.Length >= 2) return str;
        else return '0' + str;
    }

    public void LoadSixAmScene()
    {
        NextbotManager.Instance.Disable();
        SceneManager.LoadScene(3);
    }
}
