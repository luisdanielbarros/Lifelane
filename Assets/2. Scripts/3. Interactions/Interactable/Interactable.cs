public abstract class Interactable
{
    //Interaction Dialogue
    public Interaction[] interactionDialogue;
    //Type
    protected interactableType type;
    public interactableType Type { get { return type; } }
    public Interactable(string[] _interactionDialogue)
    {
        //Interaction Dialogue
        interactionDialogue = new Interaction[_interactionDialogue.Length];
        for (int i = 0; i < _interactionDialogue.Length; i++)
        {
            interactionDialogue[i] = new Interaction(new Dialogue(_interactionDialogue[i], dialogueType.Normal));
        }
        //Type
        type = interactableType.Regular;
    }
    public Interactable(string[] _interactionDialogue, interactionImgLibEntry[][] _interactionImages, soundLibEntry[][] _Music)
    {
        //Interaction Dialogue
        interactionDialogue = new Interaction[_interactionDialogue.Length];
        for (int i = 0; i < _interactionDialogue.Length; i++)
        {
            interactionDialogue[i] = new Interaction(new Dialogue(_interactionDialogue[i], dialogueType.Normal), _interactionImages[i], _Music[i]);
        }
        //Type
        type = interactableType.Regular;
    }
}
