using UnityEngine;
using System;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;// Hollow Knight Reference
    private int currentHealth;
    
    public HealthUI healthUI;
    private SpriteRenderer spriteRenderer;
    
    public static event Action OnPlayerDied; 

    void Start()
    {
        ResetHealth();

        spriteRenderer = GetComponent<SpriteRenderer>();
        GameController.OnReset += ResetHealth;
        HealthItem.OnHealthCollect += Heal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy)
        {
            TakeDamage(enemy.damage);
            SoundManager.Play("PlayerHit");
        }
        Trap trap = collision.GetComponent<Trap>();
        if(trap && trap.damage > 0)
        {
            TakeDamage(trap.damage);
            SoundManager.Play("PlayerHit");
        }
        else if (trap)
        {
            SoundManager.Play("Bounce");
        }
    }

    void Heal(int amount)
    {
        currentHealth += amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthUI.UpdateHearts(currentHealth);
    }

    void ResetHealth()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthUI.UpdateHearts(currentHealth);

        StartCoroutine(FlashRed());

        if(currentHealth <= 0)
        {
            // zdoh
            OnPlayerDied.Invoke();
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

}
