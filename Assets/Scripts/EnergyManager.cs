using UnityEngine;
using Plugins.Audio.Core;

public class EnergyManager : MonoBehaviour
{
    public TextTranslator energyTxt;
    public GameObject[] usageInds;
    public int[] energyDecreaseTimePerNight;
    public int powerUsageK = 1;
    public FanCntrl fan;
    public OfficeLight light;

    public static EnergyManager Instance;

    private SourceAudio poweroffAudio;
    private int energy = 1000;
    private int powerUsage = 1;
    private float energyDecreaseTime = 0f;
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
        energyDecreaseTime = energyDecreaseTimePerNight[GameData.SelectedNightId];
        nextEnergyDecreaseTime = Time.time + energyDecreaseTime;

        UpdateEnergyTxt();
        UpdateUsageInds();
    }

    void Update()
    {
        if (energy > 0 && Time.time >= nextEnergyDecreaseTime)
        {
            energy -= 1;
            energy -= (powerUsage-1) * powerUsageK;
            if (energy < 0) energy = 0;
            UpdateEnergyTxt();
            nextEnergyDecreaseTime = Time.time + energyDecreaseTime;
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
        energyTxt.AddAdditionalText((energy/10).ToString() + '%');
    }

    private void UpdateUsageInds()
    {
        for (int i = 0; i < usageInds.Length; i++)
        {
            usageInds[i].SetActive(i < powerUsage);
        }
    }
}
