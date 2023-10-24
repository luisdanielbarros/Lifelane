using System.Runtime.Serialization;
public class StoredItemSerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        storedItemData[] itemData = (storedItemData[])obj;
        for (int i = 0; i < itemData.Length; i++)
        {
            storedItemData currentItem = itemData[i];
            info.AddValue("itemId" + 1, currentItem.itemId);
            info.AddValue("Quantity" + 1, currentItem.Quantity);
        }
    }
    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        storedItemData[] itemData = (storedItemData[])obj;
        for (int i = 0; i < itemData.Length; i++)
        {
            itemData[i].itemId = (string)info.GetValue("itemId" + i, typeof(string));
            itemData[i].Quantity = (int)info.GetValue("Quantity" + i, typeof(int));
        }
        obj = itemData;
        return obj;
    }
}
