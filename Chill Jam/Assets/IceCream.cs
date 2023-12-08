using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class IceCream : MonoBehaviour {
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private BoxCollider2D enemyDetector;
    [SerializeField] private GameObject iceCream, shadow;
    [SerializeField] private Sprite happy, scared, dead;

    public float jumpHeight, jumpDuration, shadowShrinkPercent;

    private Vector3 iceCreamStartPosition, shadowStartScale;

    private Coroutine hopping;
    

    private void Start() {
        StartCoroutine(ScanForEnemies());

        iceCreamStartPosition = iceCream.transform.localPosition;
        shadowStartScale = shadow.transform.localScale;
    }
    public void Hit()
    {
        sr.sprite = dead;

        AudioManager.instance.Lose();

        UIManager.Instance.Lose();
    }

    public void Safe()
    {
        sr.sprite = happy;
    }

    public void Scared()
    {
        if (sr.sprite != dead) sr.sprite = scared;
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

    public void Hop()
    {
        if (hopping != null) return;

        hopping = StartCoroutine(HopAnimation());
    }

    private IEnumerator HopAnimation()
    {
        Safe();

        Vector3 peakPosition = iceCreamStartPosition + new Vector3(0, jumpHeight, 0);

        Vector3 shrunkenShadowScale = shadowStartScale * shadowShrinkPercent;

        // Hop up and down twice
        for (int i = 0; i < 2; i++)
        {
            // Move up
            float time = 0;
            while (time < jumpDuration)
            {
                float lerpFactor = time / jumpDuration;
                iceCream.transform.localPosition = Vector3.Lerp(iceCreamStartPosition, peakPosition, lerpFactor);
                shadow.transform.localScale = Vector3.Lerp(shadowStartScale, shrunkenShadowScale, lerpFactor);

                time += Time.deltaTime;
                yield return null;
            }

            // Move down
            time = 0;
            while (time < jumpDuration)
            {
                float lerpFactor = time / jumpDuration;
                iceCream.transform.localPosition = Vector3.Lerp(peakPosition, iceCreamStartPosition, lerpFactor);
                shadow.transform.localScale = Vector3.Lerp(shrunkenShadowScale, shadowStartScale, lerpFactor);

                time += Time.deltaTime;
                yield return null;
            }
        }

        hopping = null;
    }

}