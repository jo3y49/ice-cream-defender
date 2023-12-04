using UnityEngine;

public class GridSlot : MonoBehaviour {
    [SerializeField] private Turret turret;
    [SerializeField] private Trap trap;

    public void Placed()
    {
        if (!MouseObject.Instance.Sticking) return;

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

        Placeable newItem = Instantiate(item, transform);
        newItem.Initialize(MouseObject.Instance.selectedItem);
    }
    
}