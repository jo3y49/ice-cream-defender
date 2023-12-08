using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Placeable : MonoBehaviour {
    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] private Transform healthBar;
    public PlaceableData data;
    public int health;
    protected GridSlot gridSlot;

    private Vector3 healthBarScale;

    public virtual void SetUp(GridSlot gridSlot)
    {
        this.gridSlot = gridSlot;
        healthBarScale = healthBar.localScale;
        healthBar.localPosition = new Vector3(0, healthBar.position.y, 0);
    }

    public virtual void Initialize(PlaceableData data, int health = 0)
    {
        this.data = data;
        sr.sprite = data.sprite;
        
        if (health == 0 || data.health == health)
        {
            this.health = data.health;
            healthBar.localScale = healthBarScale;
            healthBar.gameObject.SetActive(false);
        } 
        else
        {
            this.health = health;
            healthBar.gameObject.SetActive(true);
            healthBar.localScale = new Vector3(healthBarScale.x * health / data.health, healthBarScale.y,1);
        } 

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

        healthBar.gameObject.SetActive(true);

        if (health <= 0)
            gridSlot.Destroyed();

        else healthBar.localScale = new Vector3(healthBarScale.x * health / data.health, healthBarScale.y,1);

        
    }
}