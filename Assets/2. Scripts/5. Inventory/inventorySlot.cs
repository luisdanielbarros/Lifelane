using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class inventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Item
    [SerializeField]
    private itemData slotitem;
    public itemData slotItem { get { return slotitem; } set { changeItem(value); } }
    //Empty
    public bool isEmpty { get; set; }
    //Occupied
    public bool isBoxed { get; set; }
    //Quantity
    [SerializeField]
    private int quantity;
    public int Quantity { get { return quantity; } set { changeQuantity(value); } }
    //Space
    private int slotspace;
    public int slotSpace { get { return slotspace; } set { slotspace = value; } }
    //Variables above
    //Functions below
    //Change Item
    public void changeItem(itemData _slotItem)
    {
        slotitem = _slotItem;
        Image UIIcon = GetComponent<Image>();
        if (UIIcon != null)
        {
            if (slotitem == null) UIIcon.sprite = null;
            else UIIcon.sprite = slotitem.itemIcon;
        }
    }
    //Calculate Slot Space
    public void calcSlotSpace()
    {
        slotspace = slotspace * quantity;
    }
    //Increment Quantity
    public void incrementQuantity(int _Quantity)
    {
        quantity += _Quantity;
        if (quantity < 0) quantity = 0;
        calcSlotSpace();
    }
    //Change Quantity
    private void changeQuantity(int _Quantity)
    {
        quantity = _Quantity;
        calcSlotSpace();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryManager.Instance.viewItem(slotitem, transform);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryManager.Instance.hideItemViwer();
    }
}
