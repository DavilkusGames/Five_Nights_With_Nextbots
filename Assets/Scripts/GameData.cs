using UnityEngine;

public class GameData
{
    public static GameData data = null;
    public static bool dataLoaded = false;
    public static int SelectedNightId = 0;

    // SAVE DATA
    public int nightId = 0;
    public int survivedNightsCount = 0;
    public int score = 0;
    public bool isCustomNightOpened = false;
    // =======================

    public static void LoadData()
    {
        if (!YandexGames.Instance.LoadData())
        {
            LoadLocalData();
        }
    }

    private static bool LoadLocalData()
    {
        if (!PlayerPrefs.HasKey("GAME_DATA"))
        {
            data = new GameData();
            dataLoaded = true;
            SaveData();
            Debug.Log("Local saved data not found. New save created.");
            MainMenuManager.Instance.DataLoaded(true);
            return false;
        }
        else
        {
            data = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString("GAME_DATA"));

            bool dataSaved = false;
            if (data == null)
            {
                Debug.Log("LOCAL DATA CORRUPTED. Resaving new data...");
                data = new GameData();
                SaveData();
                dataSaved = true;
            }
            else Debug.Log("LOCAL data loaded.");
            dataLoaded = true;
            MainMenuManager.Instance.DataLoaded(true);
            return !dataSaved;
        }
    }

    public static void CloudDataLoaded(string dataStr)
    {
        if (dataStr.Length > 3)
        {
            data = JsonUtility.FromJson<GameData>(dataStr);
            if (data == null)
            {
                Debug.Log("Cloud data corrupted. Loading local data...");

                if (LoadLocalData()) SaveData();
            }
            else
            {
                Debug.Log("CLOUD data loaded.");
                dataLoaded = true;
                MainMenuManager.Instance.DataLoaded(true);
            }
        }
        else
        {
            Debug.Log("Cloud data empty. Loading local data...");
            if (LoadLocalData()) SaveData();
        }
    }

    public static void SaveData()
    {
        string dataStr = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("GAME_DATA", dataStr);
        Debug.Log("Local data saved");
        YandexGames.Instance.SaveData(dataStr);
    }

    public static void Reset()
    {
        data = new GameData();
        SaveData();
        dataLoaded = false;
        Debug.Log("Game data reset");
    }
}
