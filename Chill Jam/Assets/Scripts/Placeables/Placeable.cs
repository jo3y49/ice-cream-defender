using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Placeable : MonoBehaviour {
    [SerializeField] protected SpriteRenderer sr;
    public PlaceableData data;
    public int health;
    protected GridSlot gridSlot;

    public virtual void SetUp(GridSlot gridSlot)
    {
        this.gridSlot = gridSlot;
    }

    public virtual void Initialize(PlaceableData data)
    {
        this.data = data;
        sr.sprite = data.sprite;
        health = data.health;
        transform.position = gridSlot.transform.position;

        SetWeapon();
    }

    public virtual void Upgrade()
    {
        if (data.upgrade != null)
        {
            GameDataManager.Instance.AddCoins(-data.upgrade.price);
            Initialize(data.upgrade);
            // gridSlot.SetItem(data.upgrade);
        }
            
    }

    protected abstract void SetWeapon();

    public virtual void EnemyTouch(Enemy enemy)
    {
        health -= enemy.damage;

        enemy.Kill();

        if (health <= 0)
            gridSlot.Destroyed();
    }
}