using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour {
    public static Pool Instance;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject enemyPrefab;
    private Queue<GameObject> bullets = new();
    private Queue<GameObject> enemies = new();

    private void Awake() {
        Instance = this;

        NewObject(enemyPrefab, enemies, 50);
        NewObject(bulletPrefab, bullets, 50);
    }

    public GameObject GetBullet()
    {
        if (bullets.Count == 0)
        {
            NewObject(bulletPrefab, bullets);
        }

        return bullets.Dequeue();
    }

    public GameObject GetEnemy()
    {
        if (enemies.Count == 0)
        {
            NewObject(enemyPrefab, enemies);
        }

        return enemies.Dequeue();
    }

    private void NewObject(GameObject prefab, Queue<GameObject> queue, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            queue.Enqueue(obj);
        }
    }

    public void ReturnBullet(GameObject obj)
    {
        obj.SetActive(false);
        bullets.Enqueue(obj);
    }

    public void ReturnEnemy(GameObject obj)
    {
        obj.SetActive(false);
        enemies.Enqueue(obj);
    }
}