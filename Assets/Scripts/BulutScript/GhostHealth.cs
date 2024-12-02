using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; // Hayaletin maksimum can deðeri
    public float currentHealth; // Hayaletin þu anki can deðeri

    private void Start()
    {
        currentHealth = maxHealth; // Baþlangýçta can deðerini maksimuma ayarla
    }

    public void TakeDamage(float damage) // Hayalete hasar vermek için çaðrýlacak fonksiyon
    {
        currentHealth -= damage; // Can deðerini azalt

        if (currentHealth <= 0) // Can deðeri 0 veya altýna düþerse
        {
            Die(); // Ölüm fonksiyonunu çaðýr
        }
    }

    private void Die() // Hayalet öldüðünde çaðrýlacak fonksiyon
    {
        // Ölüm animasyonu veya efekti oynat (eðer varsa)
        // ...

        // Hayaleti yok et
        Destroy(gameObject);
    }
}
