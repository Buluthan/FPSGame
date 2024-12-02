using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f; // Merminin verece�i hasar
    

   
    private void OnCollisionEnter(Collision collision)
    {
        // �sabet kontrol�
        GhostHealth ghostHealth = collision.gameObject.GetComponent<GhostHealth>();
        if (ghostHealth != null)
        {
            ghostHealth.TakeDamage(damage);
        }

        // Mermiyi hemen yok et
        Destroy(gameObject);
        
    }
}




