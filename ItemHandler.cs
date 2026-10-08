using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if(canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; 
        transform.SetParent(transform.root); 
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; 
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();

        if (dropSlot == null)
        {
            GameObject item = eventData.pointerEnter;
            if(item != null)
            {
                dropSlot = item.GetComponentInParent<Slot>();
            }
        }
        
        Slot ogSlot = originalParent.GetComponent<Slot>();

        if (dropSlot != null)
        {
            if (dropSlot.currentItem != null && dropSlot.currentItem != gameObject) 
            {
                dropSlot.currentItem.transform.SetParent(ogSlot.transform);
                ogSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else if (dropSlot.currentItem != gameObject)
            {
                ogSlot.currentItem = null;
            }

            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            transform.SetParent(originalParent); 
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}