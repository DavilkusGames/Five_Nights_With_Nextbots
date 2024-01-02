using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScanLines : MonoBehaviour
{
    public List<GameObject> staticScanLines;
    public RectTransform movingScanLine;
    public RectTransform canvas;
    public Camera cam;

    public float minStaticScanLineDelay = 1.0f;
    public float maxStaticScanLineDelay = 3.0f;
    public float minStaticScanLineShowtime = 0.2f;
    public float maxStaticScanLineShowtime = 0.4f;
    public int minStaticLineCount = 2;
    public int maxStaticLineCount = 4;

    public float movingScanLineSpeed = 1.0f;

    private float nextStaticScanLineShow = 0f;

    private void Start()
    {
        movingScanLine.anchoredPosition = GetBottomEdge();
    }
    
    private void Update()
    {
        movingScanLine.localPosition += Vector3.down * movingScanLineSpeed * Time.deltaTime;
        if (movingScanLine.anchoredPosition.y < GetBottomEdge().y) movingScanLine.anchoredPosition = GetTopEdge();

        if (Time.time >= nextStaticScanLineShow)
        {
            StartCoroutine(nameof(ShowStaticScanLines));
            nextStaticScanLineShow = Time.time + Random.Range(minStaticScanLineDelay, maxStaticScanLineDelay);
        }
    }

    private IEnumerator ShowStaticScanLines()
    {
        int lineCount = Random.Range(minStaticLineCount, maxStaticLineCount+1);
        List<GameObject> tmpScanLines = new List<GameObject>();
        tmpScanLines.AddRange(staticScanLines);
        for (int i = 0; i < lineCount; i++)
        {
            int scanLineId = Random.Range(0, tmpScanLines.Count);
            tmpScanLines[scanLineId].SetActive(true);
            tmpScanLines.RemoveAt(scanLineId);
        }

        yield return new WaitForSeconds(Random.Range(minStaticScanLineShowtime, maxStaticScanLineShowtime));
        foreach (var line in staticScanLines) line.SetActive(false);
    }

    private Vector3 GetTopEdge()
    {
        return canvas.anchoredPosition + new Vector2(0, canvas.rect.yMax);
    }

    private Vector3 GetBottomEdge()
    {
        return canvas.anchoredPosition + new Vector2(0, canvas.rect.yMin);
    }
}
