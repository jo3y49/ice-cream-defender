using System.Collections.Generic;
using UnityEngine;

public class AttackList
{
    private static AttackList instance;
    private IDictionary<string, CharacterAction> actionList;

    private AttackList()
    {
        FillDictionary();
    }

    public static AttackList GetInstance()
    {
        instance ??= new AttackList();

        return instance;
    }

    public CharacterAction GetAction(string key)
    {
        if (actionList.ContainsKey(key)) return actionList[key];

        else return new CharacterAction("Null", EmptyAction);
    }


    private void FillDictionary()
    {
        actionList = new Dictionary<string, CharacterAction>()
        {
            {"Attack", new CharacterAction("Attack", HitEnemy)}
        };
    }

    public void EmptyAction(CharacterBattle self, CharacterBattle target)
    {
        Debug.Log("This action is null");
        string attackName = "null";
        int damageMultiplier = 0;

        CharacterAction.DoAttack(self, target, attackName, damageMultiplier);
    }

    public void HitEnemy(CharacterBattle self, CharacterBattle target)
    {
        string attackName = "Attack";
        int damageMultiplier = 1;

        CharacterAction.DoAttack(self, target, attackName, damageMultiplier);
    }
}