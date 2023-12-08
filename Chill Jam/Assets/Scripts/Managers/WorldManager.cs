using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour {
    public static WorldManager Instance {get; private set;}
    [SerializeField] private UIManager uiManager;
    [SerializeField] private IceCream iceCream;
    [SerializeField] private Transform[] spawns;
    private float bigY;
    [SerializeField] private GameObject waveButton;
    private List<EnemyData> normalEnemies = new();
    private List<EnemyData> speedyEnemies = new();
    private List<EnemyData> bulkyEnemies = new();
    private List<EnemyData> bossEnemies = new(); 

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
    public List<EnemyData> waveEnemies = new();
    public List<GameObject> liveEnemies = new();

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

        foreach (int i in normalWaves)
        {
            if (i == wave)
            {
                enemy = normalEnemies[0];
                enemyCount = normalCount;
                enemyBaseAmount = normalBaseCount++;
                if (i == 1)
                {
                    waveDelayChange = 5;
                } else if (i > 5)
                {
                    normalCount *= 2;
                    enemyBaseAmount = normalBaseCount++;
                    waveDelay /= 2;
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
                enemyBaseAmount = speedyBaseCount++;

                if (i > 5)
                {
                    speedyCount *= 2;
                    enemyBaseAmount = speedyBaseCount++;
                    waveDelay /= 2;
                }
    
                speedyCount *= 2;
                goto EndLoop;
            }
        }

        foreach (int i in bulkyWaves)
        {
            if (i == wave)
            {
                enemy = bulkyEnemies[0];
                enemyCount = bulkyCount;
                enemyBaseAmount = bulkyBaseCount;
                enemyIncrease = 4;
                waveDelayChange = 3;

                bulkyCount *= 4;
                bulkyBaseCount *= 4;
                goto EndLoop;
            }
        }

        foreach (int i in bossWaves)
        {
            if (i == wave)
            {
                enemy = bossEnemies[0];
                enemyCount = bossCount;
                enemyBaseAmount = bossBaseCount;
                enemyIncrease = 2;
                waveDelay = 5;
                waveDelayChange = 5;

                bossBaseCount *= 4;
                bossCount *= 4;
                goto EndLoop;
            }
        }

        EndLoop:

        SetWave(enemy, enemyCount, enemyBaseAmount, enemyIncrease, waveDelay, waveDelayChange);
    }

    private void SetWave(EnemyData enemyData, int total, int enemyBaseAmount, int enemyIncrease, float waveDelay, int waveDelayChange)
    {
        while (waveEnemies.Count < total)
        {
            waveEnemies.Add(enemyData);
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
                int r = Random.Range(0, spawnAmount);
                Transform spawn = spawns[r];

                if (waveEnemies.Count > 0) 
                {
                    EnemyData nextEnemy = waveEnemies[0];

                    SpawnEnemy(spawn, nextEnemy);
                }

                yield return new WaitForSeconds(timeBetweenSpawns);

                if (waveEnemies.Count <= 0) goto EndLoop;
            }

            i++;

            float t = 0;

            while (t < waveDelay && liveEnemies.Count > 0)
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

        waveEnemies.Remove(nextEnemy);

        GameObject enemyObject = Pool.Instance.GetEnemy();

        if (nextEnemy.size == 1)
            enemyObject.transform.position = spawn.transform.position;
        else 
            enemyObject.transform.position = new Vector3(spawn.position.x, bigY, spawn.position.z);

        liveEnemies.Add(enemyObject);
        
        enemyObject.SetActive(true);
        enemyObject.GetComponent<Enemy>().Initialize(nextEnemy, spawn);
    }

    private IEnumerator WaitForEnd()
    {
        while (liveEnemies.Count > 0)
        {
            int n = liveEnemies.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                if (!liveEnemies[i].activeSelf) liveEnemies.RemoveAt(i);
            }

                yield return null;
            }

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
        if (UIManager.Instance.Lossed()) return;

        StopAllCoroutines();

        waveEnemies = new();
        liveEnemies = new();

        AudioManager.instance.Win();

        iceCream.Hop();

        GameDataManager.Instance.AddCoins(waveCoins);

        wave++;

        uiManager.SetWave(wave);

        waveButton.SetActive(true);
    }

    public void DebugSkipWave()
    {
        if (waveEnemies.Count == 0) return;

        StopAllCoroutines();

        int n = liveEnemies.Count;

        for (int i = n - 1; i >= 0; i--)
        {
            Pool.Instance.ReturnEnemy(liveEnemies[i]);
        }

        EndWave();
        
    }
}