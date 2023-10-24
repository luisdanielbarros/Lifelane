using System;
using UnityEngine;
[Serializable]
public class itemData
{
    //Item Id
    protected string itemid;
    public string itemId { get { return itemid; } }
    //Name
    protected string name;
    public string Name { get { return name; } }
    //Description
    protected string description;
    public string Description { get { return description; } }
    //Sprite
    protected Sprite itemicon;
    public Sprite itemIcon { get { return itemicon; } }
    //Type
    protected itemDataType type;
    public itemDataType Type { get { return type; } }
    //Rarity
    protected itemRarity rarity;
    public itemRarity Rarity { get { return rarity; } }
    //Slot Space
    protected int slotspace;
    public int slotSpace { get { return slotspace; } }
    //Droppable
    protected bool droppable;
    public bool Droppable { get { return droppable; } }
    //Quantity
    protected int quantity;
    public int Quantity { get { return quantity; } set { quantity = value; } }
    //Constructor
    public itemData(string _itemId, string _Name, string _Description, Sprite _itemIcon, itemDataType _Type, itemRarity _Rarity, int _slotSpace)
    {
        itemid = _itemId;
        name = _Name;
        description = _Description;
        itemicon = _itemIcon;
        type = _Type;
        rarity = _Rarity;
        slotspace = _slotSpace;
        if (Type == itemDataType.Collectible) droppable = false;
        else droppable = true;
        quantity = 0;
    }
}
