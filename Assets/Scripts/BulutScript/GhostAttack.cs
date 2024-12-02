using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostAttack : MonoBehaviour
{
    public Transform player; // Oyuncu karakterinin transform'u
    public float attackDistance = 2f; // Saldýrý mesafesi
    public float attackDamage = 10f; // Saldýrý hasarý
    public float attackRate = 1f; // Saldýrý hýzý

    private NavMeshAgent agent; // Hayaletin Nav Mesh Agent bileþeni
    private Animator animator; // Hayaletin Animator bileþeni
    private float nextAttackTime = 0f; // Bir sonraki saldýrý zamaný

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Oyuncuya olan mesafeyi hesapla
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Saldýrý mesafesindeyse
        if (distanceToPlayer <= attackDistance)
        {
            // Oyuncuya bak
            transform.LookAt(player);

            // Saldýrý zamaný geldiyse
            if (Time.time >= nextAttackTime)
            {
                // Saldýrý animasyonunu oynat
                animator.SetTrigger("Attack_Shift"); // "attack_shift" trigger'ýný kullan

                // Oyuncuya hasar ver
                player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

                // Bir sonraki saldýrý zamanýný ayarla
                nextAttackTime = Time.time + 1f / attackRate;
            }

            // Hareket animasyonunu durdur (eðer varsa)
            // animator.SetBool("IsMoving", false); // "IsMoving" parametresini false yap
        }
        else
        {
            // Oyuncuya doðru hareket et
            agent.SetDestination(player.position);

            // Hareket animasyonunu oynat (eðer varsa)
            // animator.SetBool("IsMoving", true); // "IsMoving" parametresini true yap
        }
    }
}
