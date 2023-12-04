using System.Linq;
using UnityEngine;

public class GameDataManager : MonoBehaviour {
    public static GameDataManager Instance;
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

    public void LoseLife(int lives = 1)
    {
        gameData.playerData.lives -= lives;
    }

    public int GetLives()
    {
        return gameData.playerData.lives;
    }

    public void AddCoins(int coins)
    {
        gameData.playerData.coins += coins;
    }

    public int GetCoins()
    {
        return gameData.playerData.coins;
    }

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            gameData = new GameData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}