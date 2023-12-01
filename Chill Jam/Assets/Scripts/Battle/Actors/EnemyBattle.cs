using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBattle : CharacterBattle {
    protected WorldManager worldManager;
    protected GameObject player;
    protected bool isAttacking;
    [SerializeField] private EnemyInfo enemyInfo;

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player");
        // worldManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<WorldManager>();
    }

    // public override void PrepareCombat()
    // {
    //     SetStats();
    // }

    public bool DoAction()
    {
        if (enemyInfo.distance >= 5)
        {
            enemyInfo.NextTurn();
            return false;
        }
            
        else 
        {
            return true;
        }
    }

    public void ResetFromFight()
    {
        gameObject.SetActive(true);
    }

    protected virtual void SetStats(){}

    public override void Kill()
    {
        Destroy(gameObject);
    }

    public override void Defeated()
    {
        gameObject.SetActive(false);
    }
}