using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GridSlot : MonoBehaviour {
    [SerializeField] private Image image;
    [SerializeField] private GameObject turretPrefab, trapPrefab;
    private Turret turret;
    private Trap trap;

    private Placeable activeItem;

    private void Start() {
        turret = (Turret)CreateItem(turretPrefab);

        trap = (Trap)CreateItem(trapPrefab);
    }

    private Placeable CreateItem(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        Placeable item = obj.GetComponent<Placeable>();
        item.SetUp(this);
        item.gameObject.SetActive(false);

        return item;
    }

    public void Placed()
    {
        if (!MouseObject.Instance.Sticking || activeItem != null) return;

        MouseObject.Instance.Placed();

        switch (MouseObject.Instance.selectedItem)
        {
            case TurretData:
                activeItem = turret;
                break;
            default:
                return;
        }

        activeItem.gameObject.SetActive(true);
        activeItem.Initialize(MouseObject.Instance.selectedItem);
    }

    public void Destroyed()
    {
        activeItem.gameObject.SetActive(false);
        activeItem = null;
    }
    
}