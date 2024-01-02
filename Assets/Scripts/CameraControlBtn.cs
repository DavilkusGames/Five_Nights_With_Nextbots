using UnityEngine;

public class CameraControlBtn : MonoBehaviour
{
    public CameraController cam;
    public int id = 0;

    public void ButtonHover()
    {
        if (!YandexGames.IsMobile) cam.ControlBtnPressed(id);
    }

    public void ButtonPressed()
    {
        if (YandexGames.IsMobile) cam.ControlBtnPressed(id);
    }
}
