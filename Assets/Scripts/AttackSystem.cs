using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
     public float attackRange = 2f; // Saldırı mesafesi
    public int damage = 20; // Verilen hasar
    public float attackCooldown = 1.5f; // Saldırılar arası bekleme süresi
    public Transform attackPoint; // Saldırının merkez noktası
    public LayerMask targetLayer; // Hedefin katmanı (örneğin Player veya Enemy)

    private float lastAttackTime = 0f; // Son saldırı zamanı

    void Update()
    {
        if (CanAttack())
        {
            PerformAttack();
        }
    }

    private bool CanAttack()
    {
        // Eğer bir oyuncuysa, saldırı için tuşa basıldığında tetiklenir
        return Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown;
    }

    public void PerformAttack()
    {
        lastAttackTime = Time.time; // Saldırı zamanını güncelle
        Debug.Log(gameObject.name + " is attacking!");

        // Saldırı noktasındaki hedefleri bul
        Collider[] hitTargets = Physics.OverlapSphere(attackPoint.position, attackRange, targetLayer);

        foreach (Collider target in hitTargets)
        {
            // Hedefin can sistemine zarar ver
            target.GetComponent<HealthSystem>()?.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Saldırı alanını görselleştirmek için Gizmos çizin
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
