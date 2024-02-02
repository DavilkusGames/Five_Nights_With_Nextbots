using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Plugins.Audio.Core;
using System;

public class TabletCntrl : MonoBehaviour
{
    public delegate void CamChangeCallback(int camId);

    public KeyCode tabletKey;
    public GameObject camPanel;
    public GameObject camRender;
    public GameObject[] cams;
    public GameObject[] camRooms;
    public DynamicTextTranslator roomNameTxt;
    public RecIndicator[] officeRecInds;
    public Image[] camIcons;
    public Color selectedCamIconColor;
    public WhiteNoise noise;
    public SourceAudio camAudio;
    public Transform playerT;
    public DustFX dustFX;

    public List<GameObject> staticScanLines;
    public int minScanLineCount = 2;
    public int maxScanLineCount = 4;
    public int scanLineAnimCycles = 3;
    public float scanLineAnimDelay = 0.2f;
    public float disableTime = 4.0f;

    private Animator anim;
    private bool isTabletUp = false;
    private bool isAnimPlaying = false;
    private bool camsDisabled = false;
    private int selectedCamId = 0;
    private Action downCallback = null;
    private CamChangeCallback camChangeCallback = null;

    public static TabletCntrl Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else DestroyImmediate(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        noise.SetAlpha(0.3f);
        camIcons[selectedCamId].color = selectedCamIconColor;
    }

    void Update()
    {
        if (Input.GetKeyDown(tabletKey)) ToggleTablet();
    }

    public void SetDownCallback(Action callback)
    {
        downCallback = callback;
    }

    public void SubscribeToCamChange(CamChangeCallback callback)
    {
        camChangeCallback += callback;
    }

    public void ChangeCam(int id)
    {
        if (!isTabletUp || isAnimPlaying) return;
        cams[selectedCamId].SetActive(false);
        if (camRooms[selectedCamId] != null) camRooms[selectedCamId].SetActive(false);
        camIcons[selectedCamId].color = Color.white;
        selectedCamId = id;
        camIcons[selectedCamId].color = selectedCamIconColor;
        cams[selectedCamId].SetActive(true);
        dustFX.TeleportTo(cams[selectedCamId].transform);
        if (camRooms[selectedCamId] != null) camRooms[selectedCamId].SetActive(true);
        if (camChangeCallback != null) camChangeCallback(selectedCamId);

        roomNameTxt.SetText(selectedCamId);
        camAudio.PlayOneShot("camChange");
        StartCoroutine(nameof(ScanLinesAnimation));
    }

    public void GameOver()
    {
        camAudio.Volume = 0f;
        if (isTabletUp || isAnimPlaying) ToggleTablet();
        this.enabled = false;
    }

    public void DisableCams()
    {
        if (camsDisabled) return;
        noise.SetAlpha(0.6f);
        camRender.SetActive(false);
        StartCoroutine(nameof(ScanLinesAnimation));
        camAudio.Stop();
        camAudio.PlayOneShot("camsDisabled_" + UnityEngine.Random.Range(0, 3).ToString());
        camsDisabled = true;
        Invoke(nameof(EnableCams), disableTime);
    }

    private void EnableCams()
    {
        noise.SetAlpha(0.3f);
        camRender.SetActive(true);
        StartCoroutine(nameof(ScanLinesAnimation));
        camAudio.Stop();
        camsDisabled = false;
    }

    public bool IsTabletUp()
    {
        return isTabletUp;
    }

    public int GetCameraId()
    {
        if (!isTabletUp) return -1;
        return selectedCamId;
    }

    public void ToggleTablet()
    {
        if (isAnimPlaying) return;
        isTabletUp = !isTabletUp;
        if (!isTabletUp) camPanel.SetActive(false);
        if (isTabletUp)
        {
            anim.Play("tabletUp");
            camAudio.Play("tabletUp");
            if (!camsDisabled) camAudio.PlayOneShot("camWorking");
            else camAudio.PlayOneShot("camsDisabled_" + UnityEngine.Random.Range(0, 3).ToString());
            foreach (var ind in officeRecInds) ind.SetState(false);
            dustFX.TeleportTo(cams[selectedCamId].transform);
            if (camChangeCallback != null) camChangeCallback(selectedCamId);
        }
        else
        {
            cams[selectedCamId].SetActive(false);
            if (camRooms[selectedCamId] != null) camRooms[selectedCamId].SetActive(false);
            anim.Play("tabletDown");
            camAudio.Stop();
            camAudio.Play("tabletDown");
            dustFX.TeleportTo(playerT);

            if (downCallback != null)
            {
                downCallback();
                downCallback = null;
            }
            if (camChangeCallback != null) camChangeCallback(-1);
        }
        isAnimPlaying = true;

        if (isTabletUp) EnergyManager.Instance.IncreaseUsage();
        else EnergyManager.Instance.DecreaseUsage();
    }

    public void AnimEnded()
    {
        isAnimPlaying = false;
    }

    public void TabletOpened()
    {
        cams[selectedCamId].SetActive(true);
        if (camRooms[selectedCamId] != null) camRooms[selectedCamId].SetActive(true);
        camPanel.SetActive(true);
        StartCoroutine(nameof(ScanLinesAnimation));
    }

    public void TabletClosed()
    {
        foreach (var ind in officeRecInds) ind.SetState(true);
    }

    private IEnumerator ScanLinesAnimation()
    {
        for (int i = 0; i < scanLineAnimCycles; i++)
        {
            yield return new WaitForSeconds(scanLineAnimDelay);
            int lineCount = UnityEngine.Random.Range(minScanLineCount, maxScanLineCount);
            List<GameObject> tmpScanLines = new List<GameObject>();
            tmpScanLines.AddRange(staticScanLines);
            for (int j = 0; j < lineCount; j++)
            {
                int scanLineId = UnityEngine.Random.Range(0, tmpScanLines.Count);
                tmpScanLines[scanLineId].SetActive(true);
                tmpScanLines.RemoveAt(scanLineId);
            }
            yield return new WaitForSeconds(scanLineAnimDelay / 2f);
            foreach (var line in staticScanLines) line.SetActive(false);
        }
    }
}
