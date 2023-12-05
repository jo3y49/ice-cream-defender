using UnityEngine;
using UnityEngine.UI;

public class ItemOptions : MonoBehaviour {
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject upgradeButton;
    private GridSlot gridSlot;
    private PlaceableData data;

    public void SetData(GridSlot gridSlot)
    {
        if (gridSlot.activeItem == null || (gameObject.activeSelf && gridSlot == this.gridSlot))
        {
            Selected();
            return;
        }

        gameObject.SetActive(true);

        this.gridSlot = gridSlot;

        data = gridSlot.activeItem.data;

        // itemImage.sprite = data.sprite;

        if (data.upgrade != null)
        {
            upgradeButton.SetActive(true);
            upgradeButton.GetComponent<Button>().interactable = GameDataManager.Instance.GetCoins() >= data.upgrade.price;
        }
        else 
            upgradeButton.SetActive(false);
        
    }

    public void Move()
    {
        if (!MouseObject.Instance.Sticking)
        {
            MouseObject.Instance.StickToMouse(data);
            gridSlot.Destroyed();
        }  

        Selected();
    }

    public void Upgrade()
    {
        gridSlot.activeItem.Upgrade();

        Selected();
    }

    public void Sell()
    {
        GameDataManager.Instance.AddCoins(data.price/2);

        gridSlot.Destroyed();

        Selected();
    }

    private void Selected()
    {
        gridSlot = null;
        gameObject.SetActive(false);
    }
}