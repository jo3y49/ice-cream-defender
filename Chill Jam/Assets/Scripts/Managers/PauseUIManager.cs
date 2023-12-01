using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUIManager : MonoBehaviour {
    [SerializeField] private GameObject pauseUI;

    public static int mainMenuSceneIndex = 0;

    private void OnEnable() {
        PauseManager.PauseEvent += ToggleUI;

        pauseUI.SetActive(false);
    }

    private void OnDisable() {
        PauseManager.PauseEvent -= ToggleUI;
    }

    private void ToggleUI(bool b)
    {
        pauseUI.SetActive(b);
    }

    public static void Quit() {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}