using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour {
    public static WorldManager Instance {get; private set;}
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Transform[] spawns;
    private float bigY;
    [SerializeField] private GameObject waveButton;
    private List<EnemyData> normalEnemies = new();
    private List<EnemyData> speedyEnemies = new();
    private List<EnemyData> bulkyEnemies = new();
    private List<EnemyData> bossEnemies = new(); 

    public float timeBetweenSpawns = 1;
    public float startWaveDelay = 2;
    public float waveCoinRewardMultiplier = 100;

    public int[] normalWaves = {1,3,6,8};
    public int[] speedyWaves = {2,7};
    public int[] bulkyWaves = {4,9};
    public int[] bossWaves = {5,10};
    public int normalCount = 10;
    public int speedyCount = 8;
    public int bulkyCount = 10;
    public int bossCount = 5;

    public int finalWave = 10;

    private int wave = 1;
    private int spawnAmount;
    private List<EnemyData> waveEnemies = new();
    private List<GameObject> liveEnemies = new();

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

    private void PlayerReady()
    {
        StartWave(wave);

        waveButton.SetActive(false);
    }

    private void StartWave(int wave)
    {
        EnemyData enemy = null;
        int enemyCount = 10;
        int enemyBaseAmount = 1;
        int enemyIncrease = 3;
        int waveDelay = 5;
        int waveDelayChange = 4;

        foreach (int i in normalWaves)
        {
            if (i == wave)
            {
                enemy = normalEnemies[0];
                enemyCount = normalCount;
    
                if (i == 1)
                {
                    waveDelayChange = 5;
                } 

                normalCount *= 2;
                goto EndLoop;
            }
        }

        foreach (int i in speedyWaves)
        {
            if (i == wave)
            {
                enemy = speedyEnemies[0];
                enemyCount = speedyCount;
    
                speedyCount *= 2;
                goto EndLoop;
            }
        }

        foreach (int i in bulkyWaves)
        {
            if (i == wave)
            {
                enemy = bulkyEnemies[0];
                enemyBaseAmount = 2;
                enemyCount = bulkyCount;
                enemyIncrease = 4;
                waveDelayChange = 3;

                bulkyCount *= 2;
                goto EndLoop;
            }
        }

        foreach (int i in bossWaves)
        {
            if (i == wave)
            {
                enemy = bossEnemies[0];
                enemyCount = bossCount;
                enemyIncrease = 2;
                waveDelay = 10;
                waveDelayChange = 5;

                bossCount *= 2;
                goto EndLoop;
            }
        }

        EndLoop:

        SetWave(enemy, enemyCount, enemyBaseAmount, enemyIncrease, waveDelay, waveDelayChange);
    }

    private void SetWave(EnemyData enemyData, int total, int enemyBaseAmount, int enemyIncrease, int waveDelay, int waveDelayChange)
    {
        while (waveEnemies.Count <= total)
        {
            waveEnemies.Add(enemyData);
        }

        StartCoroutine(SpawnEnemies(total, enemyBaseAmount, enemyIncrease, waveDelay, waveDelayChange));
    }

    private IEnumerator SpawnEnemies(int total, int enemyBaseAmount, int enemyIncrease, int waveDelay, int waveDelayChange)
    {
        yield return new WaitForSeconds(startWaveDelay);

        int i = 0;

        while (true)
        {
            if (i == enemyIncrease) enemyBaseAmount *= 2;

            if (i == waveDelayChange) waveDelay = waveDelay / 2 + 1;

            for (int j = 0; j < enemyBaseAmount; j++)
            {
                int r = Random.Range(0, spawnAmount);
                Transform spawn = spawns[r];

                SpawnEnemy(spawn, waveEnemies[0]);

                yield return new WaitForSeconds(timeBetweenSpawns);

                if (waveEnemies.Count == 0) goto EndLoop;
            }

            i++;

            yield return new WaitForSeconds(waveDelay);
        }

        EndLoop:

        StartCoroutine(WaitForEnd());
    }

    private void SpawnEnemy(Transform spawn, EnemyData nextEnemy)
    {
        waveEnemies.Remove(nextEnemy);

        GameObject enemyObject = Pool.Instance.GetEnemy();

        if (nextEnemy.size == 1)
            enemyObject.transform.position = spawn.transform.position;
        else 
            enemyObject.transform.position = new Vector3(spawn.position.x, bigY, spawn.position.z);

        
        enemyObject.SetActive(true);
        enemyObject.GetComponent<Enemy>().Initialize(nextEnemy, spawn);
        
        liveEnemies.Add(enemyObject);
    }

    private IEnumerator WaitForEnd()
    {
        yield return new WaitUntil(() => liveEnemies.Count == 0);

        EndWave();
    }

    public void DefeatedEnemy(GameObject enemy, int coins)
    {
        GameDataManager.Instance.AddCoins(coins);

        liveEnemies.Remove(enemy);

        Pool.Instance.ReturnEnemy(enemy);
    }

    private void EndWave()
    {
        GameDataManager.Instance.AddCoins((int)(wave * waveCoinRewardMultiplier));

        wave++;

        uiManager.SetWave(wave);

        waveButton.SetActive(true);
    }
}