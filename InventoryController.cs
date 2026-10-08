using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefab;

    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            if (i < itemPrefab.Length && itemPrefab[i] != null)
            {
                GameObject item = Instantiate(itemPrefab[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
                
                if (item.GetComponent<ItemHandler>() == null) item.AddComponent<ItemHandler>();
                if (item.GetComponent<CanvasGroup>() == null) item.AddComponent<CanvasGroup>();
            }
        }
    }

    public bool AddItem(GameObject objectToAdd)
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(objectToAdd, slotTransform);
                newItem.layer = LayerMask.NameToLayer("UI");

                RectTransform rect = newItem.GetComponent<RectTransform>();
                if (rect == null) rect = newItem.AddComponent<RectTransform>();
                
                rect.anchoredPosition = Vector2.zero;
                rect.localScale = Vector3.one;
                rect.sizeDelta = new Vector2(75, 75); 

                Sprite itemSprite = null;
                Color itemColor = Color.white;

                SpriteRenderer sr = newItem.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    itemSprite = sr.sprite;
                    itemColor = sr.color;
                    sr.enabled = false; 
                }
                else
                {
                    Image existingImage = newItem.GetComponent<Image>();
                    if (existingImage != null)
                    {
                        itemSprite = existingImage.sprite;
                        itemColor = existingImage.color;
                    }
                }

                if (itemSprite != null)
                {
                    Image img = newItem.GetComponent<Image>();
                    if (img == null) img = newItem.AddComponent<Image>();
                    
                    img.sprite = itemSprite;
                    img.color = itemColor;
                    img.preserveAspect = true;
                    
                    img.raycastTarget = true; 
                }

                if (newItem.GetComponent<ItemHandler>() == null)
                {
                    newItem.AddComponent<ItemHandler>();
                }
                if (newItem.GetComponent<CanvasGroup>() == null)
                {
                    newItem.AddComponent<CanvasGroup>();
                }

                var colliders = newItem.GetComponents<Collider2D>();
                foreach (var c in colliders) c.enabled = false;
                var rb = newItem.GetComponent<Rigidbody2D>();
                if (rb != null) Destroy(rb);

                slot.currentItem = newItem;
                return true;
            }
        }
        
        Debug.Log("Inventory is full");
        return false;
    }
}