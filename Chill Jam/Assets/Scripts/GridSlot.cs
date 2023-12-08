using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GridSlot : MonoBehaviour {
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject turretPrefab, trapPrefab, sundaePrefab, gridHighlight;
    private Turret turret;
    private Wall trap;
    private Bomb sundae;

    public Placeable activeItem;

    private void Start() {
        gridHighlight.SetActive(false);

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
        if (Time.timeScale == 0) return;

        if (!MouseObject.Instance.Sticking)
        {
            UIManager.Instance.SetItem(this);
            return;

        } else if (activeItem != null) return;

        MouseObject.Instance.Placed();

        SetItem(MouseObject.Instance.GetItem());
        
    }

    public void SetItem((PlaceableData, int) item)
    {
        switch (item.Item1)
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
        activeItem.Initialize(item.Item1, item.Item2);
    }

    public void Destroyed()
    {
        if (activeItem == null) return;

        activeItem.gameObject.SetActive(false);
        activeItem = null;
    }

    public void ToggleGridHighlight(bool b)
    {
        gridHighlight.SetActive(b);
    }
}