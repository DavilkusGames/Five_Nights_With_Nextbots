using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Plugins.Audio.Core;

public class MainMenuManager : MonoBehaviour
{
    public GameObject continueBtn;
    public GameObject continueDisabledTxt;

    public GameObject customNightBtn;
    public GameObject customNightDisabledTxt;

    public GameObject newGameConfirmPanel;
    public GameObject newspaper;
    public TextTranslator nightIdTxt;
    public TextTranslator scoreTxt;
    public TextTranslator survivedNightsCountTxt;
    public SourceAudio menuMusic;

    public TMP_Text verTxt;
    public GameObject loadingPanel;
    public float newsTime = 3f;

    public static MainMenuManager Instance;

    private void Awake()
    {
        if (Instance != null) DestroyImmediate(gameObject);
        Instance = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        verTxt.text = Application.version;

        menuMusic.Play("menuMusic");

        if (GameData.dataLoaded) DataLoaded(false);
        else if (Application.isEditor) GameData.LoadData();
        else loadingPanel.SetActive(true);
        BlackPanel.Instance.FadeOut(null);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        CancelInvoke();
    }

    public void DataLoaded(bool firstTime)
    {
        loadingPanel.SetActive(false);
        if (firstTime) YandexGames.Instance.GameInitialized();

        continueBtn.SetActive((GameData.data.nightId > 0));
        continueDisabledTxt.SetActive((GameData.data.nightId == 0));

        customNightBtn.SetActive(GameData.data.isCustomNightOpened);
        customNightDisabledTxt.SetActive(!GameData.data.isCustomNightOpened);

        nightIdTxt.AddAdditionalText(' ' + (GameData.data.nightId+1).ToString());
        scoreTxt.AddAdditionalText(' ' + GameData.data.score.ToString());
        survivedNightsCountTxt.AddAdditionalText(' ' + GameData.data.survivedNightsCount.ToString());

        if (GameData.data.prevGameVersion != Application.version.ToString())
        {
            GameData.data.prevGameVersion = Application.version.ToString();
            GameData.SaveData();
        }
    }

    public void Continue()
    {
        GameData.SelectedNightId = GameData.data.nightId;
        BlackPanel.Instance.SetUIBlock(true);
        BlackPanel.Instance.FadeIn(LoadGameScene);
    }

    public void NewGame()
    {
        if (GameData.data.nightId != 0) newGameConfirmPanel.SetActive(true);
        else
        {
            BlackPanel.Instance.SetUIBlock(true);
            BlackPanel.Instance.FadeIn(ShowNewspaper, 3f);
        }
    }

    private void ShowNewspaper()
    {
        newspaper.SetActive(true);
        Invoke(nameof(NewspaperTimer), newsTime);
        BlackPanel.Instance.FadeOut(null);
    }

    private void NewspaperTimer()
    {
        BlackPanel.Instance.FadeIn(LoadGameScene);
    }

    public void ConfirmNewGame()
    {
        newGameConfirmPanel.SetActive(false);
        GameData.SelectedNightId = 0;
        GameData.data.nightId = 0;
        GameData.SaveData();
        BlackPanel.Instance.SetUIBlock(true);
        BlackPanel.Instance.FadeIn(LoadGameScene);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
