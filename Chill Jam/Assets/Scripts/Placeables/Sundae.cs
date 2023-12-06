using UnityEngine;

public class Sundae : Placeable {
    [SerializeField] private BoxCollider2D bombRadius;

    protected override void SetWeapon()
    {
        
    }

    public override void EnemyTouch(Enemy enemy)
    {
        Explode();
    }

    private void Explode()
    {
        Vector2 center = bombRadius.bounds.center;
        Vector2 size = bombRadius.bounds.size;
        LayerMask layer = 1 << 0;
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(center, size, 0, layer);

        foreach (var hitCollider in hitColliders)
        {
            
            if (hitCollider.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.Shot(data.damage);
            }
            
        }

        gridSlot.Destroyed();
    }
}