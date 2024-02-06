using UnityEngine;
using Plugins.Audio.Core;
using System;

public class DoorCntrl : MonoBehaviour
{
    public KeyCode toggleDoorKey;
    public KeyCode toggleLightKey;
    public MeshRenderer doorBtn;
    public MeshRenderer lightBtn;
    public Material[] doorBtnMats;
    public Material[] lightBtnMats;
    public RecIndicator officeRecIndBehindDoor;
    public GameObject light;
    public GameObject blackImitation;
    public SourceAudio lightAudio;
    public SourceAudio doorScreamerAudio;

    public float minLightBlinkDelay = 0.3f;
    public float maxLightBlinkDelay = 0.7f;
    public float minLightBlinkTime = 0.3f;
    public float maxLightBlinkTime = 0.7f;
    private float nextLightBlinkTime = 0f;

    private Animator anim;
    private SourceAudio doorAudio;
    private bool isClosed = false;
    private bool isLightOn = false;
    private bool isLightBlinked = false;
    private bool isAnimationPlaying = false;
    private bool isBroken = false;

    private NextbotCntrl nextbotInDoorway;
    private Action lightBlinkCallback;
    private bool isNextbotUnspotted = false;
    private bool isOccupied = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        doorAudio = GetComponent<SourceAudio>();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleDoorKey)) ToggleDoor(false);
        if (Input.GetKeyDown(toggleLightKey)) ToggleLight(true);
        if (Input.GetKeyUp(toggleLightKey)) ToggleLight(false);

        if (isLightOn && Time.time >= nextLightBlinkTime)
        {
            isLightBlinked = !isLightBlinked;
            light.SetActive(!isLightBlinked);
            if (nextbotInDoorway != null) nextbotInDoorway.obj.SetActive(!isLightBlinked);
            if (isLightBlinked && lightBlinkCallback != null)
            {
                lightBlinkCallback();
                lightBlinkCallback = null;
            }
            lightAudio.Volume = ((isLightBlinked) ? 0f : 1f);
            if (!isLightBlinked) nextLightBlinkTime = Time.time + UnityEngine.Random.Range(minLightBlinkDelay, maxLightBlinkDelay);
            else nextLightBlinkTime = Time.time + UnityEngine.Random.Range(minLightBlinkTime, maxLightBlinkTime);
        }
    }

    public bool GetDoorState() { return isClosed; }
    public bool GetLightState() { return isLightOn; }
    public bool IsOccupied() { return isOccupied; }

    public void Break()
    {
        isBroken = true;
    }

    public void SetLightBlinkCallback(Action callback)
    {
        if (!isLightOn) callback();
        else lightBlinkCallback = callback;
    }

    public void Poweroff()
    {
        if (isClosed) ToggleDoor(true);
        if (isLightOn) ToggleLight(false);
        this.enabled = false;
        officeRecIndBehindDoor.SetState(false);
    }

    public void GameOver()
    {
        if (isLightOn) ToggleLight(false);
        this.enabled = false;
        officeRecIndBehindDoor.SetState(false);
    }

    public void NextbotEntered(NextbotCntrl nextbot)
    {
        if (nextbot != null)
        {
            nextbotInDoorway = nextbot;
            nextbot.obj.SetActive(isLightOn && !isLightBlinked);
            isNextbotUnspotted = true;
        }
        isOccupied = true;
    }

    public void NextbotLeft()
    {
        nextbotInDoorway = null;
        isOccupied = false;
    }

    public void ToggleDoor(bool isSilent)
    {
        if (isAnimationPlaying || !this.enabled) return;
        if (isBroken)
        {
            doorAudio.Play("doorError");
            return;
        }

        isClosed = !isClosed;
        doorBtn.material = doorBtnMats[((isClosed) ? 1 : 0)];
        if (isClosed) anim.Play("doorClose");
        else anim.Play("doorOpen");
        if (!isSilent) doorAudio.PlayOneShot("door");
        isAnimationPlaying = true;
        officeRecIndBehindDoor.SetState(!isClosed);

        if (isClosed) EnergyManager.Instance.IncreaseUsage();
        else EnergyManager.Instance.DecreaseUsage();
    }

    public void ToggleLight(bool state)
    {
        if (!this.enabled) return;
        if (isBroken)
        {
            if (state) doorAudio.Play("doorError");
            return;
        }

        if (isLightOn != state) isLightOn = state;
        else return;

        lightBtn.material = lightBtnMats[((isLightOn) ? 1 : 0)];
        light.SetActive(isLightOn);
        blackImitation.SetActive(!isLightOn);
        lightAudio.Volume = 1f;
        if (isLightOn)
        {
            lightAudio.Play("lights");
            nextLightBlinkTime = Time.time + UnityEngine.Random.Range(minLightBlinkDelay, maxLightBlinkDelay);
            if (isNextbotUnspotted)
            {
                doorScreamerAudio.Play("doorScreamer");
                isNextbotUnspotted = false;
            }
        }
        if (nextbotInDoorway != null) nextbotInDoorway.obj.SetActive(isLightOn);

        if (isLightOn) EnergyManager.Instance.IncreaseUsage();
        else EnergyManager.Instance.DecreaseUsage();
    }

    public void AnimEnded()
    {
        isAnimationPlaying = false;
    }
}
