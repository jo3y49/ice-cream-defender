using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour {
    [SerializeField] protected GameObject battleUI;
    // [SerializeField] protected DialogManager dialogManager;
    protected GameDataManager gameManager;
    protected GameObject player;
    protected PlayerBattle playerBattle;
    protected PlayerMovement playerMovement;
    protected List<EnemyBattle> enemies = new();

    protected virtual void Start() {
        gameManager = GameDataManager.instance;

        // int currentScene = SceneManager.GetActiveScene().buildIndex;
        // gameManager.SetCurrentScene(currentScene);

        player = GameObject.FindGameObjectWithTag("Player");
        gameManager.SetPlayer(player);
        playerBattle = player.GetComponent<PlayerBattle>();
        playerMovement = player.GetComponent<PlayerMovement>();

        StartCoroutine(WaitToBattle());
    }

    public virtual List<EnemyBattle> GetBattleEnemies() 
    {
        return enemies;
    }

    public virtual void EncounterEnemy()
    {
        if (battleUI.activeSelf) return;

        foreach (EnemyBattle e in FindObjectsOfType<EnemyBattle>())
        {
            enemies.Add(e);
            e.PrepareCombat();  
        }

        // enemies.Reverse();

        StartBattle();
    }

    public virtual void WinBattle()
    {
        enemies.Clear();
    }

    public virtual void LoseBattle()
    {
        SceneManager.LoadScene("Title Screen");
    }

    public virtual void EscapeBattle(){}

    // protected virtual IEnumerator DoDialog(DialogObject dialogObject)
    // {
    //     playerMovement.enabled = false;

    //     dialogManager.enabled = true;
        
    //     // Start the next dialog.
    //     dialogManager.DisplayDialog(dialogObject);

    //     // Wait for this dialog to finish before proceeding to the next one.
    //     yield return new WaitUntil(() => !dialogManager.ShowingDialog());
        
    //     dialogManager.enabled = false;

    //     playerMovement.enabled = true;
    // }

    protected virtual void StartBattle() 
    {
        playerBattle.PrepareCombat();
        battleUI.SetActive(true);
    }

    public PlayerBattle GetPlayer() { return playerBattle; }

    private IEnumerator WaitToBattle()
    {
        yield return new WaitForSeconds(1);

        EncounterEnemy();
    }
}