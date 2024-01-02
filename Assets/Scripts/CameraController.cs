using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject moveRightBtn;
    public GameObject moveLeftBtn;
    public float rotSpeed = 3.0f;
    public float minX = 0f;
    public float maxX = 0f;

    private Transform trans;
    private float rotVectorY = 0f;

    void Start()
    {
        trans = transform;
    }

    private float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }

    void FixedUpdate()
    {
        if (rotVectorY != 0f)
        {
            trans.Rotate(0f, rotVectorY * Time.fixedDeltaTime, 0f, Space.Self);
            trans.eulerAngles = new Vector3(0f, ClampAngle(trans.eulerAngles.y, minX, maxX), 0f);
            moveLeftBtn.SetActive(trans.eulerAngles.y > minX+0.01f);
            moveRightBtn.SetActive(trans.eulerAngles.y < maxX-0.01f);
        }
    }

    public void SetRightBtnState(bool pressed)
    {
        rotVectorY = ((pressed) ? rotSpeed : 0f);
    }

    public void SetLeftBtnState(bool pressed)
    {
        rotVectorY = ((pressed) ? -rotSpeed : 0f);
    }
}
