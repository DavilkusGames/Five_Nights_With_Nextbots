using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    public Light light;

    private float nextStateChangeTime = 0f;
    private bool state = false;

    private void Update()
    {
        if (Time.time > nextStateChangeTime)
        {
            state = !state;
            light.enabled = state;
            nextStateChangeTime = Time.time + Random.Range(0.1f, 0.3f) * (!state ? 1.5f : 1f);
        }
    }
}
