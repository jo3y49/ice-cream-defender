using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class Path : MonoBehaviour {
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private GameObject enemyPrefab;
    private Bounds bounds;
    private Vector3 basePoint;

    private void Awake() {
        Vector3 point = new Vector3(0, -sr.localBounds.extents.y, 0);
        basePoint = transform.TransformPoint(point);
    }

    public void SpawnEnemy(EnemyData enemy)
    {
        GameObject enemyObject = Instantiate(enemyPrefab);
        enemyObject.transform.position = basePoint;
        enemyObject.GetComponent<Enemy>().Initialize(enemy, -(int)Mathf.Sign(transform.rotation.z));
    }
}