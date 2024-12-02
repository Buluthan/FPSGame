using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint; // Merminin çýkýþ noktasý
    public GameObject bulletPrefab; // Mermi prefab'ý
    public float bulletSpeed = 500f; // Mermi hýzý
    public float fireRate = 15f; // Ateþ etme hýzý

    private float nextFireTime = 0f; // Bir sonraki ateþ etme zamaný

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
        // Mermiyi oluþtur
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Mermiye hýz ver
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = -firePoint.forward * bulletSpeed;

    }
}
