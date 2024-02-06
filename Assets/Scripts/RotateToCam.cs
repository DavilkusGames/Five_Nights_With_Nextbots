using UnityEngine;

public class RotateToCam : MonoBehaviour
{
    private Transform target;
    private Transform trans;

    private void Start()
    {
        trans = transform;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        if (target != null) trans.LookAt(trans.position - (target.position - trans.position));
    }
}
