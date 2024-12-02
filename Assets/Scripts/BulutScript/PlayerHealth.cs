using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    public float currentHealth;

    public HealthBar healthBar; // Sa�l�k bar� referans�

    private void Start()
    {
        currentHealth = maxHealth;

        // Sa�l�k bar�n� ba�lat
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Sa�l�k bar�n� g�ncelle
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
        // �l�m animasyonu veya efekti oynat
        // ...

        // Objenin yok edilmesi veya devre d��� b�rak�lmas�
        Destroy(gameObject);
    }
}
