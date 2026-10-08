using UnityEngine;
using UnityEngine.InputSystem;

public class HotBarController : MonoBehaviour
{
    public GameObject hotBarPanel;
    public int slotCount = 10; 
    private Key[] hotbarKeys;

    private void Awake()
    {
        hotbarKeys = new Key[slotCount];
        for(int i = 0; i < slotCount; i++)
        {
            hotbarKeys[i] = i < 9 ? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }
    }

    void Update()
    {
        for(int i = 0; i < slotCount; i++)
        {
            if (i < hotBarPanel.transform.childCount)
            {
                if(Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
                {
                    UseItemInSlot(i);
                }
            }
        }
    }

    void UseItemInSlot(int index)
    {
        Slot slot = hotBarPanel.transform.GetChild(index).GetComponent<Slot>();
        if(slot != null && slot.currentItem != null)
        {
            Item item = slot.currentItem.GetComponent<Item>();
            
            if (item != null)
            {
                bool isConsumed = item.UseItem(); 

                if (isConsumed)
                {
                    Destroy(slot.currentItem);
                    slot.currentItem = null; 
                }
            }
        }
    }
}