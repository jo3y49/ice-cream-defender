using UnityEngine;

[CreateAssetMenu(fileName = "Turret", menuName = "Placeable/Turret")]
public class TurretData : PlaceableData {
    [Range(.5f, 10f)]
    public float fireRate = 1;

    [Range(1, 50)]
    public float bulletSpeed = 1;
    public float range = 5;
}