using UnityEngine;

public class GameDataManager : MonoBehaviour {
    public static GameDataManager instance;
    private GameData gameData;
    private GameObject player;

    public void InitializeGameData(GameData data)
    {
        gameData = data;
    }

    public void SaveGame()
    {
        SaveSystem.SaveGameData(gameData);
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public void SetPlayerLevel(int level)
    {
        gameData.playerData.level = level;
    }

    public void SetCurrentScene(int sceneIndex)
    {
        gameData.worldData.currentScene = sceneIndex;
    }

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}