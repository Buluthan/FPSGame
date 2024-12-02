using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; // Hayaletin maksimum can de�eri
    public float currentHealth; // Hayaletin �u anki can de�eri

    [SerializeField] FloatingHealthBar healthBar;

    private void Awake() 
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    private void Start()
    {
        currentHealth = maxHealth; // Ba�lang��ta can de�erini maksimuma ayarla
        healthBar.UpdateHealthBar(currentHealth,maxHealth);
    }

    public void TakeDamage(float damage) // Hayalete hasar vermek i�in �a�r�lacak fonksiyon
    {
        currentHealth -= damage; // Can de�erini azalt
        healthBar.UpdateHealthBar(currentHealth,maxHealth);

        if (currentHealth <= 0) // Can de�eri 0 veya alt�na d��erse
        {
            Die(); // �l�m fonksiyonunu �a��r
        }
    }

    private void Die() // Hayalet �ld���nde �a�r�lacak fonksiyon
    {
        // �l�m animasyonu veya efekti oynat (e�er varsa)
        // ...

        // Hayaleti yok et
        Destroy(gameObject);
    }
}
