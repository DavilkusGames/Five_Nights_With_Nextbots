using UnityEngine;

public class DustFX : MonoBehaviour
{
    private Transform trans;

    private void Start()
    {
        trans = transform;
    }

    public void TeleportTo(Transform target)
    {
        trans.position = target.position;
        trans.rotation = target.rotation;
    }
}
