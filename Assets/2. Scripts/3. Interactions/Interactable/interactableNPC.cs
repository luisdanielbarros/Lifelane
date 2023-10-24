using UnityEngine;
[System.Serializable]

public enum npcStates { Idle, movingToInteraction };
public class interactableNPC : Interactable
{
    //State
    private npcStates npcState = npcStates.Idle;
    //Personal Data
    [SerializeField]
    private characterData personaldata;
    public characterData personalData { get { return personaldata; } }
    //Name
    public string Name { get { return personalData.Name; } }
    //Interaction
    private GameObject currentTarget;
    private Interactable Interaction;
    public interactableNPC(string[] _interactionDialogue) : base(_interactionDialogue)
    {
        type = interactableType.NPC;
        //State
        npcState = npcStates.Idle;
        //Interaction
        //Interaction = GetComponent<Interactable>();
    }
    //Interaction
    public delegate Interaction[] OnInteraction(jsonCharacterData targetCard);
    public OnInteraction onInteraction;
    public void Interact(jsonCharacterData targetCard)
    {
        //interactionDialogue = charInteractionGenerator.Instance.genInteraction(Card, targetCard);
    }
    public void addProgress(int newProgress)
    {
        ////characterMemory System
        ////string playerUniqueId = gameState.Instance.getPlayerUniqueId();
        //string playerUniqueId = "a";
        //bool recognizesPlayer = false;
        //for (int i = 0; i < Card.Memory.Count; i++)
        //{
        //    characterMemory currentCharMemory = Card.Memory.ElementAt(i);
        //    //If the Player has been recognized in the NPC's memory
        //    if (currentCharMemory.uniqueId == playerUniqueId)
        //    {
        //        //Iterate through each progress milestone
        //        for (int j = 0; j < currentCharMemory.charRelations.Length; j++)
        //        {
        //            charRelation currentCharRelation = currentCharMemory.charRelations[j];
        //            //If the current milestone or flag isn't completed, that's where the relationship between the Player and the NPC stands
        //            if (currentCharRelation.Progress < 100)
        //            {
        //                currentCharRelation.Progress += newProgress;
        //                if (currentCharRelation.Progress > 100) currentCharRelation.Progress = 100;
        //                break;
        //            }
        //        }
        //        recognizesPlayer = true;
        //    }
        //}
        ////If the Player is a stranger, store it in the NPC's memory
        //if (!recognizesPlayer) Card.Memory.AddLast(new characterMemory(playerUniqueId));
    }
}
