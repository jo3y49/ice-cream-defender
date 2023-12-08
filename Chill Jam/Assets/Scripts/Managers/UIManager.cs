using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    [SerializeField] private TextMeshProUGUI coins, wave, gameOver;
    [SerializeField] private GameObject itemMenu, cancel, gameOverUI;
    [SerializeField] private Button pauseButton;
    private ItemOptions itemOptions;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        itemMenu.TryGetComponent(out itemOptions);
        itemMenu.SetActive(false);
        cancel.SetActive(false);
        gameOverUI.SetActive(false);
    }

    private void Update() {
        coins.text = GameDataManager.Instance.GetCoins().ToString();
    }

    public void SetWave(int wave)
    {
        this.wave.text = wave.ToString();
    }

    public void SetItem(GridSlot gridSlot)
    {
        itemOptions.SetData(gridSlot);
    }

    public void SetCancel(bool b)
    {
        // cancel.SetActive(b);
    }

    public void Cancel()
    {
        MouseObject.Instance.Undo();
    }

    public void Lose()
    {
        gameOver.text = "Oh no! They got the ice cream! Try again?";
        EndGame();
    }

    public void Win()
    {
        gameOver.text = "Congrats! You won!";
        EndGame();
    }

    private void EndGame()
    {
        itemMenu.SetActive(false);
        pauseButton.interactable = false;
        PauseManager.TogglePause();

        gameOverUI.SetActive(true);
    }

    public bool Lossed()
    {
        return gameOverUI.activeSelf;
    }

    public void Retry()
    {
        PauseManager.TogglePause();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        // SceneManager.LoadScene(0);
    }

    public void DebugCoins()
    {
        GameDataManager.Instance.AddCoins(100);
    }
}