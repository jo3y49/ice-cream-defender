using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Path : MonoBehaviour {
    [SerializeField] private SpriteRenderer sr;
    private Bounds bounds;
    private Vector3 basePoint;

    private void Awake() {
        bounds = sr.localBounds;
        basePoint = new Vector3(0, -bounds.extents.y, 0);
    }

    public Vector3 GetLocation(float distance)
    {
        float distanceOnPath = (100-distance)/100f;

        Vector3 newPoint = basePoint + new Vector3(0, bounds.size.y * distanceOnPath, 0);
        Vector3 transformNewPoint = transform.TransformPoint(newPoint);

        return transformNewPoint;
    }
}