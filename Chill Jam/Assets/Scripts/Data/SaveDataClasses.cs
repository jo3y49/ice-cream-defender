using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    public int lives = 1;
    public int coins = 150;
}

[System.Serializable]
public class SettingsData
{
    
}

[System.Serializable]
public class GameData
{
    public PlayerData playerData = new();
    public SettingsData settingsData = new();

    public GameData NewGame()
    {
        playerData = new();

        return this;
    }
}