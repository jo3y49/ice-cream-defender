using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour {
    public static WorldManager Instance {get; private set;}
    [SerializeField] private UIManager uiManager;
    [SerializeField] private IceCream iceCream;
    [SerializeField] private Transform[] spawns;
    private float bigY;
    [SerializeField] private GameObject waveButton;
    private readonly List<EnemyData> normalEnemies = new();
    private readonly List<EnemyData> speedyEnemies = new();
    private readonly List<EnemyData> bulkyEnemies = new();
    private readonly List<EnemyData> bossEnemies = new(); 

    public float timeBetweenSpawns = 1;
    public float startWaveDelay = 2;
    public int waveCoins = 100;

    public int[] normalWaves = {1,3,6,8};
    public int[] speedyWaves = {2,7};
    public int[] bulkyWaves = {4,9};
    public int[] bossWaves = {5,10};
    public int normalCount = 10;
    public int normalBaseCount = 1;
    public int speedyCount = 8;
    public int speedyBaseCount = 1;
    public int bulkyCount = 10;
    public int bulkyBaseCount = 2;
    public int bossCount = 5;
    public int bossBaseCount = 1;

    public int finalWave = 10;

    private int wave = 1;
    private int spawnAmount;
    public Queue<EnemyData> waveEnemies = new();
    // public List<GameObject> liveEnemies = new();

    private void Awake() {
        Instance = this;

        SortEnemies();

        spawnAmount = spawns.Length;

        foreach (Transform t in spawns)
            bigY += t.position.y;

        bigY /= spawnAmount;

        uiManager.SetWave(wave);

        waveButton.SetActive(true);
        waveButton.GetComponent<Button>().onClick.AddListener(PlayerReady);
    }

    private void SortEnemies()
    {
        EnemyData[] enemies = Resources.LoadAll<EnemyData>("Enemies");

        foreach (EnemyData e in enemies)
        {
            switch (e.rarity)
            {
                case EnemyData.Rarity.Normal:
                    normalEnemies.Add(e);
                    break;
                case EnemyData.Rarity.Speedy:
                    speedyEnemies.Add(e);
                    break;
                case EnemyData.Rarity.Bulky:
                    bulkyEnemies.Add(e);
                    break;
                case EnemyData.Rarity.Boss:
                    bossEnemies.Add(e);
                    break;
            }
        }
    }

    public void PlayerReady()
    {
        if (Time.timeScale == 0) return;

        AudioManager.instance.StartWave();

        iceCream.Hop();

        StartWave(wave);

        waveButton.SetActive(false);
    }

    private void StartWave(int wave)
    {
        EnemyData enemy = null;
        int enemyCount = 10;
        int enemyBaseAmount = 1;
        int enemyIncrease = 3;
        float waveDelay = 3;
        int waveDelayChange = 4;

        if (wave == 6) timeBetweenSpawns /= 2f;

        
        if (Array.IndexOf(normalWaves, wave) != -1)
        {
            enemy = normalEnemies[0];
            enemyCount = normalCount;
            enemyBaseAmount = normalBaseCount++;
            if (wave == 1)
            {
                waveDelayChange = 5;
            } else if (wave == 6)
            {
                normalCount *= 2;
                enemyBaseAmount = normalBaseCount++;
                waveDelay /= 2;
            } else if (wave == 8)
            {
                enemyBaseAmount = normalBaseCount++;
                waveDelay /= 4;
            }

            normalCount *= 2;
        }
    
        else if (Array.IndexOf(speedyWaves, wave) != -1)
        {
            enemy = speedyEnemies[0];
            enemyCount = speedyCount;
            enemyBaseAmount = speedyBaseCount++;

            if (wave > 5)
            {
                speedyCount *= 3;
                enemyBaseAmount = speedyBaseCount++;
                waveDelay /= 3;
            }

            speedyCount *= 2;
        }
    
        else if (Array.IndexOf(bulkyWaves, wave) != -1)
        {
            enemy = bulkyEnemies[0];
            enemyCount = bulkyCount;
            enemyBaseAmount = bulkyBaseCount;
            enemyIncrease = 4;
            waveDelayChange = 3;

            if (wave == 9) waveDelay /= 2;

            bulkyCount *= 5;
            bulkyBaseCount *= 5;
        }
    
        else if (Array.IndexOf(bossWaves, wave) != -1)
        {
            enemy = bossEnemies[0];
            enemyCount = bossCount;
            enemyBaseAmount = bossBaseCount;
            enemyIncrease = 2;
            waveDelay = 5;
            waveDelayChange = 5;

            bossBaseCount *= 4;
            bossCount *= 4;
        }

        SetWave(enemy, enemyCount, enemyBaseAmount, enemyIncrease, waveDelay, waveDelayChange);
    }

    private void SetWave(EnemyData enemyData, int total, int enemyBaseAmount, int enemyIncrease, float waveDelay, int waveDelayChange)
    {
        while (waveEnemies.Count < total)
        {
            waveEnemies.Enqueue(enemyData);
        }

        StartCoroutine(SpawnEnemies(enemyBaseAmount, enemyIncrease, waveDelay, waveDelayChange));
    }

    private IEnumerator SpawnEnemies(int enemyBaseAmount, int enemyIncrease, float waveDelay, int waveDelayChange)
    {
        yield return new WaitForSeconds(startWaveDelay);

        int i = 1;

        while (true)
        {
            if (i == enemyIncrease) 
            {
                enemyBaseAmount *= 2;

                if (wave == 1)
                {
                    enemyIncrease += 2;
                }
            }

            if (i == waveDelayChange) waveDelay /= 2;

            for (int j = 0; j < enemyBaseAmount; j++)
            {
                int r = UnityEngine.Random.Range(0, spawnAmount);
                Transform spawn = spawns[r];

                if (waveEnemies.Count > 0) 
                {
                    EnemyData nextEnemy = waveEnemies.Peek();

                    SpawnEnemy(spawn, nextEnemy);
                }

                yield return new WaitForSeconds(timeBetweenSpawns);

                if (waveEnemies.Count <= 0) goto EndLoop;
            }

            i++;

            float t = 0;

            while (t < waveDelay && Pool.Instance.AreEnemiesActive())
            {
                t += Time.deltaTime; 
                yield return null;
            }
        }

        EndLoop:

        StartCoroutine(WaitForEnd());
    }

    private void SpawnEnemy(Transform spawn, EnemyData nextEnemy)
    {
        if (nextEnemy == null) return;

        waveEnemies.Dequeue();

        GameObject enemyObject = Pool.Instance.GetEnemy();

        if (nextEnemy.size == 1)
            enemyObject.transform.position = spawn.transform.position;
        else 
            enemyObject.transform.position = new Vector3(spawn.position.x, bigY, spawn.position.z);

        enemyObject.SetActive(true);
        enemyObject.GetComponent<Enemy>().Initialize(nextEnemy, spawn);
    }

    private IEnumerator WaitForEnd()
    {
        // check if there are any gameobjects with the enemy component active in the scene

        while (Pool.Instance.AreEnemiesActive())
        {
            yield return null;
        }


        

        EndWave();
    }

    public void DefeatedEnemy(GameObject enemy, int coins)
    {
        GameDataManager.Instance.AddCoins(coins);

        Pool.Instance.ReturnEnemy(enemy);
    }

    private void EndWave()
    {
        if (UIManager.Instance.Loss()) return;

        StopAllCoroutines();

        waveEnemies = new();

        AudioManager.instance.Win();

        iceCream.Hop();

        GameDataManager.Instance.AddCoins(waveCoins);

        wave++;

        uiManager.SetWave(wave);

        waveButton.SetActive(true);

        if (wave > finalWave) SceneManager.LoadScene(2);
    }

    public void DebugSkipWave()
    {
        if (waveEnemies.Count == 0) return;

        StopAllCoroutines();

        // Call the shot method on every active object with the enemy component
        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            e.Shot(100);
        } 
        

        EndWave();
        
    }
}