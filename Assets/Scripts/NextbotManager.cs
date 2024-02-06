using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Plugins.Audio.Core;
using System;

public class NextbotManager : MonoBehaviour
{
    public NextbotCntrl[] nextbots;
    public SanicCntrl sanic;
    public DoorCntrl[] doors;
    public static NextbotManager Instance;

    private SourceAudio screamerAudio;
    private bool isEnabled = true;

    private bool isLightsOff = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else DestroyImmediate(gameObject);
    }

    private void Start()
    {
        screamerAudio = GetComponent<SourceAudio>();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void NextbotEnteredDoor(int doorId, int nextbotId)
    {
        doors[doorId].NextbotEntered(nextbots[nextbotId]);
    }
    
    public bool IsPlayerWatching(int camId)
    {
        if (camId == -1) return false;
        return (TabletCntrl.Instance.IsTabletUp() && TabletCntrl.Instance.GetCameraId() == camId);
    }
    
    public bool IsDoorClosed(int doorId)
    {
        return doors[doorId].GetDoorState();
    }

    public bool IsLightOff(int doorId)
    {
        return doors[doorId].GetLightState();
    }

    public void WaitForLightBlink(int doorId, Action callback)
    {
        doors[doorId].SetLightBlinkCallback(callback);
    }

    public bool CanEnterDoor(int doorId)
    {
        return !(doors[doorId].IsOccupied() || (doors[doorId].GetLightState() && !TabletCntrl.Instance.IsTabletUp()));
    }

    public void NextbotLeftDoor(int doorId)
    {
        doors[doorId].NextbotLeft();
    }

    public void BreakDoor(int doorId)
    {
        doors[doorId].Break();
    }

    public void TabletDownCallback(Action callback)
    {
        TabletCntrl.Instance.SetDownCallback(callback);
    }

    public void LightsOff()
    {
        isLightsOff = true;
        Invoke(nameof(LightsOffScreamer), UnityEngine.Random.Range(10.0f, 15.0f));
    }

    private void LightsOffScreamer()
    {
        Screamer(0, true);
    }

    public void Screamer(int id, bool isPowerDown=false)
    {
        Disable();
        nextbots[id].screamer.SetActive(true);
        screamerAudio.Play("screamer_" + id.ToString());
        UIManager.Instance.GameOver();
        if (!isLightsOff) GameManager.Instance.GameOver();
        Invoke(nameof(ScreamerEnd), nextbots[id].screamerTime);
        if (isPowerDown) GameOverManager.killerId = 4;
        else GameOverManager.killerId = id;
    }

    public void ScreamerEnd()
    {
        SceneManager.LoadScene(4);
    }

    public void Disable()
    {
        foreach (var nextbot in nextbots) nextbot.Disable();
        sanic.Disable();
    }
}
