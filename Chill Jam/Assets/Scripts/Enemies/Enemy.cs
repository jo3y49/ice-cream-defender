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
        } else 
        {
            sr.flipX = false;
        }

        rb.velocity = speed * Vector2.right;
        
    }
    
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Placeable"))
        {
            other.gameObject.GetComponent<Placeable>().EnemyTouch(this);
        }

        else if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().Hit();
            Death(0);
        }
    }

    public void Shot(int damage)
    {
        health -= damage;
        if (health <= 0) Death(coins);
    }

    public void Kill()
    {
        Death(coins);
    }

    private void Death(int c)
    {
        WorldManager.Instance.DefeatedEnemy(gameObject, c);
    }
}