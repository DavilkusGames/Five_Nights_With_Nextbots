using UnityEngine;

public class CCTVCamCntrl : MonoBehaviour
{
    public bool isRotating = true;
    public float minRot = -90f;
    public float maxRot = 90f;
    public float rotSpeed = 4.0f;
    public float rotDelay = 1f;
    public int rotDir = -1;

    private Transform trans;
    private float rot = 0f;

    void Start()
    {
        trans = transform;
        rot = trans.localRotation.x;
    }

    void FixedUpdate()
    {
        if (isRotating)
        {
            trans.Rotate(0f, rotSpeed * rotDir * Time.fixedDeltaTime, 0f, Space.World);
            //Debug.Log(trans.eulerAngles.y);
            if (rotDir == -1 && trans.eulerAngles.y < minRot + 0.01f)
            {
                rotDir = 1;
                isRotating = false;
                Invoke(nameof(RotateDelayTimer), rotDelay);
            }

            if (rotDir == 1 && trans.eulerAngles.y > maxRot - 0.01f)
            {
                rotDir = -1;
                isRotating = false;
                Invoke(nameof(RotateDelayTimer), rotDelay);
            }
        }
    }

    private void RotateDelayTimer()
    {
        isRotating = true;
    }
}
