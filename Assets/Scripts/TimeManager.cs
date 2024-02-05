using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class TimeManager : MonoBehaviour
{
    [Serializable]
    public class AIIncrease
    {
        public int incTime = -1;
        public NextbotCntrl nextbot;
    }

    public TMP_Text timeTxt;
    public TextTranslator nightIdTxt;
    public float nightTimeInSec = 10f;
    public AIIncrease[] aiIncreases;

    private int time = 0;

    void Start()
    {
        nightIdTxt.AddAdditionalText(' ' + (GameData.SelectedNightId+1).ToString());
        if (YandexGames.Instance != null && YandexGames.IsRus) timeTxt.text = "00:00";
        StartCoroutine(nameof(NightTimer));
    }

    private void Update()
    {
        if (Application.isEditor && Input.GetKeyDown(KeyCode.N)) SkipNight();
    }

    private IEnumerator NightTimer()
    {
        while (time < 6)
        {
            yield return new WaitForSeconds(nightTimeInSec);
            time++;

            foreach (var inc in aiIncreases)
            {
                if (inc.incTime == time) inc.nextbot.IncreaseAI();
            }

            if (YandexGames.Instance == null || !YandexGames.IsRus) timeTxt.text = time.ToString() + " AM";
            else timeTxt.text = ToTwoDigits(time.ToString()) + ":00";
        }
        SkipNight();
    }

    private string ToTwoDigits(string str)
    {
        if (str.Length >= 2) return str;
        else return '0' + str;
    }

    public void SkipNight()
    {
        BlackPanel.Instance.SetUIBlock(true);
        BlackPanel.Instance.SetFadeSpeed(10f);
        BlackPanel.Instance.FadeIn(LoadSixAmScene);
    }

    public void LoadSixAmScene()
    {
        NextbotManager.Instance.Disable();
        SceneManager.LoadScene(3);
    }
}
