using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraController cam;
    public TabletCntrl tablet;
    public DoorCntrl[] doors;

    public static GameManager Instance;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (Instance == null) Instance = this;
        else DestroyImmediate(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        BlackPanel.Instance.FadeOut(null);
    }

    public void GameOver()
    {
        cam.enabled = false;
        tablet.GameOver();
        for (int i = 0; i < doors.Length; i++) doors[i].GameOver();
        AmbienceManager.Instance.DecreaseVolume();
    }

    public void PowerOff()
    {
        tablet.GameOver();
        for (int i = 0; i < doors.Length; i++) doors[i].Poweroff();
        AmbienceManager.Instance.Disable();
    }
}
