using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Placeable : MonoBehaviour {
    [SerializeField] protected SpriteRenderer sr;
    protected PlaceableData data;
    protected GridSlot gridSlot;

    public virtual void SetUp(GridSlot gridSlot)
    {
        this.gridSlot = gridSlot;
    }

    public virtual void Initialize(PlaceableData data)
    {
        this.data = data;
        sr.sprite = data.sprite;
        transform.position = gridSlot.transform.position;

        SetWeapon();
    }

    protected abstract void SetWeapon();

    public virtual void EnemyTouch(Enemy enemy)
    {
        gridSlot.Destroyed();
        enemy.Kill();
    }
}