using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour {
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D box;
    [SerializeField] private Transform healthBar;
    public int health, maxHealth = 1;
    public float speed = 1;
    public int coins = 1;
    public int damage = 1;

    private Vector3 healthBarScale, healthBarPosition;

    private void Awake() {
        healthBarScale = healthBar.localScale;
        healthBarPosition = new Vector3(0, healthBar.localPosition.y, 0);
    }

    public void Initialize(EnemyData enemyData, Transform spawn)
    {
        
        sr.sprite = enemyData.sprite;
        health = maxHealth = enemyData.health;
        healthBar.localScale = healthBarScale;
        healthBar.localPosition = new Vector3(0, healthBarPosition.y, 0);
        speed = enemyData.speed;
        coins = enemyData.coins;
        damage = enemyData.damage;

        box.size = 1.6f * enemyData.size * Vector2.one;

        if (enemyData.size == 2)
        {
            healthBar.position += Vector3.up * 1.1f;
        }

        if (spawn.position.x > 0) 
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
            Placeable p = other.gameObject.GetComponent<Placeable>();
            StartCoroutine(WaitToMove(p));
            p.EnemyTouch(this);
        }

        else if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponentInParent<IceCream>().Hit();
        }
    }

    public void Shot(int damage)
    {
        health -= damage;
        if (health <= 0) Death();

        else
        {
            healthBar.localScale = new Vector3(healthBarScale.x * health / maxHealth, healthBarScale.y,1);
            // healthBar.position = new Vector3(transform.position.x - ((health / (float)maxHealth) * healthBarScale.x), healthBar.position.y,0);
        } 
    }

    private IEnumerator WaitToMove(Placeable placeable)
    {
        rb.velocity = Vector2.zero;

        float t = 0;
        while (t < 1)
        {
            if (!placeable.gameObject.activeSelf)
            {
                rb.velocity = speed * Vector2.right;
                yield break;
            }
                

            t += Time.deltaTime;
            yield return null;
        }

        placeable.EnemyTouch(this);
        StartCoroutine(WaitToMove(placeable));
    }

    public void Kill()
    {
        Death();
    }

    private void Death()
    {
        WorldManager.Instance.DefeatedEnemy(gameObject, coins);
    }
}