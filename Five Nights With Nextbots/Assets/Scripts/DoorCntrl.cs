using UnityEngine;
using Plugins.Audio.Core;

public class DoorCntrl : MonoBehaviour
{
    public KeyCode toggleDoorKey;
    public KeyCode toggleLightKey;
    public MeshRenderer doorBtn;
    public MeshRenderer lightBtn;
    public Material[] doorBtnMats;
    public Material[] lightBtnMats;
    public GameObject light;
    public GameObject blackImitation;
    public SourceAudio lightAudio;

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

    void Start()
    {
        anim = GetComponent<Animator>();
        doorAudio = GetComponent<SourceAudio>();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleDoorKey)) ToggleDoor(false);
        if (Input.GetKeyDown(toggleLightKey)) ToggleLight();

        if (isLightOn && Time.time >= nextLightBlinkTime)
        {
            isLightBlinked = !isLightBlinked;
            light.SetActive(!isLightBlinked);
            lightAudio.Volume = ((isLightBlinked) ? 0f : 1f);
            if (!isLightBlinked) nextLightBlinkTime = Time.time + Random.Range(minLightBlinkDelay, maxLightBlinkDelay);
            else nextLightBlinkTime = Time.time + Random.Range(minLightBlinkTime, maxLightBlinkTime);
        }
    }

    public void Poweroff()
    {
        if (isClosed) ToggleDoor(true);
        if (isLightOn) ToggleLight();
        this.enabled = false;
    }

    public void GameOver()
    {
        if (isLightOn) ToggleLight();
        this.enabled = false;
    }

    public void ToggleDoor(bool isSilent)
    {
        if (isAnimationPlaying || !this.enabled) return;
        isClosed = !isClosed;
        doorBtn.material = doorBtnMats[((isClosed) ? 1 : 0)];
        if (isClosed) anim.Play("doorClose");
        else anim.Play("doorOpen");
        if (!isSilent) doorAudio.PlayOneShot("door");
        isAnimationPlaying = true;

        if (isClosed) EnergyManager.Instance.IncreaseUsage();
        else EnergyManager.Instance.DecreaseUsage();
    }

    public void ToggleLight()
    {
        if (!this.enabled) return;
        isLightOn = !isLightOn;
        lightBtn.material = lightBtnMats[((isLightOn) ? 1 : 0)];
        light.SetActive(isLightOn);
        blackImitation.SetActive(!isLightOn);
        lightAudio.Volume = 1f;
        if (isLightOn)
        {
            lightAudio.Play("lights");
            nextLightBlinkTime = Time.time + Random.Range(minLightBlinkDelay, maxLightBlinkDelay);
        }

        if (isLightOn) EnergyManager.Instance.IncreaseUsage();
        else EnergyManager.Instance.DecreaseUsage();
    }

    public void AnimEnded()
    {
        isAnimationPlaying = false;
    }
}
