using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GridSlot : MonoBehaviour {
    [SerializeField] private Image image;
    [SerializeField] private GameObject turretPrefab, trapPrefab, sundaePrefab;
    private Turret turret;
    private Wall trap;
    private Bomb sundae;

    public Placeable activeItem;

    private void Start() {
        turret = (Turret)CreateItem(turretPrefab);

        trap = (Wall)CreateItem(trapPrefab);

        sundae = (Bomb)CreateItem(sundaePrefab);
    }

    private Placeable CreateItem(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        Placeable item = obj.GetComponent<Placeable>();
        item.SetUp(this);
        item.gameObject.SetActive(false);

        return item;
    }

    public void SelectGrid()
    {
        if (!MouseObject.Instance.Sticking && Time.timeScale == 1)
        {
            UIManager.Instance.SetItem(this);
            return;

        } else if (activeItem != null) return;

        MouseObject.Instance.Placed();

        SetItem(MouseObject.Instance.selectedItem);
        
    }

    public void SetItem(PlaceableData item)
    {
        switch (item)
        {
            case TurretData:
                activeItem = turret;
                break;
            case WallData:
                activeItem = trap;
                break;
            case BombData:
                activeItem = sundae;
                break;
            default:
                return;
        }

        activeItem.gameObject.SetActive(true);
        activeItem.Initialize(item);
    }

    public void Destroyed()
    {
        if (activeItem == null) return;

        activeItem.gameObject.SetActive(false);
        activeItem = null;
    }
}