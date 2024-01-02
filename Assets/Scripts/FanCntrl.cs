using UnityEngine;

public class FanCntrl : MonoBehaviour
{
    public Transform fanRotatePart;
    public Vector3 rotateVector;

    private bool isOn = true;

    void FixedUpdate()
    {
        if (isOn) fanRotatePart.Rotate(rotateVector * Time.fixedDeltaTime);
    }

    public void SetState(bool isOn)
    {
        this.isOn = isOn;
    } 
}
