using Plugins.Audio.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTxtAnim : MonoBehaviour
{
    public TextTranslator scoreTxt;
    public TextTranslator incScoreTxt;

    public float startDelay = 1f;
    public float endDelay = 1f;
    public float incAnimTime = 1f;
    public int perTickIncrease = 10;

    private int initScore = 0;
    private int incScore = 0;

    private SixAmSceneManager manager;
    private SourceAudio incAudio;

    private void Start()
    {
        incAudio = GetComponent<SourceAudio>();
    }

    public void Init(SixAmSceneManager manager, int initScore, int incScore)
    {
        this.manager = manager;
        this.initScore = initScore;
        this.incScore = incScore;
    }

    public void StartAnim()
    {
        StartCoroutine(nameof(ScoreAnimation));
    }

    private IEnumerator ScoreAnimation()
    {
        scoreTxt.AddAdditionalText(' ' + ScoreToStr(initScore));
        incScoreTxt.AddAdditionalText(' ' + incScore.ToString());

        scoreTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(startDelay/2);
        incScoreTxt.gameObject.SetActive(true);
        yield return new WaitForSeconds(startDelay/2);

        float tickTime = incAnimTime / (incScore / perTickIncrease);
        while (incScore > 0)
        {
            incScore -= perTickIncrease;
            initScore += perTickIncrease;
            scoreTxt.AddAdditionalText(' ' + ScoreToStr(initScore));
            incAudio.PlayOneShot("score");
            yield return new WaitForSeconds(tickTime);
        }
        initScore += -incScore;
        incScore = 0;
        scoreTxt.AddAdditionalText(' ' + ScoreToStr(initScore));

        yield return new WaitForSeconds(endDelay/2);
        incScoreTxt.gameObject.SetActive(false);
        yield return new WaitForSeconds(endDelay/2);
        manager.ScoreAnimDone();
    }

    public static string ScoreToStr(int score)
    {
        return score.ToString("D5");
    }
}
