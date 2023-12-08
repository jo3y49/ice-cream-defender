using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    [SerializeField] private TextMeshProUGUI coins, wave;
    [SerializeField] private GameObject itemMenu, cancel, loseUI;
    [SerializeField] private Button pauseButton;
    private ItemOptions itemOptions;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        itemMenu.TryGetComponent(out itemOptions);
        itemMenu.SetActive(false);
        cancel.SetActive(false);
        loseUI.SetActive(false);
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
        itemMenu.SetActive(false);
        pauseButton.interactable = false;
        PauseManager.TogglePause();

        loseUI.SetActive(true);
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
}