using UnityEngine;
public abstract class PlaceableData : ScriptableObject {
    public Sprite sprite;
    public PlaceableData upgrade;
    public int price = 0;
    public int health = 1;
    public int damage = 50;
}