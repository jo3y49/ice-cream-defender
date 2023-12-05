using UnityEngine;

public class Player : MonoBehaviour {
    public void Hit()
    {
        GameDataManager.Instance.LoseLife();

        UIManager.Instance.SetLives();

        if (GameDataManager.Instance.GetLives() <= 0)
        {   
            Destroy(gameObject);
        }
    }
}