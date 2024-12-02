using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint; // Merminin ��k�� noktas�
    public GameObject bulletPrefab; // Mermi prefab'�
    public float bulletSpeed = 500f; // Mermi h�z�
    public float fireRate = 15f; // Ate� etme h�z�

    private float nextFireTime = 0f; // Bir sonraki ate� etme zaman�

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    private void Shoot()

    {
        // Mermiyi olu�tur
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Mermiye h�z ver
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = -firePoint.forward * bulletSpeed;

    }
}
