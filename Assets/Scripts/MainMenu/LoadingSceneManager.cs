using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Plugins.Audio.Core;

public class LoadingSceneManager : MonoBehaviour
{
    public TextTranslator nightTxt;
    public float loadDelay = 4.0f;

    public List<GameObject> staticScanLines;
    public int minScanLineCount = 2;
    public int maxScanLineCount = 4;
    public int scanLineAnimCycles = 3;
    public float scanLineAnimDelay = 0.2f;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        if (GameData.SelectedNightId < 6) nightTxt.AddAdditionalText(' ' + (GameData.SelectedNightId + 1).ToString());
        else nightTxt.SetText(' ' + (YandexGames.IsRus ? "Своя Ночь" : "Custom Night"));

        Invoke(nameof(LoadGameScene), loadDelay);
        StartCoroutine(nameof(ScanLinesAnimation));

        GetComponent<SourceAudio>().Play("camChange");
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(2);
    }

    private IEnumerator ScanLinesAnimation()
    {
        for (int i = 0; i < scanLineAnimCycles; i++)
        {
            yield return new WaitForSeconds(scanLineAnimDelay);
            int lineCount = Random.Range(minScanLineCount, maxScanLineCount);
            List<GameObject> tmpScanLines = new List<GameObject>();
            tmpScanLines.AddRange(staticScanLines);
            for (int j = 0; j < lineCount; j++)
            {
                int scanLineId = Random.Range(0, tmpScanLines.Count);
                tmpScanLines[scanLineId].SetActive(true);
                tmpScanLines.RemoveAt(scanLineId);
            }
            yield return new WaitForSeconds(scanLineAnimDelay/2f);
            foreach (var line in staticScanLines) line.SetActive(false);
        }
    }
}
