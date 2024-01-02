using UnityEngine;
using UnityEngine.UI;

public class WhiteNoise : MonoBehaviour
{
    public Image img;
    public float delay = 0.2f;

    private Transform trans;
    private float nextChangeTime = 0f;
    private int state = 0;

    private void Start()
    {
        trans = transform;
    }

    private void Update()
    {
        if (Time.time >= nextChangeTime)
        {
            trans.localScale = new Vector3((state > 1) ? -1 : 1, (state % 2 == 1) ? 1 : -1, 1f);

            state++;
            if (state > 3)
            {
                state = 0;
            }

            nextChangeTime = Time.time + delay;
        }
    }

    public void SetAlpha(float alpha)
    {
        img.color = new Color(1f, 1f, 1f, alpha);
    }
}
