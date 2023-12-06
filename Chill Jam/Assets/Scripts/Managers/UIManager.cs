using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    [SerializeField] private TextMeshProUGUI coins, wave, lives;
    [SerializeField] private GameObject itemMenu, cancel;
    private ItemOptions itemOptions;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        itemMenu.TryGetComponent(out itemOptions);
        itemMenu.SetActive(false);
        cancel.SetActive(false);
        
        SetLives();
    }

    private void Update() {
        coins.text = GameDataManager.Instance.GetCoins().ToString();
    }

    public void SetLives()
    {
        lives.text = GameDataManager.Instance.GetLives().ToString();
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
        cancel.SetActive(b);
    }

    public void Cancel()
    {
        MouseObject.Instance.Undo();
    }
}