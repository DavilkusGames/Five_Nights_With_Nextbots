using UnityEngine;
using UnityEngine.UI;

public class BlackPanel : MonoBehaviour
{
    public delegate void ActionCallback();

    public float fadeSpeed = 1.0f;
    private Image img;
    private float alpha = 0f;
    private float targetAlpha = 0f;
    private ActionCallback callback = null;
    private float currentFadeSpeed = 1f;

    public static BlackPanel Instance;

    private void Awake()
    {
        if (Instance != null) DestroyImmediate(gameObject);
        Instance = this;
        img = GetComponent<Image>();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Start()
    {
        currentFadeSpeed = fadeSpeed;
    }

    void Update()
    {
        if (alpha != targetAlpha)
        {
            alpha = Mathf.Lerp(alpha, targetAlpha, currentFadeSpeed * Time.deltaTime);
            if (Mathf.Abs(alpha - targetAlpha) <= 0.03f)
            {
                alpha = targetAlpha;
                if (callback != null)
                {
                    callback();
                    callback = null;
                }
            }
            img.color = new Color(0f, 0f, 0f, alpha);
        }
    }

    public void FadeIn(ActionCallback callback, float fadeSpeed = -1f)
    {
        if (fadeSpeed != -1) currentFadeSpeed = fadeSpeed;
        else currentFadeSpeed = this.fadeSpeed;

        alpha = 0f;
        targetAlpha = 1f;
        SetUIBlock(true);
        this.callback = callback;
    }

    public void FadeOut(ActionCallback callback, float fadeSpeed = -1f)
    {
        if (fadeSpeed != -1) currentFadeSpeed = fadeSpeed;
        else currentFadeSpeed = this.fadeSpeed;

        alpha = 1f;
        targetAlpha = 0f;
        SetUIBlock(false);
        this.callback = callback;
    }

    public void SetFadeSpeed(float fadeSpeed)
    {
        this.fadeSpeed = fadeSpeed;
    } 

    public void SetUIBlock(bool isBlocking)
    {
        img.raycastTarget = isBlocking;
    }
}
