using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BuyButton : MonoBehaviour {
    [SerializeField] private Image image;
    [SerializeField] private PlaceableData item;

    private void Awake() {
        image.sprite = item.sprite;
    }

    public void Transaction()
    {
        if (GameDataManager.Instance.GetCoins() >= item.price && !MouseObject.Instance.Sticking)
        {
            GameDataManager.Instance.AddCoins(-item.price);

            MouseObject.Instance.StickToMouse(item);
        }
    }

    
}