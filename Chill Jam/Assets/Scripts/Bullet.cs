using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
    public int damage = 1;

    public void Fire(Vector2 velocity)
    {
        rb.velocity = velocity;
    }

    public void Contact()
    {
        Pool.Instance.ReturnBullet(gameObject);
    }
}