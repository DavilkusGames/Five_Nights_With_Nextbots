using UnityEngine;

public class BlinkingTV : MonoBehaviour
{
    public Light light;
    public Material[] mats;
    public MeshRenderer screenRenderer;

    private float nextStateChangeTime = 0f;
    private bool state = false;

    private void Update()
    {
        if (Time.time > nextStateChangeTime)
        {
            state = !state;
            light.enabled = state;
            screenRenderer.material = mats[state ? 1 : 0];
            nextStateChangeTime = Time.time + Random.Range(0.1f, 0.3f) * (!state ? 1.5f : 1f);
        }
    }
}
