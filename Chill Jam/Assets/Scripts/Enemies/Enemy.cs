using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour {
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D box;
    public int health = 1;
    public float speed = 1;
    public int coins = 1;
    public int damage = 1;

    public void Initialize(EnemyData enemyData, bool moveRight)
    {
        
        sr.sprite = enemyData.sprite;
        health = enemyData.health;
        speed = enemyData.speed;
        coins = enemyData.coins;
        damage = enemyData.damage;

        box.size = 1.6f * enemyData.size * Vector2.one;

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
            Placeable p = other.gameObject.GetComponent<Placeable>();
            p.EnemyTouch(this);
            StartCoroutine(WaitToMove(p));

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
        Death(coins);
    }

    private void Death(int c)
    {
        WorldManager.Instance.DefeatedEnemy(gameObject, c);
    }
}