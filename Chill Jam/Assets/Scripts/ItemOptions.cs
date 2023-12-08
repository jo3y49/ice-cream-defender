using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemOptions : MonoBehaviour {
    [SerializeField] private Image itemImage;
    [SerializeField] private Button upgradeButton;
    private Image upgradeImage;
    [SerializeField] private Sprite upgradeReady, upgradeNot, upgradeMax;
    [SerializeField] private TextMeshProUGUI levelText;
    private GridSlot gridSlot;
    private PlaceableData data;

    private void Awake() {
        upgradeImage = upgradeButton.GetComponent<Image>();
    }

    public void SetData(GridSlot gridSlot)
    {
        if (gridSlot.activeItem == null || (gameObject.activeSelf && gridSlot == this.gridSlot))
        {
            Selected();
            return;
        }

        UpdateItem(gridSlot);
    }

    private void UpdateItem(GridSlot gridSlot)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);

        this.gridSlot = gridSlot;

        data = gridSlot.activeItem.data;
        levelText.text = data.level.ToString();

        itemImage.sprite = data.sprite;
        itemImage.SetNativeSize();

        if (data.upgrade != null)
        {
            if (GameDataManager.Instance.GetCoins() >= data.upgrade.price)
            {
                upgradeImage.sprite = upgradeReady;
            } else
            {
                upgradeImage.sprite = upgradeNot;
            }
        }
        else 
            upgradeImage.sprite = upgradeMax;

        upgradeButton.interactable = upgradeImage.sprite == upgradeReady;
    }

    public void Move()
    {
        if (!MouseObject.Instance.Sticking)
        {
            MouseObject.Instance.StickToMouse(data, gridSlot);
            gridSlot.Destroyed();
        }  

        Selected();
    }

    public void Upgrade()
    {
        gridSlot.activeItem.Upgrade();
        UpdateItem(gridSlot);
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

    private void OnEnable() {
        StartCoroutine(CheckForClick());
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    private IEnumerator CheckForClick()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));

        Selected();
    }
}