using UnityEngine;

public abstract class Placeable : MonoBehaviour {
    protected PlaceableData data;

    public virtual void Initialize(PlaceableData data)
    {
        this.data = data;

        Attack();
    }

    protected abstract void Attack();
}