using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour {
    [SerializeField] private Path[] paths;
    [SerializeField] private EnemyData[] enemies;
    protected GameDataManager gameManager;
    protected GameObject player;

    public float timeBetweenSpawns = 1;

    private int wave = 1;

    protected virtual void Start() {
        gameManager = GameDataManager.instance;

        player = GameObject.FindGameObjectWithTag("Player");
        gameManager.SetPlayer(player);

        enemies = Resources.LoadAll<EnemyData>("Enemies");

        StartWave();


    }

    public void StartWave()
    {
        int common = 10 + (int)Mathf.Pow(wave, 2);

        int perPath = common / paths.Length;

        StartCoroutine(SpawnEnemies(perPath));
    }

    private IEnumerator SpawnEnemies(int perPath)
    {
        EnemyData enemy = enemies[0];

        for (int i = 0; i < perPath; i++)
        {
            Debug.Log("spawning");

            foreach (Path path in paths)
            {
                path.SpawnEnemy(enemy);
            }

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public void EndWave()
    {
        wave++;
    }
}