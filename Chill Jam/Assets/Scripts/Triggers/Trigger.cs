using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour {
    public bool destroy = true;

    protected virtual void OnTriggerEnter2D(Collider2D other) 
    {
        if (destroy) Destroy(gameObject);
    }
}

