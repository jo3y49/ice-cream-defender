using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour {
    public static WorldManager Instance {get; private set;}
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Transform[] leftSpawns;
    [SerializeField] private Transform[] rightSpawns;
    private List<EnemyData> commonEnemies = new();
    private List<EnemyData> uncommonEnemies = new();
    private List<EnemyData> rareEnemies = new();
    private List<EnemyData> ultrarareEnemies = new(); 
    [SerializeField] private GameObject enemyPrefab;
    protected GameDataManager gameManager;
    protected GameObject player;

    public float timeBetweenSpawns = 1;
    public float waveDelay = 2;
    public float waveCoinRewardMultiplier = 100;

    private int wave = 1;
    private Queue<EnemyData> waveEnemies = new();
    private List<GameObject> liveEnemies = new();

    private void Awake() {
        Instance = this;
    }

    protected virtual void Start() {
        gameManager = GameDataManager.Instance;

        player = GameObject.FindGameObjectWithTag("Player");
        gameManager.SetPlayer(player);

        SortEnemies();

        StartWave(wave);
    }

    private void SortEnemies()
    {
        EnemyData[] enemies = Resources.LoadAll<EnemyData>("Enemies");

        foreach (EnemyData e in enemies)
        {
            switch (e.rarity)
            {
                case EnemyData.Rarity.Common:
                    commonEnemies.Add(e);
                    break;
                case EnemyData.Rarity.Uncommon:
                    uncommonEnemies.Add(e);
                    break;
                case EnemyData.Rarity.Rare:
                    rareEnemies.Add(e);
                    break;
                case EnemyData.Rarity.UltraRare:
                    ultrarareEnemies.Add(e);
                    break;
            }
        }
    }

    private void StartWave(int wave)
    {
        int common = 16 + (int)Mathf.Pow(wave, 2);

        int perPath = common / (leftSpawns.Length + rightSpawns.Length);

        EnemyData commonEnemy = commonEnemies[0];

        for (int i = 0; i < common; i++)
        {
            waveEnemies.Enqueue(commonEnemy);
        }

        StartCoroutine(SpawnEnemies(perPath));
    }

    private void FillWave()
    {
        
    }

    private IEnumerator SpawnEnemies(int perPath)
    {
        uiManager.SetWave(wave);

        Pool.Instance.AddEnemies(waveEnemies.Count);

        yield return new WaitForSeconds(waveDelay);

        for (int i = 0; i < perPath; i++)
        {
            foreach (Transform spawn in leftSpawns)
            {
                SpawnEnemy(spawn, true);
            }
            foreach (Transform spawn in rightSpawns)
            {
                SpawnEnemy(spawn, false);
            }

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        StartCoroutine(WaitForEnd());
    }

    private void SpawnEnemy(Transform spawn, bool moveRight)
    {
        EnemyData nextEnemy = waveEnemies.Peek();
        GameObject enemyObject = Pool.Instance.GetEnemy();
        enemyObject.transform.position = spawn.transform.position;
        enemyObject.SetActive(true);
        enemyObject.GetComponent<Enemy>().Initialize(nextEnemy, moveRight);
        

        liveEnemies.Add(enemyObject);
        // enemyObject.transform.position = ;
        

        
        waveEnemies.Dequeue();
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

        StartWave(++wave);
    }
}