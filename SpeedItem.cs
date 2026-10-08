using UnityEngine;
using System;

public class SpeedItem : Item
{
    public static event Action<float> OnSpeedCollected;
    public float speedMultiplier = 1.5f;

    public override bool UseItem()
    {
        base.UseItem();
        OnSpeedCollected?.Invoke(speedMultiplier);
        return true;
    }
}