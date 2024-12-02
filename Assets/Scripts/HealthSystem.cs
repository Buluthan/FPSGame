using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
 public int maxHealth = 100; // Maksimum can
 public int damage = 5;
    private int currentHealth; // Mevcut can

    public bool isPlayer; // Oyuncu mu? (true) Yoksa düşman mı? (false)

    void Start()
    {
        currentHealth = maxHealth; // Oyuncu/düşman tam canla başlar
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Can maksimumu aşmasın
        }
        Debug.Log(gameObject.name + " healed for " + amount + ". Current health: " + currentHealth);
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " died!");
        
        if (isPlayer)
        {
            // Oyuncu öldüğünde oyunu durdur veya yeniden başlat
            Debug.Log("Game Over!");
            // Örneğin: Time.timeScale = 0;
        }
        else
        {
            // Düşman öldüğünde nesneyi yok et
            Destroy(gameObject);
        }
    }
}