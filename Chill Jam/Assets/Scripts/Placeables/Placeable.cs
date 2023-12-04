using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Placeable : MonoBehaviour {
    [SerializeField] protected SpriteRenderer sr;
    protected PlaceableData data;

    public virtual void Initialize(PlaceableData data)
    {
        this.data = data;

        sr.sprite = data.sprite;

        SetWeapon();
    }

    protected abstract void SetWeapon();
}