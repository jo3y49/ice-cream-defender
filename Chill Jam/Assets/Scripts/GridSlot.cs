using UnityEngine;

public class GridSlot : MonoBehaviour {
    [SerializeField] private Turret turret;
    [SerializeField] private Trap trap;

    private Placeable activeItem;

    public void Placed()
    {
        if (!MouseObject.Instance.Sticking || activeItem != null) return;

        MouseObject.Instance.Placed();

        Placeable item = null;

        switch (MouseObject.Instance.selectedItem)
        {
            case TurretData:
                item = turret;
                break;
            default:
                return;
        }

        activeItem = Instantiate(item, transform);
        activeItem.Initialize(MouseObject.Instance.selectedItem);
    }
    
}