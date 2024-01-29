using UnityEngine;

public class ButtonCntrl : MonoBehaviour
{
    public DoorCntrl door;
    public bool isDoorBtn = true;

    void OnMouseDown()
    {
        if (isDoorBtn) door.ToggleDoor(false);
        else door.ToggleLight(true);
    }

    private void OnMouseUp()
    {
        if (isDoorBtn) return;
        door.ToggleLight(false);
    }
}
