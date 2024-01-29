using UnityEngine;

public class ButtonCntrl : MonoBehaviour
{
    public DoorCntrl door;
    public bool isDoorBtn = true;
    public bool hold = false;

    void OnMouseDown()
    {
        if (isDoorBtn) door.ToggleDoor(false);
        else door.ToggleLight();
    }

    private void OnMouseUp()
    {
        if (!hold) return;
        if (isDoorBtn) door.ToggleDoor(false);
        else door.ToggleLight();
    }
}
