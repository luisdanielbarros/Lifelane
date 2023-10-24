using UnityEngine;
using UnityEngine.EventSystems;
public class invDragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField]
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            inventorySlot droppedItem = eventData.pointerDrag.GetComponent<inventorySlot>();
            if (droppedItem != null)
            {
                inventorySlot currentItem = GetComponent<inventorySlot>();
                itemData currentItemClone = currentItem.slotItem;
                currentItem.changeItem(droppedItem.slotItem);
                droppedItem.changeItem(currentItemClone);
            }
        }
    }
}
