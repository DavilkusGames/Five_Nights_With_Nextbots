using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraController cam;
    public TabletCntrl tablet;
    public DoorCntrl[] doors;

    public GameObject adNotificationPanel;
    public TextTranslator adNotificationTxt;

    public float adDelay = 60;

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

    private IEnumerator AdTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(adDelay-5);
            adNotificationPanel.SetActive(true);
            for (int i = 5; i > 0; i--)
            {
                adNotificationTxt.AddAdditionalText(' ' + i.ToString() + "...");
                yield return new WaitForSeconds(1f);
            }
            adNotificationPanel.SetActive(false);
            Pause();
            YandexGames.Instance.ShowAd(Resume);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }
}
