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
    [SerializeField] private Transform[] leftSpawns;
    [SerializeField] private Transform[] rightSpawns;
    private float bigY;
    [SerializeField] private GameObject waveButton;
    private List<EnemyData> normalEnemies = new();
    private List<EnemyData> speedyEnemies = new();
    private List<EnemyData> bulkyEnemies = new();
    private List<EnemyData> bossEnemies = new(); 

    public float timeBetweenSpawns = 1;
    public float waveDelay = 2;
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
    private List<EnemyData> waveEnemies = new();
    private List<GameObject> liveEnemies = new();

    private void Awake() {
        Instance = this;

        SortEnemies();

        foreach (Transform t in leftSpawns)
            bigY += t.position.y;

        bigY /= leftSpawns.Length;

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
        foreach (int i in normalWaves)
        {
            if (i == wave)
            {
                SetWave(normalEnemies[0], normalCount);
                normalCount *= 2;
                return;
            }
        }

        foreach (int i in speedyWaves)
        {
            if (i == wave)
            {
                SetWave(speedyEnemies[0], speedyCount);
                speedyCount *= 2;
                return;
            }
        }

        foreach (int i in bulkyWaves)
        {
            if (i == wave)
            {
                SetWave(bulkyEnemies[0], bulkyCount);
                bulkyCount *= 2;
                return;
            }
        }

        foreach (int i in bossWaves)
        {
            if (i == wave)
            {
                SetWave(bossEnemies[0], bossCount);
                bossCount *= 2;
                return;
            }
        }
    }

    private void SetWave(EnemyData enemyData, int total)
    {
        while (waveEnemies.Count <= total)
        {
            waveEnemies.Add(enemyData);
        }

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(waveDelay);

        while (waveEnemies.Count > 0)
        {
            foreach (Transform spawn in leftSpawns)
            {
                SpawnEnemy(spawn, waveEnemies[0], true);

                yield return new WaitForSeconds(timeBetweenSpawns);

                if (waveEnemies.Count == 0) goto EndLoop;
            }

            foreach (Transform spawn in rightSpawns)
            {
                SpawnEnemy(spawn, waveEnemies[0], false);
                
                yield return new WaitForSeconds(timeBetweenSpawns);

                if (waveEnemies.Count == 0) goto EndLoop;
            }
        }

        EndLoop:

        StartCoroutine(WaitForEnd());
    }

    private void SpawnEnemy(Transform spawn, EnemyData nextEnemy, bool moveRight)
    {
        waveEnemies.Remove(nextEnemy);

        GameObject enemyObject = Pool.Instance.GetEnemy();

        if (nextEnemy.size == 1)
            enemyObject.transform.position = spawn.transform.position;
        else 
            enemyObject.transform.position = new Vector3(spawn.position.x, bigY, spawn.position.z);

        
        enemyObject.SetActive(true);
        enemyObject.GetComponent<Enemy>().Initialize(nextEnemy, moveRight);
        
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