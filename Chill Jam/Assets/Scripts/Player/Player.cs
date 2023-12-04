using UnityEngine;

public class Player : MonoBehaviour {
    public void Hit()
    {
        GameDataManager.Instance.LoseLife();

        if (GameDataManager.Instance.GetLives() <= 0)
        {
            Destroy(gameObject);
        }
    }
}