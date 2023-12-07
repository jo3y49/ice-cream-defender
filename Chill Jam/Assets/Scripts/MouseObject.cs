using UnityEngine;
using System.Collections;

public class MouseObject : MonoBehaviour {
    public static MouseObject Instance;
    [SerializeField] private GameObject itemOnMousePrefab;
    public PlaceableData selectedItem {get; private set;}
    public GameObject itemObject {get; private set;}
    private SpriteRenderer itemObjectRenderer;

    private GridSlot lastGridSlot;

    public bool Sticking {get; private set;} = false;

    private void Awake() {
        Instance = this;

        itemObject = Instantiate(itemOnMousePrefab);
        itemObject.TryGetComponent(out itemObjectRenderer); 

        itemObject.SetActive(false);
    }

    private void OnEnable() {
        PauseManager.PauseEvent += (b) => Undo();
    }

    private void OnDisable() {
        PauseManager.PauseEvent -= (b) => Undo();
    }

    public void StickToMouse(PlaceableData item, GridSlot gridSlot = null)
    {
        if (Time.timeScale == 0) return;

        Sticking = false;

        lastGridSlot = gridSlot;
        UIManager.Instance.SetCancel(true);

        StartCoroutine(StickingToMouse(item));
    }

    private IEnumerator StickingToMouse(PlaceableData item)
    {
        itemObject.SetActive(true);
        if (itemObjectRenderer != null) itemObjectRenderer.sprite = item.sprite;

        selectedItem = item;

        Sticking = true;

        while (Sticking)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
            itemObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);

            yield return null;
        }

        itemObject.SetActive(false);
        UIManager.Instance.SetCancel(false);
    }

    public void Placed()
    {
        Sticking = false;
        lastGridSlot = null;
    }

    public void Undo()
    {
        if (!Sticking) return;

        if (lastGridSlot == null)
        {
            GameDataManager.Instance.AddCoins(selectedItem.price);
            Placed();
        } else 
        {
            lastGridSlot.SelectGrid();
        }

        Sticking = false;
    }
}