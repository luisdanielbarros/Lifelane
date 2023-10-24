using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    //Instance
    private static Journal instance = null;
    public static Journal Instance { get { return instance; } }
    //Inventory Slots
    private List<itemData> inventorySlots = new List<itemData>();
    //Prefab & Container
    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private Transform prefabContainer;
    //UI
    [SerializeField]
    private TextMeshProUGUI Title;
    [SerializeField]
    private TextMeshProUGUI Body;
    [SerializeField]
    private TextMeshProUGUI Author;
    //Pagination Component
    [SerializeField]
    private collectiblesItemViewerMenu viewerMenu;
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
    #region Inventory Management
    //Add Item
    public void addItem(itemData _Item)
    {
        bool alreadyExists = false;
        //If the item already exists, stack it
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            itemData currentSlot = inventorySlots[i];
            if (currentSlot.itemId == _Item.itemId)
            {
                alreadyExists = true;
                currentSlot.Quantity = currentSlot.Quantity + 1;
                break;
            }
        }
        //Else add it
        if (!alreadyExists) inventorySlots.Add(_Item);
        updateUI();
    }
    //Remove Item
    public void removeItem(itemData _Item)
    {
        inventorySlots.Remove(_Item);
        updateUI();
    }
    //Decrement Item
    public void decrementItem(itemData _Item)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            itemData currentSlot = inventorySlots[i];
            if (currentSlot.itemId == _Item.itemId)
            {
                currentSlot.Quantity = currentSlot.Quantity - 1;
                break;
            }
        }
        updateUI();
    }
    #endregion
    #region Interface
    //Update UI
    private void updateUI()
    {
        //Clear all the previous Instatiated Prefabs
        foreach (Transform slot in prefabContainer)
        {
            if (slot.name == "Open Collectible Button(Clone)") Destroy(slot.gameObject);
        }
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            itemData currentSlot = inventorySlots[i];
            //Instatiate & Parent Button Prefab
            GameObject newSlot = Instantiate(slotPrefab);
            newSlot.transform.SetParent(prefabContainer.transform, false);
            //Update the Button's Text
            newSlot.GetComponentInChildren<TextMeshProUGUI>().SetText(currentSlot.Name);
            //Add the Event Listeners
            newSlot.GetComponent<Button>().onClick.AddListener(() =>
            {
                loadNote(currentSlot.itemId);
                openNote();
            });
            newSlot.SetActive(true);
        }
    }
    #endregion
    #region Item Viewing
    //Open Note
    public void loadNote(string itemId)
    {
        jsonNote Note = jsonLoader.Instance.loadNote(itemId);
        Title.SetText(Note.title);
        Body.SetText(Note.body);
        Author.SetText(Note.author);
        viewerMenu.resetPagination();
    }
    //Open Note
    public void openNote()
    {
        gameState.Instance.setGameState(gameStates.collectiblesInventory);
    }
    //Clear Nore
    public void clearNote()
    {
        Title.SetText("");
        Body.SetText("");
        Author.SetText("");
        viewerMenu.resetPagination();
    }
    #endregion
    #region Save Data
    public storedItemData[] getSaveData()
    {
        storedItemData[] storedItems = new storedItemData[inventorySlots.Count];
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            itemData currentSlot = inventorySlots[i];
            storedItems[i] = new storedItemData(currentSlot.itemId, currentSlot.Quantity);
        }
        return storedItems;
    }
    public void loadSaveData(storedItemData[] _Items)
    {
        for (int i = 0; i < _Items.Length; i++)
        {
            storedItemData _Item = _Items[i];
            itemData newItem = jsonLoader.Instance.loadItemData(_Item.itemId);
            newItem.Quantity = _Item.Quantity;
            addItem(newItem);
        }
    }
    #endregion
}
