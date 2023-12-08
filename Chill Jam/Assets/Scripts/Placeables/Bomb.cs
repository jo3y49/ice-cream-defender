using System.Collections;
using UnityEngine;

public class Bomb : Placeable {
    [SerializeField] private BoxCollider2D bombRadius;
    private bool exploding = false;

    protected override void SetWeapon()
    {
        exploding = false;
    }

    public override void EnemyTouch(Enemy enemy)
    {
        Explode();
    }

    private void Explode()
    {
        if (exploding) return;

        Vector2 center = bombRadius.bounds.center;
        Vector2 size = bombRadius.bounds.size;
        LayerMask layer = 1 << LayerMask.NameToLayer("Enemy");
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(center, size, 0, layer);

        foreach (var hitCollider in hitColliders)
        {
            
            if (hitCollider.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.Shot(data.damage);
            }
            
        }

        StartCoroutine(Explosion());
    }

    private IEnumerator Explosion()
    {
        exploding = true;

        sr.sprite = (data as BombData).explosionSprite;

        yield return new WaitForSeconds(.3f);

        gridSlot.Destroyed();
    }
}