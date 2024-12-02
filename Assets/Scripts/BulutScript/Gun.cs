using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint; // Merminin ��k�� noktas�
    public GameObject bulletPrefab; // Mermi prefab'�
    public float bulletSpeed = 500f; // Mermi h�z�
    public float fireRate = 15f; // Ate� etme h�z�

    public AudioClip fireSound; // Ate� etme sesi dosyas�
    private AudioSource audioSource; // AudioSource bile�eni

    private float nextFireTime = 0f; // Bir sonraki ate� etme zaman�

    private void Start()
    {
        // ... (di�er kodlar)

        audioSource = GetComponent<AudioSource>(); // AudioSource bile�enini al
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
            audioSource.volume = 0.1f;
        }
    }

    private void Shoot()

    {
        // Mermiyi olu�tur
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = -firePoint.forward * bulletSpeed;

        // Ate� etme sesini oynat
        audioSource.PlayOneShot(fireSound);

        

    }
}
