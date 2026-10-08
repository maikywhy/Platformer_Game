using UnityEngine;
using UnityEngine.UI;

public interface ICollectable
{
    void Collect();
}

public class Item : MonoBehaviour, ICollectable
{
    [Header("Item Settings")]
    public int ID;
    public string Name;

    public virtual void Collect()
    {
        SoundManager.Play("ItemPickUp");
        Sprite itemIcon = GetComponent<SpriteRenderer>()?.sprite;
        if (itemIcon == null) itemIcon = GetComponent<Image>()?.sprite;

        if (ItemPickUpUi.Instance != null && itemIcon != null)
        {
            ItemPickUpUi.Instance.ShowItemPickUp(Name, itemIcon);
        }

        Debug.Log("Picked up: " + Name);
    }

    public virtual bool UseItem()
    {
        Debug.Log("Using abstract item: " + Name);
        return false; 
    }
}