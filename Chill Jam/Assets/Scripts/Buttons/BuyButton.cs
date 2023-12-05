using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BuyButton : MonoBehaviour {
    [SerializeField] private PlaceableData item;

    public void Transaction()
    {
        if (GameDataManager.Instance.GetCoins() >= item.price && !MouseObject.Instance.Sticking)
        {
            GameDataManager.Instance.AddCoins(-item.price);

            MouseObject.Instance.StickToMouse(item);
        }
    }

    
}