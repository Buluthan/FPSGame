using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f; // Merminin vereceði hasar
    

   
    private void OnCollisionEnter(Collision collision)
    {
        // Ýsabet kontrolü
        GhostHealth ghostHealth = collision.gameObject.GetComponent<GhostHealth>();
        if (ghostHealth != null)
        {
            ghostHealth.TakeDamage(damage);
        }

        // Mermiyi hemen yok et
        Destroy(gameObject);
        
    }
}




