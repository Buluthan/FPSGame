using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostAttack : MonoBehaviour
{
  public Transform player; // Oyuncu karakterinin transform'u
    public float agroDistance = 10f; // Algılama mesafesi
    public float attackDistance = 2f; // Saldırı mesafesi
    public float attackDamage = 10f; // Saldırı hasarı
    public float attackRate = 1f; // Saldırı hızı

    private NavMeshAgent agent; // Hayaletin Nav Mesh Agent bileşeni
    private Animator animator; // Hayaletin Animator bileşeni
    private float nextAttackTime = 0f; // Bir sonraki saldırı zamanı
    private bool isAgro = false; // Hayaletin saldırı modunda olup olmadığını kontrol eder

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Oyuncuya olan mesafeyi hesapla
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Agro mekanizması: Hayalet yalnızca agroDistance içinde hareket eder
        if (distanceToPlayer <= agroDistance)
        {
            isAgro = true;
        }
        else
        {
            isAgro = false;
            agent.SetDestination(transform.position); // Hareketi durdur
            animator.SetBool("IsMoving", false); // Hareket animasyonunu durdur
        }

        if (isAgro)
        {
            if (distanceToPlayer <= attackDistance)
            {
                // Oyuncuya bak
                transform.LookAt(player);

                // Saldırı zamanını kontrol et
                if (Time.time >= nextAttackTime)
                {
                    // Saldırı animasyonunu oynat
                    animator.SetTrigger("Attack");

                    // Oyuncuya hasar ver
                    player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

                    // Bir sonraki saldırı zamanı
                    nextAttackTime = Time.time + 1f / attackRate;
                }

                // Hareket animasyonunu durdur
                animator.SetBool("IsMoving", false);
                agent.SetDestination(transform.position); // Hareketi durdur
            }
            else
            {
                // Oyuncuya doğru hareket et
                agent.SetDestination(player.position);

                // Hareket animasyonunu oynat
                animator.SetBool("IsMoving", true);
            }
        }
    }
}
