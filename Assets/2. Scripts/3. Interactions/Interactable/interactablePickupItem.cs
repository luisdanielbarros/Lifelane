public class interactablePickupItem : Interactable
{
    //Item Id
    private string itemid;
    public string itemId { get { return itemid; } }
    //Music
    private soundLibEntry music;
    public soundLibEntry Music { get { return music; } }
    //Pickup Music
    private soundLibEntry pickupmusic;
    public soundLibEntry pickupMusic { get { return pickupmusic; } }
    //Effect
    private particleEffect effect;
    public particleEffect Effect { get { return effect; } }
    public interactablePickupItem(string[] _interactionDialogue, string _itemId, soundLibEntry _Music, soundLibEntry _pickupMusic, particleEffect _Effect) : base(_interactionDialogue)
    {
        type = interactableType.PickupItem;
        itemid = _itemId;
        music = _Music;
        pickupmusic = _pickupMusic;
        effect = _Effect;
    }
}
