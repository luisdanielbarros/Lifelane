public class interactableRegular : Interactable
{
    protected string interactableName;
    public string Name { get { return interactableName; } }
    public interactableRegular(string[] _interactionDialogue, string _interactableName) : base(_interactionDialogue)
    {
        type = interactableType.Regular;
        interactableName = _interactableName;
    }
    public interactableRegular(string[] _interactionDialogue, interactionImgLibEntry[][] _interactionImages, soundLibEntry[][] _Music, string _interactableName) : base(_interactionDialogue, _interactionImages, _Music)
    {
        type = interactableType.Regular;
        interactableName = _interactableName;
    }
}
