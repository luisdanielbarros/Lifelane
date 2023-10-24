using TMPro;
using UnityEngine;

public class inventoryManager : MonoBehaviour
{
    //Instance
    private static inventoryManager instance = null;
    public static inventoryManager Instance { get { return instance; } }
    //Slots
    private int freeSpace = 4;
    private int itemNumber = 0, boxNumber = 0;
    public GameObject[] inventorySlots;
    //Item Viewer
    [SerializeField]
    private RectTransform itemViewer;
    [SerializeField]
    private RectTransform itemContainer;
    [SerializeField]
    private GameObject[] itemWeaponProperties;
    [SerializeField]
    private TextMeshProUGUI itemName;
    [SerializeField]
    private TextMeshProUGUI itemType;
    [SerializeField]
    private TextMeshProUGUI itemDescription;
    [SerializeField]
    private TextMeshProUGUI itemRange;
    [SerializeField]
    private TextMeshProUGUI itemAttack;
    [SerializeField]
    private TextMeshProUGUI itemDefense;
    [SerializeField]
    private TextMeshProUGUI itemSpeed;
    private bool isMouseOver = false;
    void Awake()
    {
        //Instatiate
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (isMouseOver)
        {
            Vector3 mousePosition = Input.mousePosition;
            itemContainer.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        }
    }
    //Add Item
    public bool addItem(inventorySlot _Item)
    {
        int neededSpace = _Item.slotSpace;
        //If there's space available
        if (freeSpace >= neededSpace)
        {
            freeSpace -= neededSpace;
            itemNumber += 1;
            if (neededSpace > 1) boxNumber += neededSpace - 1;
            bool alreadyExists = false;
            int firstEmptySlot = -1;
            //If the item already exists, stack it
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                inventorySlot currentSlot = inventorySlots[i].GetComponent<inventorySlot>();
                //Get the first empty slot found, in case the item doesn't exist
                if (string.IsNullOrEmpty(currentSlot.slotItem.Name))
                {
                    if (firstEmptySlot == -1) firstEmptySlot = 0;
                    continue;
                }
                else if (currentSlot.slotItem.Name == _Item.slotItem.Name)
                {
                    alreadyExists = true;
                    currentSlot.incrementQuantity(_Item.Quantity);
                    break;
                }
            }
            //Else add it
            if (!alreadyExists)
            {
                inventorySlot newSlot = inventorySlots[firstEmptySlot].GetComponent<inventorySlot>();
                newSlot.changeItem(_Item.slotItem);
            }
            return true;
        }
        else return false;
    }
    //Remove Item
    public bool removeItem(inventorySlot _Item)
    {
        //If the item's droppable, drop it
        if (_Item.slotItem.Droppable)
        {
            freeSpace += _Item.slotSpace;
            itemNumber -= 1;
            if (_Item.slotSpace > 1) boxNumber -= _Item.slotSpace - 1;
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                inventorySlot currentSlot = inventorySlots[i].GetComponent<inventorySlot>();
                if (currentSlot == null) continue;
                if (currentSlot.slotItem.Name == _Item.slotItem.Name)
                {
                    inventorySlots[i] = null;
                    break;
                }
            }
            return true;
        }
        //Else forbid the player
        else return false;
    }
    //Item Viewer
    #region Item Viewer
    public void viewItem(itemData _item, Transform _toParent)
    {
        isMouseOver = true;
        itemViewer.gameObject.SetActive(true);
        //General
        itemName.SetText(_item.Name);
        itemType.SetText(_item.Type.ToString());
        itemDescription.SetText(_item.Description);
        switch (_item.Type)
        {
            //Weapon
            case itemDataType.Weapon:
                for (int i = 0; i < itemWeaponProperties.Length; i++) itemWeaponProperties[i].SetActive(true);
                weaponData _itemWeapon = (weaponData)_item;
                itemRange.SetText(_itemWeapon.Range.ToString());
                itemAttack.SetText(_itemWeapon.Attack.ToString());
                itemDefense.SetText(_itemWeapon.Defense.ToString());
                itemSpeed.SetText(_itemWeapon.Speed.ToString());
                break;
            default:
                for (int i = 0; i < itemWeaponProperties.Length; i++) itemWeaponProperties[i].SetActive(false);
                break;
        }
    }
    public void hideItemViwer()
    {
        isMouseOver = false;
        itemViewer.gameObject.SetActive(false);
    }
    #endregion
}
