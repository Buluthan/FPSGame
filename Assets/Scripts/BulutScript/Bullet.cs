using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f; // Merminin vereceði hasar
    

   
    private void OnCollisionEnter(Collision collision)
    {
        // Ýsabet kontrolü
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        // Mermiyi hemen yok et
        Destroy(gameObject);
        
    }
}




