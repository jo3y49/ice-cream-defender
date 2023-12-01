using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe")]
public class Recipe : ScriptableObject {
    public ItemQuantity result;
    public ItemQuantity[] ingredients;
}