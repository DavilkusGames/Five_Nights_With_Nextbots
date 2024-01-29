using UnityEngine;

public class DustFX : MonoBehaviour
{
    private ParticleSystem fx;
    private Transform trans;

    private void Start()
    {
        fx = GetComponent<ParticleSystem>();
        trans = transform;
    }

    public void TeleportTo(Transform target)
    {
        trans.position = target.position;
        trans.rotation = target.rotation;
        fx.Stop();
        fx.Play();
    }
}
