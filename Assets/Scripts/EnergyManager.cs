using UnityEngine;
using Plugins.Audio.Core;

public class EnergyManager : MonoBehaviour
{
    public TextTranslator energyTxt;
    public GameObject[] usageInds;
    public float[] staticEnergyDecreasePerHour;
    public int powerUsageK = 1;
    public FanCntrl fan;
    public OfficeLight light;
    public TimeManager timeManager;

    public static EnergyManager Instance;

    private SourceAudio poweroffAudio;
    private float energy = 100f;
    private int powerUsage = 1;
    private float staticDecreasePerH = 0f;
    private float nextEnergyDecreaseTime = 0f;

    private void Awake()
    {
        if (Instance != null) DestroyImmediate(gameObject);
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Start()
    {
        poweroffAudio = GetComponent<SourceAudio>();
        staticDecreasePerH = staticEnergyDecreasePerHour[GameData.SelectedNightId];
        nextEnergyDecreaseTime = Time.time + 1f;

        UpdateEnergyTxt();
        UpdateUsageInds();
    }

    void Update()
    {
        if (energy > 0 && Time.time >= nextEnergyDecreaseTime)
        {
            float energyDecreasePerH = staticDecreasePerH + ((powerUsage - 1) * powerUsageK);
            energy -= (energyDecreasePerH / timeManager.nightTimeInSec);
            if (energy < 0) energy = 0;
            UpdateEnergyTxt();
            nextEnergyDecreaseTime = Time.time + 1f;
            if (energy == 0) PowerDown();
        }
    }

    public void PowerDown()
    {
        GameManager.Instance.PowerOff();
        NextbotManager.Instance.LightsOff();
        UIManager.Instance.Poweroff();
        fan.SetState(false);
        light.SetState(false);
        poweroffAudio.Play("powerdown");
    }

    public void IncreaseUsage()
    {
        powerUsage++;
        UpdateUsageInds();
    }

    public void DecreaseUsage()
    {
        powerUsage--;
        UpdateUsageInds();
    }

    private void UpdateEnergyTxt()
    {
        energyTxt.AddAdditionalText(((int)energy).ToString() + '%');
    }

    private void UpdateUsageInds()
    {
        for (int i = 0; i < usageInds.Length; i++)
        {
            usageInds[i].SetActive(i < powerUsage);
        }
    }
}
