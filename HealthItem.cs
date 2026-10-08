using UnityEngine;
using System;

public class HealthItem : Item
{
    public int healAmount = 1;
    public static event Action<int> OnHealthCollect;

    public override bool UseItem()
    {
        base.UseItem();
        OnHealthCollect?.Invoke(healAmount);
        return true;
    }
}