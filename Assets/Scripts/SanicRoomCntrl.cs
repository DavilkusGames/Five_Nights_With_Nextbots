using Plugins.Audio.Core;
using UnityEngine;

public class SanicRoomCntrl : MonoBehaviour
{
    public GameObject alarmLight;
    public MeshRenderer alarmRenderer;
    public Material[] alarmMats;
    public SourceAudio alarmAudio;
    public int camId = 0;

    private void Start()
    {
        TabletCntrl.Instance.SubscribeToCamChange(CamChanged);
    }

    public void CamChanged(int camId)
    {
        alarmAudio.Mute = !(camId == this.camId);
    }

    public void AlarmState(bool state)
    {
        alarmLight.SetActive(state);
        alarmRenderer.material = alarmMats[state ? 1 : 0];
        if (state) alarmAudio.Play("alarm");
        else alarmAudio.Stop();
    }
}
