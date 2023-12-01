using UnityEngine;
using System.Collections.Generic;

public class PlayerBattle : CharacterBattle {
    private int experience = 0;
    public int level {get; private set;}

    protected override void Start() {
        base.Start();

        CharacterName = "Leoh";

        attackKeys.Add("physical");
        attackKeys.Add("ranged");
        attackKeys.Add("heal");

        attackActions.Add(AttackList.GetInstance().GetAction(attackKeys[0]));
        attackActions.Add(AttackList.GetInstance().GetAction(attackKeys[1]));
        attackActions.Add(AttackList.GetInstance().GetAction(attackKeys[2]));
    }

    public override void PrepareCombat()
    {
        GetComponent<PlayerMovement>().enabled = false;
    }

    private void LevelUp(int xpForLevel)
    {
        experience -= xpForLevel;
        level++;

        SetStats(level);

        GameDataManager.instance.SetPlayerLevel(level);
    }

    public void SetStats(int level)
    {
        
    }

    public void EndCombat()
    {
        GetComponent<PlayerMovement>().enabled = true;
    }
    public void SetData(int level, int experience)
    {
        this.level = level;
        this.experience = experience;
        
        // SetStats(level);
    }

    public void ResetHealth()
    {
        health = maxHealth;
    }
}