using UnityEngine;
using UnityEngine.UI;

public class RecIndicator : MonoBehaviour
{
    public float stateChangeDelay = 1.0f;

    private Image img;
    private float nextStateChangeTime = 0f;
    private bool state = true;

    void Start()
    {
        img = GetComponent<Image>();
    }

    void Update()
    {
        if (Time.time >= nextStateChangeTime)
        {
            state = !state;
            img.enabled = state;
            nextStateChangeTime = Time.time + stateChangeDelay;
        }
    }
}
