using UnityEngine;

public class Player : MonoBehaviour {
    public int lives = 5;

    public void Hit()
    {
        lives--;

        if (lives <= 0)
        {
            Destroy(gameObject);
        }
    }
}