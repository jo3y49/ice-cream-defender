using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {
    [SerializeField] private Button loadButton;
    // [SerializeField] private GameObject settingsMenu;
    private GameData gameData;
    
    [SerializeField] private int introSceneIndex = 1;

    private void Awake() 
    {
        
    }

    public void StartGame()
    {
        EnterGame(gameData.NewGame(), introSceneIndex);
    }

    private void EnterGame(GameData gameData, int sceneIndex)
    {
        // GameDataManager.Instance.InitializeGameData(gameData);

        SceneManager.LoadScene(sceneIndex);
    }

    public void Settings()
    {
        // settingsMenu.SetActive(true);
        // gameObject.SetActive(false);
    }

    public void QuitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // public void UpdateSettings(SettingsData settingsData)
    // {
    //     gameData.settingsData = settingsData;
    //     SaveSystem.SaveGameData(gameData);
    // }
}