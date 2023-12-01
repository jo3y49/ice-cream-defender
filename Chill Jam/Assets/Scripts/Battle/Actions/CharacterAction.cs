using UnityEngine;

public class CharacterAction {
    public delegate void AttackDelegate(CharacterBattle self, CharacterBattle target);

    public string Name { get; protected set; }
    public AttackDelegate Action { get; private set; }

    public CharacterAction(string name, AttackDelegate action)
    {
        Name = name;
        Action = action;
    }

    public static void DoAttack(CharacterBattle self, CharacterBattle target, string attackName, int damage = 1)
    {
        DoAction(self, attackName);

        target.SetAnimationTrigger("Attacked");

        damage *= self.attack;


        target.Attacked(damage);
    }

    public static void DoAttackRandom(CharacterBattle self, CharacterBattle target, string attackName, int low, int high)
    {
        int damage = Random.Range(low, high + 1);

        DoAttack(self, target, attackName, damage);
    }

    public static void DoHeal(CharacterBattle self, string attackName, int heal = 1)
    {
        DoAction(self, attackName);

        self.Heal(heal);
    }

    private static void DoAction(CharacterBattle self, string attackName)
    {
        self.AttackTrigger(attackName);
    }
    
}