using UnityEngine;

public class RecIndicator : MonoBehaviour
{
    public float stateChangeDelay = 1.0f;

    public GameObject obj;
    private float nextStateChangeTime = 0f;
    private bool state = true;
    private int statePoints = 0;
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
        if (state) statePoints++;
        else statePoints--;
        isEnabled = (statePoints == 0);
        if (!isEnabled)
        {
            this.state = false;
            obj.SetActive(false);
        }
    }
}
