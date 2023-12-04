using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    private EnemyData enemyData;
    public int health = 1;
    public float speed = 1;
    public int coins = 1;

    private void Start() {
        
    }

    public void Initialize(EnemyData enemyData, bool moveRight)
    {
        
        sr.sprite = enemyData.sprite;
        health = enemyData.health;
        speed = enemyData.speed;
        coins = enemyData.coins;
        
        if (!moveRight) 
        {
            sr.flipX = true;
            speed = -speed;
        }

        rb.velocity = speed * Vector2.right;
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();

            health -= bullet.damage;
            bullet.Contact();

            if (health <= 0) Death(coins);
        }

        else if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().Hit();
            Death(0);
        }
    }

    private void Death(int c)
    {
        WorldManager.Instance.DefeatedEnemy(gameObject, c);
    }
}