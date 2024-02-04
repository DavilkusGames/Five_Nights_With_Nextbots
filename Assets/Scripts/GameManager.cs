using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraController cam;
    public TabletCntrl tablet;
    public DoorCntrl[] doors;
    public TimeManager gameTime;

    public GameObject adNotificationPanel;
    public TextTranslator adNotificationTxt;

    public GameObject skipNightBtn;

    public float adDelay = 60;
    public float skipNightBtnTime = 10f;

    public static GameManager Instance;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (Instance == null) Instance = this;
        else DestroyImmediate(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        BlackPanel.Instance.FadeOut(null);
        StartCoroutine(nameof(AdTimer));

        if (GameData.SelectedNightId == 0)
        {
            skipNightBtn.SetActive(true);
            Invoke(nameof(SkipNightRequest), skipNightBtnTime);
        }
    }

    public void GameOver()
    {
        cam.enabled = false;
        tablet.GameOver();
        for (int i = 0; i < doors.Length; i++) doors[i].GameOver();
        AmbienceManager.Instance.DecreaseVolume();
    }

    public void PowerOff()
    {
        tablet.GameOver();
        for (int i = 0; i < doors.Length; i++) doors[i].Poweroff();
        AmbienceManager.Instance.Disable();
    }

    private void SkipNightBtnTimeout()
    {
        skipNightBtn.SetActive(false);
    }

    public void SkipNightRequest()
    {
        gameTime.SkipNight();
    }

    private IEnumerator AdTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(adDelay-3);
            Pause();
            adNotificationPanel.SetActive(true);
            for (int i = 3; i > 0; i--)
            {
                adNotificationTxt.AddAdditionalText(' ' + i.ToString() + "...");
                yield return new WaitForSecondsRealtime(1f);
            }
            adNotificationPanel.SetActive(false);
            if (YandexGames.Instance != null) YandexGames.Instance.ShowAd(Resume);
            else Resume();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        AudioListener.volume = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        AudioListener.volume = 1f;
    }
}
