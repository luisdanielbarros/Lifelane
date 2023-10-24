public class interactableCutscene : Interactable
{
    public interactableCutscene(string[] _interactionDialogue) : base(_interactionDialogue)
    {
        type = interactableType.Cutscene;
    }
}
