using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostAttack : MonoBehaviour
{
    public Transform player; // Oyuncu karakterinin transform'u
    public float attackDistance = 2f; // Sald�r� mesafesi
    public float attackDamage = 10f; // Sald�r� hasar�
    public float attackRate = 1f; // Sald�r� h�z�

    private NavMeshAgent agent; // Hayaletin Nav Mesh Agent bile�eni
    private Animator animator; // Hayaletin Animator bile�eni
    private float nextAttackTime = 0f; // Bir sonraki sald�r� zaman�

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Oyuncuya olan mesafeyi hesapla
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Sald�r� mesafesindeyse
        if (distanceToPlayer <= attackDistance)
        {
            // Oyuncuya bak
            transform.LookAt(player);

            // Sald�r� zaman� geldiyse
            if (Time.time >= nextAttackTime)
            {
                // Sald�r� animasyonunu oynat
                animator.SetTrigger("Attack_Shift"); // "attack_shift" trigger'�n� kullan

                // Oyuncuya hasar ver
                player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

                // Bir sonraki sald�r� zaman�n� ayarla
                nextAttackTime = Time.time + 1f / attackRate;
            }

            // Hareket animasyonunu durdur (e�er varsa)
            // animator.SetBool("IsMoving", false); // "IsMoving" parametresini false yap
        }
        else
        {
            // Oyuncuya do�ru hareket et
            agent.SetDestination(player.position);

            // Hareket animasyonunu oynat (e�er varsa)
            // animator.SetBool("IsMoving", true); // "IsMoving" parametresini true yap
        }
    }
}
