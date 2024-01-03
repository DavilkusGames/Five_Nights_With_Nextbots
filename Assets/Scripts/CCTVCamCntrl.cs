using UnityEngine;

public class CCTVCamCntrl : MonoBehaviour
{
    public bool canRotate = true;
    public float minRot = -90f;
    public float maxRot = 90f;
    public float rotSpeed = 4.0f;
    public float rotDelay = 1f;
    public int rotDir = 1;

    private Transform trans;
    private Quaternion startRot;
    private Quaternion targetRot;
    private float rotProgress = 0f;
    private float rotY = 0f;
    private bool isRotating = false;

    void Start()
    {
        trans = transform;
        rotY = transform.eulerAngles.x;
        SwitchDirTimer();
    }

    void FixedUpdate()
    {
        if (canRotate && isRotating)
        {
            if (rotProgress >= 1f)
            {
                isRotating = false;
                rotProgress = 1f;
                rotDir *= -1;
                Invoke(nameof(SwitchDirTimer), rotDelay);
            }
            trans.localRotation = Quaternion.Lerp(startRot, targetRot, rotProgress);
            rotProgress += rotSpeed * Time.deltaTime;
        }
    }

    private void SwitchDirTimer()
    {
        rotProgress = 0f;
        startRot = Quaternion.Euler(rotY, (rotDir == 1 ? minRot : maxRot), 0f);
        targetRot = Quaternion.Euler(rotY, (rotDir == 1 ? maxRot : minRot), 0f);
        isRotating = true;
    }
}
