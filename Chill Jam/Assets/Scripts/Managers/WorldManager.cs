using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour {
    public static WorldManager Instance {get; private set;}
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Transform[] leftSpawns;
    [SerializeField] private Transform[] rightSpawns;
    [SerializeField] private GameObject waveButton;
    private List<EnemyData> commonEnemies = new();
    private List<EnemyData> uncommonEnemies = new();
    private List<EnemyData> rareEnemies = new();
    private List<EnemyData> ultrarareEnemies = new(); 

    public float timeBetweenSpawns = 1;
    public float waveDelay = 2;
    public float waveCoinRewardMultiplier = 100;

    private int wave = 1;
    private List<EnemyData> waveEnemies = new();
    private List<GameObject> liveEnemies = new();

    private void Awake() {
        Instance = this;

        SortEnemies();

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

    private void PlayerReady()
    {
        StartWave(wave);

        waveButton.SetActive(false);
    }

    private void StartWave(int wave)
    {
        int common = 16 + (int)Mathf.Pow(wave, 2);
        int uncommon = 4 + (int)Mathf.Pow(wave, 1.1f);
        int total = common + uncommon;

        int uncommonGap = common/uncommon;

        EnemyData commonEnemy = commonEnemies[0];
        EnemyData uncommonEnemy = uncommonEnemies[0];

        for (int i = 1; i <= total; i++)
        {
            if (i % uncommonGap == 0)
                waveEnemies.Add(uncommonEnemy);
            else
                waveEnemies.Add(commonEnemy);
        }

        int perPath = (int)(waveEnemies.Count / (float)(leftSpawns.Length + rightSpawns.Length));

        StartCoroutine(SpawnEnemies(perPath));
    }

    private IEnumerator SpawnEnemies(int perPath)
    {
        yield return new WaitForSeconds(waveDelay);

        for (int i = 0; i < perPath; i++)
        {
            foreach (Transform spawn in leftSpawns)
            {
                SpawnEnemy(spawn, waveEnemies[0], true);

                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            foreach (Transform spawn in rightSpawns)
            {
                SpawnEnemy(spawn, waveEnemies[0], false);
                
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }

        StartCoroutine(WaitForEnd());
    }

    private void SpawnEnemy(Transform spawn, EnemyData nextEnemy, bool moveRight)
    {
        waveEnemies.Remove(nextEnemy);

        GameObject enemyObject = Pool.Instance.GetEnemy();
        enemyObject.transform.position = spawn.transform.position;
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