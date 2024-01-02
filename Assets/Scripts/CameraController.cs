using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject moveRightBtn;
    public GameObject moveLeftBtn;
    public float rotSpeed = 3.0f;
    public float minX = 0f;
    public float maxX = 0f;

    private Transform trans;
    private Quaternion startRot;
    private Quaternion targetRot;
    private float rotProgress = 0f;
    private bool isRotating = false;

    void Start()
    {
        trans = transform;
    }

    void Update()
    {
        if (isRotating)
        {
            if (rotProgress >= 1f)
            {
                isRotating = false;
                rotProgress = 1f;
            }
            trans.localRotation = Quaternion.Lerp(startRot, targetRot, rotProgress);
            rotProgress += rotSpeed * Time.deltaTime;
        }
    }

    public void ControlBtnPressed(int id)
    {
        if (isRotating) rotProgress = 1f - rotProgress;
        else rotProgress = 0f;
        startRot = Quaternion.Euler(0f, (id == 1 ? minX : maxX), 0f);
        targetRot = Quaternion.Euler(0f, (id == 1 ? maxX : minX), 0f);
        isRotating = true;
        moveRightBtn.SetActive((id == 0));
        moveLeftBtn.SetActive((id == 1));
    }
}
