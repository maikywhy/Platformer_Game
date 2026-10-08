using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections; // Обов'язково для роботи корутин

public class PlayerAttack : MonoBehaviour
{
    [Header("Налаштування Атаки")]
    public Transform attackPoint; 
    public float attackRange = 0.5f; 
    public int attackDamage = 1;
    public LayerMask enemyLayers; 

    [Header("Налаштування Затримки (Cooldown)")]
    public float attackRate = 2f; 
    private float nextAttackTime = 0f;

    // Змінна для керування картинкою
    private SpriteRenderer attackSprite;

    private void Start()
    {
        // Знаходимо SpriteRenderer на об'єкті AttackPoint
        if (attackPoint != null)
        {
            attackSprite = attackPoint.GetComponent<SpriteRenderer>();
            
            // Примусово вимикаємо картинку на старті гри, щоб вона не висіла перед гравцем
            if (attackSprite != null)
            {
                attackSprite.enabled = false;
            }
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.time >= nextAttackTime)
            {
                PerformAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    private void PerformAttack()
    {
        // 1. Запускаємо показ картинки
        if (attackSprite != null)
        {
            // Зупиняємо попередню корутину, якщо гравець б'є дуже швидко, щоб картинка не блимала
            StopAllCoroutines(); 
            StartCoroutine(ShowAttackVisual());
        }

        // 2. Математично перевіряємо зіткнення і віднімаємо здоров'я
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    }

    // Корутина для керування видимістю SpriteRenderer
    private IEnumerator ShowAttackVisual()
    {
        // Вмикаємо картинку (робимо видимою)
        attackSprite.enabled = true;

        // Чекаємо 0.15 секунди (можеш змінити цей час під свою анімацію)
        yield return new WaitForSeconds(0.15f);

        // Вимикаємо картинку (робимо невидимою)
        attackSprite.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}