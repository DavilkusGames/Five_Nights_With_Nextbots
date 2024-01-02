using UnityEngine;

public class LoadingIcon : MonoBehaviour
{
    public float rotSpeed = 1.0f;
    private Transform trans;

    void Start()
    {
        trans = transform;
    }

    void Update()
    {
        trans.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
    }
}
