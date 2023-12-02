using UnityEngine;

public class Player : MonoBehaviour {
    public int lives = 5;

    public void Hit()
    {
        lives--;
    }
}