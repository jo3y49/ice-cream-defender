using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
    public int damage = 1;

    public void Fire(Vector2 velocity, int damage)
    {
        this.damage = damage;
        rb.velocity = velocity;

        StartCoroutine(WaitToHit());
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Shot(damage);
            Contact();
        }
    }

    private void Contact()
    {
        AudioManager.instance.EnemyHit();

        StopAllCoroutines();
        
        Pool.Instance.ReturnBullet(gameObject);
    }

    private IEnumerator WaitToHit()
    {
        yield return new WaitForSeconds(5);

        Pool.Instance.ReturnBullet(gameObject);
    }
}