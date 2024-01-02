using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] gameplayUI;
    public GameObject[] camBtns;
    public GameObject screamerPanel;

    public static UIManager Instance;
    private bool isGameplayUIEnabled = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else DestroyImmediate(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void Poweroff()
    {
        foreach (var uiElement in gameplayUI) uiElement.SetActive(false);
        isGameplayUIEnabled = false;
    }

    public void GameOver()
    {
        if (isGameplayUIEnabled) Poweroff();
        foreach (var btn in camBtns) btn.SetActive(false);
        screamerPanel.SetActive(true);
    }
}
