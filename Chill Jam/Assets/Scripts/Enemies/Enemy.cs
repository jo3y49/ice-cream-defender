using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;

    private void Start() {
        
    }

    public void Initialize(EnemyData enemyData, int direction)
    {
        sr.sprite = enemyData.sprite;
        rb.velocity = direction * enemyData.speed * Vector2.right;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().Hit();
            Destroy(gameObject);
        }
    }
}