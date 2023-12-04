using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
public class EnemyData : ScriptableObject {
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        UltraRare,
    }

    public Sprite sprite;
    public Rarity rarity;
    public int health = 1;
    
    [Range(.1f, 5)]
    public float speed = .5f;

    [Range(1,2)]
    public int size = 1;
    public int coins;
}