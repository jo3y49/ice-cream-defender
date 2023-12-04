using UnityEngine;
using System.Collections;

public class MouseObject : MonoBehaviour {
    public static MouseObject Instance;
    [SerializeField] private GameObject itemOnMousePrefab;
    public PlaceableData selectedItem {get; private set;}
    public GameObject itemObject {get; private set;}
    private SpriteRenderer itemObjectRenderer;

    public bool Sticking {get; private set;} = false;

    private void Awake() {
        Instance = this;

        itemObject = Instantiate(itemOnMousePrefab);
        TryGetComponent(out itemObjectRenderer); 

        itemObject.SetActive(false);
    }

    public void StickToMouse(PlaceableData item)
    {
        StopAllCoroutines();

        StartCoroutine(StickingToMouse(item));
    }

    private IEnumerator StickingToMouse(PlaceableData item)
    {
        itemObject.SetActive(true);
        if (item.sprite != null) itemObjectRenderer.sprite = item.sprite;

        selectedItem = item;

        Sticking = true;

        while (true)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
            itemObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);

            yield return null;
        }
    }

    public void Placed()
    {
        StopAllCoroutines();

        Sticking = false;

        itemObject.SetActive(false);
    }
}