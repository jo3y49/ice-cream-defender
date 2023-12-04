using System.Collections;
using UnityEngine;

public class Turret : Placeable
{
    private int direction = 1;
    private float fireRate = 1;
    private float range = 5;

    private Vector2 bulletDirection;
    private Vector2 bulletVelocity;

    private Vector3 firePoint;

    protected override void SetWeapon()
    {
        direction = (int)Mathf.Sign(transform.position.x);

        sr.flipX = direction < 0;

        bulletDirection = direction * Vector2.right;

        bulletVelocity = bulletDirection * (data as TurretData).bulletSpeed;

        fireRate = (data as TurretData).fireRate;

        fireRate = 1/fireRate;

        range = (data as TurretData).range;

        firePoint = transform.position;

        StartCoroutine(ShootAtEnemies());
    }

    private IEnumerator ShootAtEnemies()
    {
        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(firePoint, bulletDirection, range);

            Debug.DrawRay(firePoint, bulletDirection * range, Color.red);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    GameObject bullet = Pool.Instance.GetBullet();
                    bullet.transform.position = firePoint;
                    bullet.SetActive(true);
                    bullet.GetComponent<Bullet>().Fire(bulletVelocity);
                }
            }

            yield return new WaitForSeconds(fireRate);
        }
    }
}