using UnityEngine;

public class RecIndicator : MonoBehaviour
{
    public float stateChangeDelay = 1.0f;

    public GameObject obj;
    private float nextStateChangeTime = 0f;
    private bool state = true;
    private bool isEnabled = true;

    void Update()
    {
        if (isEnabled && Time.time >= nextStateChangeTime)
        {
            state = !state;
            obj.SetActive(state);
            nextStateChangeTime = Time.time + stateChangeDelay;
        }
    }

    public void SetState(bool state)
    {
        isEnabled = state;
        if (!isEnabled)
        {
            this.state = false;
            obj.SetActive(false);
        }
    }
}
