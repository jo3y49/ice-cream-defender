using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class IceCream : MonoBehaviour {
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private BoxCollider2D enemyDetector;
    [SerializeField] private Sprite happy, scared, dead, shadow;
    

    private void Start() {
        StartCoroutine(ScanForEnemies());
    }
    public void Hit()
    {
        sr.sprite = dead;

        AudioManager.instance.Lose();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Safe()
    {
        sr.sprite = happy;
    }

    public void Scared()
    {
        sr.sprite = scared;
    }

    private IEnumerator ScanForEnemies()
    {
        Vector2 center = enemyDetector.bounds.center;
        Vector2 size = enemyDetector.bounds.size;
        LayerMask layer = 1 << LayerMask.NameToLayer("Enemy");

        while (true)
        {
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(center, size, 0, layer);

            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    Scared();
                    goto EndScan;
                }
            }

            Safe();

            EndScan:

            yield return new WaitForSeconds(.5f);
        }
    }
}