using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
 public Transform player; // Oyuncunun transformu
    public float detectionRange = 10f; // Oyuncuyu algılama mesafesi
    public float attackRange = 2f; // Saldırı mesafesi
    public float moveSpeed = 3f; // Hareket hızı
    public float attackCooldown = 1.5f; // Saldırılar arasındaki bekleme süresi

    private Animator animator; // Animator referansı
    private bool isDead = false; // Ölüm durumu kontrolü
    private float nextAttackTime = 0f; // Saldırılar arasındaki zaman kontrolü

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return; // Eğer ölü ise diğer işlemleri yapma

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            TryAttack();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            SetIdle();
        }
    }

    void MoveTowardsPlayer()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isAttacking", false);

        // Oyuncuya doğru hareket
        Vector3 direction = (player.position - transform.position).normalized;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void TryAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttacking", true);
            nextAttackTime = Time.time + attackCooldown; // Saldırı zamanını güncelle
        }
    }

    void SetIdle()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isAttacking", false);
    }

    public void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        Destroy(gameObject, 2f);
    }
}
