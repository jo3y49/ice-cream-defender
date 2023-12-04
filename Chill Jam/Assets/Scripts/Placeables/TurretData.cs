using UnityEngine;

[CreateAssetMenu(fileName = "Turret", menuName = "Placeable/Turret")]
public class TurretData : PlaceableData {
    public float fireRate = 1;
    public float bulletSpeed = 1;
    public float range = 5;
}