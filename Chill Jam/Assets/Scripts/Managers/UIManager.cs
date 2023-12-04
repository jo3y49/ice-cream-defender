using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI coins, wave;

    private void Update() {
        coins.text = GameDataManager.Instance.GetCoins().ToString();
    }

    public void SetWave(int wave)
    {
        this.wave.text = wave.ToString();
    }
}