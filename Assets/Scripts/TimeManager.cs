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
        StartCoroutine(nameof(NightTimer));
    }

    private IEnumerator NightTimer()
    {
        while (time < 6)
        {
            yield return new WaitForSeconds(nightTimeInSec);
            time++;

            timeTxt.text = time.ToString() + " AM";
        }
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
