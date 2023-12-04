using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour {
    public int damage = 1;

    public void Contact()
    {
        Destroy(gameObject);
    }
}