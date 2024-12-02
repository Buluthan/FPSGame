using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    public float currentHealth;

    public HealthBar healthBar; // Saðlýk barý referansý

    private void Start()
    {
        currentHealth = maxHealth;

        // Saðlýk barýný baþlat
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Saðlýk barýný güncelle
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Ölüm animasyonu veya efekti oynat
        // ...

        // Objenin yok edilmesi veya devre dýþý býrakýlmasý
        Destroy(gameObject);
    }
}
