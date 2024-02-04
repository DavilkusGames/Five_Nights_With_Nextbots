using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FifthNightOverManager : MonoBehaviour
{ 
    public float showTime = 5f;

    private void Start()
    {
        BlackPanel.Instance.FadeOut(null);
        Invoke(nameof(StartExiting), showTime);
    }

    private void StartExiting()
    {
        BlackPanel.Instance.FadeIn(ExitFromSceneRequest);
    }

    public void ExitFromSceneRequest()
    {
        if (YandexGames.Instance != null) YandexGames.Instance.ShowAd(ExitFromScene);
        else ExitFromScene();
    }

    public void ExitFromScene()
    {
        SceneManager.LoadScene(0);
    }
}
