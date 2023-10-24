using System;
[Serializable]
public class storedItemData
{
    //Item Id
    protected string itemid;
    public string itemId { get { return itemid; } set { itemid = value; } }
    //Quantity
    protected int quantity;
    public int Quantity { get { return quantity; } set { quantity = value; } }
    //Constructor
    public storedItemData(string _itemId, int _Quantity)
    {
        itemid = _itemId;
        quantity = _Quantity;
    }
}
