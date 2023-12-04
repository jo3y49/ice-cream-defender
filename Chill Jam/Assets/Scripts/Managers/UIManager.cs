using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI coins, wave, lives;

    private void Update() {
        coins.text = GameDataManager.Instance.GetCoins().ToString();
        lives.text = GameDataManager.Instance.GetLives().ToString();
    }

    public void SetWave(int wave)
    {
        this.wave.text = wave.ToString();
    }
}