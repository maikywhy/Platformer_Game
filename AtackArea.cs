using UnityEngine;
using System.Collections.Generic;

public class AtackArea : MonoBehaviour
{
    private int damage = 1;
    private Collider2D attackCollider;
    private List<Enemy> damagedEnemies = new List<Enemy>();

    private void Awake()
    {
        attackCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        damagedEnemies.Clear(); 
        CheckForEnemies();
    }

    private void Update()
    {
        CheckForEnemies();
    }

    private void CheckForEnemies()
    {
        if (attackCollider == null) return;

        List<Collider2D> hitObjects = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false; 

        attackCollider.OverlapCollider(filter, hitObjects);

        foreach (Collider2D hit in hitObjects)
        {
            if (hit == null || hit.gameObject == this.gameObject) continue;

            if (transform.parent != null)
            {
                if (hit.gameObject == transform.parent.gameObject) continue;
            }

            if (hit.TryGetComponent<Enemy>(out Enemy enemy))
            {
                if (!damagedEnemies.Contains(enemy))
                {
                    enemy.TakeDamage(damage);
                    damagedEnemies.Add(enemy);
                }
            }
        }
    }
}