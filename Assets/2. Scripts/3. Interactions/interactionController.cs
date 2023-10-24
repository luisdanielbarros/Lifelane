using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class interactionController : MonoBehaviour
{
    //Instance
    private static interactionController instance = null;
    public static interactionController Instance { get { return instance; } }
    //UI
    [SerializeField]
    private GameObject dialoguePanel, tipPanel, imagePanel, imageLabelPressToContinue;
    [SerializeField]
    private TMP_Text dialogueName, dialogueText, tipText;
    [SerializeField]
    private TMP_Dropdown dialogueOptions;
    [SerializeField]
    private Image[] dialogueImages = new Image[3];
    [SerializeField]
    private Slider dialogueProgress;
    [SerializeField]
    private interactableNPC dialogueNPC;
    [SerializeField]
    //Current Interaction Values
    private List<Interaction> interactionDialogue = new List<Interaction>();
    private int interactionDialogueIndex = 0;
    private string interactionDialogueName = "";
    private interactableType interactionDialogueType;
    private Interactable interactableObject;
    private typeWriterEffect animatedText;
    //Next Interaction List
    public List<Interactable> nextInteractions = new List<Interactable>();
    //Micro-State
    private bool isInteracting = false;
    //Callback On End
    private Action callbackOnEnd;
    public void setCallbackOnEnd(Action _Callback) { callbackOnEnd = _Callback; }
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
    #region Input
    //Player chooses to interact with...
    public void playerInteractWith(Interactable _Interaction)
    {
        if (_Interaction == null) return;
        if (isInteracting)
        {
            nextInteractions.Add(_Interaction);
            return;
        }
        //Game State
        gameState.Instance.setGameState(gameStates.Dialogue);
        //Note: changing the Game's State to Dialogue locks the Camera
        //Camera.main.transform.LookAt(_Interaction.gameObject.transform);
        //Audio Manager
        AudioManager.Instance.Play(audioTrack.SFX_INTERACTION_START, true);
        //Store the Interactable, needed in the case of a PickupItem or NPC
        if (_Interaction.Type == interactableType.PickupItem || _Interaction.Type == interactableType.NPC) interactableObject = _Interaction;
        //Start Interaction
        if (_Interaction.Type == interactableType.Regular) startInteraction(_Interaction.interactionDialogue, ((interactableRegular)_Interaction).Name);
        else if (_Interaction.Type == interactableType.Cutscene) startInteraction(_Interaction.interactionDialogue, "", _Interaction);
        else if (_Interaction.Type == interactableType.PickupItem) startInteraction(_Interaction.interactionDialogue, "", _Interaction);
        //else if (_Interaction.Type == interactableType.NPC) startInteraction((interactableNPC)_Interaction);
        else startInteraction(_Interaction.interactionDialogue);
    }
    //NPC chooses to interact with Player.
    public void InteractWPlayer(interactableNPC _NPCController)
    {
        //Game State
        gameState.Instance.setGameState(gameStates.Dialogue);
        //currentInteraction is only updated for good standards, it could not be updated
        //Interactable NPCInteraction = _NPCController.GetComponent<Interactable>();
        //If the current Interaction's an Entity with Dynamic Dialogue feed it new information to generate new Dialogue
        //if (NPCInteraction.Type == interactableType.NPC) ((interactableNPC)NPCInteraction).Interact(playerCard);
        //startInteraction(_NPCController);
    }
    #endregion
    #region Representation
    //Reset Dialogue Variables
    private void flushDialogue(Interaction[] _interactionDialogue, string _interactionDialogueName = "")
    {
        interactionDialogue.Clear();
        for (int i = 0; i < _interactionDialogue.Length; i++)
        {
            interactionDialogue.Add(_interactionDialogue[i]);
        }
        interactionDialogueIndex = -1;
        //Dialogue Name
        if (string.IsNullOrEmpty(_interactionDialogueName)) dialogueName.transform.parent.gameObject.SetActive(false);
        else
        {
            dialogueName.text = _interactionDialogueName;
            dialogueName.transform.parent.gameObject.SetActive(true);
        }
    }
    //Start Normal Static Interaction
    public void startInteraction(Interaction[] _interactionDialogue, string _interactionDialogueName = "", Interactable _interactableTarget = null)
    {
        //Set Micro-State
        isInteracting = true;
        //Set Dialogue Type
        if (_interactableTarget != null) interactionDialogueType = _interactableTarget.Type;
        else interactionDialogueType = interactableType.Regular;
        //Reset Dialogue
        flushDialogue(_interactionDialogue, _interactionDialogueName);
        //UI Visibility
        switch (interactionDialogueType)
        {
            case interactableType.Regular:
            case interactableType.PickupItem:
            case interactableType.NPC:
                dialoguePanel.SetActive(true);
                imagePanel.SetActive(true);
                for (int i = 0; i < dialogueImages.Length; i++)
                {
                    dialogueImages[i].gameObject.SetActive(true);
                }
                tipPanel.SetActive(false);
                dialogueProgress.gameObject.SetActive(false);
                break;
            case interactableType.Cutscene:
                dialoguePanel.SetActive(false);
                imagePanel.SetActive(false);
                for (int i = 0; i < dialogueImages.Length; i++)
                {
                    dialogueImages[i].gameObject.SetActive(false);
                }
                tipPanel.SetActive(true);
                dialogueProgress.gameObject.SetActive(false);
                break;
        }
        //Proceed
        proceedInteraction();
    }
    //Start NPC Dynamic Interaction
    //public void startInteraction(interactableNPC _interactionNPC)
    //{
    //    dialogueNPC = _interactionNPC;
    //    ////Generate dynamic dialogue
    //    //dialogueNPC.GetComponent<interactableNPC>().Interact(playerCard);
    //    ////Obtain the string dialogue
    //    //Interaction[] _interactionDialogue = dialogueNPC.GetComponent<interactableNPC>().interactionDialogue;
    //    ////Obtain the indexes in NPC's Memory locating the Player
    //    interactionFeedback _interactionFeedack = utilMono.Instance.searchMemory(playerCard.uniqueId, dialogueNPC.Card.Memory);
    //    charRelation rememberedRelation = dialogueNPC.Card.Memory.ElementAt(_interactionFeedack.getMemoryIndex()).charRelations[_interactionFeedack.getRelationIndex()];

    //    //State & UI Visibility
    //    gameState.Instance.setGameState(gameStates.Dialogue);
    //    //flushDialogue(_interactionDialogue);
    //    //flushDialogue(_interactionDialogue, dialogueNPC.Card.Name);
    //    dialogueProgress.value = rememberedRelation.Progress;
    //    dialogueProgress.gameObject.SetActive(true);
    //    //Proceed
    //    proceedInteraction();
    //}
    //Proceed Interaction
    public void proceedInteraction()
    {
        if (animatedText != null && !animatedText.hasFinishedAnimation)
        {
            animatedText.finishAnimationInstantly();
            return;
        }
        //Audio Manager
        AudioManager.Instance.Play(audioTrack.SFX_INTERACTION_CONTINUE, true);
        //Before Dialogue Index update, based on the Player's previous inputs
        if (interactionDialogueIndex > -1 && dialogueNPC != null)
        {
            Interaction previousInteraction = interactionDialogue[interactionDialogueIndex];
            //Dialogue NPC Relationship
            int dialogueOptionsIndex = 0;
            if (previousInteraction.Type == interactionType.YesOrNo || previousInteraction.Type == interactionType.MultipleChoice) dialogueOptionsIndex = dialogueOptions.value;
            dialogueNPC.addProgress(previousInteraction.Reward[dialogueOptionsIndex]);
        }

        //Dialogue Index update
        interactionDialogueIndex++;

        //After Dialogue Index update
        if (interactionDialogueIndex >= interactionDialogue.Count) endInteraction();
        else
        {
            Interaction currentInteraction = interactionDialogue[interactionDialogueIndex];
            //Dialogue Text
            switch (interactionDialogueType)
            {
                case interactableType.Regular:
                case interactableType.PickupItem:
                case interactableType.NPC:
                    //Dialogue adaptation for Dialogue Images
                    if (currentInteraction.Dialogue.richText.Length == 0)
                    {
                        dialoguePanel.gameObject.SetActive(false);
                        imageLabelPressToContinue.SetActive(true);
                    }
                    else
                    {
                        dialoguePanel.gameObject.SetActive(true);
                        imageLabelPressToContinue.SetActive(false);
                        dialogueText.text = currentInteraction.Dialogue.richText;
                        animatedText = dialogueText.GetComponent<typeWriterEffect>();
                        animatedText.totalVisibleCharacters = currentInteraction.Dialogue.Text.Length;
                        animatedText.Play();
                    }
                    //Dialogue Images
                    if (currentInteraction.Images != null && currentInteraction.Images.Length <= 3)
                    {
                        for (int i = 0; i < currentInteraction.Images.Length; i++)
                        {
                            interactionImgLibEntry currentdialogueImages = currentInteraction.Images[i];
                            dialogueImages[i].sprite = Resources.Load<Sprite>(interactionImgLib.getEntry(currentdialogueImages));
                        }
                        for (int i = currentInteraction.Images.Length; i < 3; i++) dialogueImages[i].sprite = Resources.Load<Sprite>(interactionImgLib.getEntry(interactionImgLibEntry.None));
                    }
                    else for (int i = 0; i < 3; i++) dialogueImages[i].sprite = Resources.Load<Sprite>(interactionImgLib.getEntry(interactionImgLibEntry.None));
                    //Dialogue Music
                    if (currentInteraction.Music != null)
                    {
                        for (int i = 0; i < currentInteraction.Music.Length; i++)
                        {
                            soundLibEntry Music = currentInteraction.Music[i];
                            AudioManager.Instance.changeTrack(Music, true);
                        }
                    }
                    break;
                case interactableType.Cutscene:
                    tipText.text = currentInteraction.Dialogue.Text;
                    animatedText = tipText.GetComponent<typeWriterEffect>();
                    animatedText.totalVisibleCharacters = currentInteraction.Dialogue.Text.Length;
                    animatedText.Play();
                    break;
            }
            //Dialogue Options
            dialogueOptions.ClearOptions();
            switch (currentInteraction.Type)
            {
                case interactionType.YesOrNo:
                    dialogueOptions.gameObject.SetActive(true);
                    dialogueOptions.AddOptions(new List<string>() { "Yes", "No" });
                    break;
                case interactionType.MultipleChoice:
                    dialogueOptions.gameObject.SetActive(true);
                    string[] Options = ((interactionMultipleChoice)currentInteraction).Options;
                    List<string> listedOptions = new List<string>();
                    for (int i = 0; i < Options.Length; i++) listedOptions.Add(Options[i]);
                    dialogueOptions.AddOptions(listedOptions);
                    break;
                default:
                    dialogueOptions.gameObject.SetActive(false);
                    break;
            }
            //Dialogue Progress
            //if (dialogueNPC != null)
            //{
            //    ////Obtain the indexes in NPC's Memory locating the Player
            //    interactionFeedback _interactionFeedack = utilMono.Instance.searchMemory(playerCard.uniqueId, dialogueNPC.Card.Memory);
            //    charRelation rememberedRelation = dialogueNPC.Card.Memory.ElementAt(_interactionFeedack.getMemoryIndex()).charRelations[_interactionFeedack.getRelationIndex()];
            //    dialogueProgress.value = rememberedRelation.Progress;
            //}
        }
    }
    //End Interaction
    private void endInteraction()
    {
        //Set Micro-State
        isInteracting = false;
        //Audio Manager
        AudioManager.Instance.Play(audioTrack.SFX_INTERACTION_END, true);
        //Do any aftermath
        switch (interactionDialogueType)
        {
            case interactableType.PickupItem:
                interactablePickupItem pickedUpItem = (interactablePickupItem)interactableObject;
                //Play the Music
                AudioManager.Instance.changeTrack(pickedUpItem.pickupMusic, true);
                //Add Item to Inventory
                itemData newItem = jsonLoader.Instance.loadItemData(pickedUpItem.itemId);
                Journal.Instance.addItem(newItem);
                break;
        }
        //Reset the stored values
        interactableObject = null;
        dialogueNPC = null;
        //If there are any next Interactions in line
        if (nextInteractions.Count > 0)
        {
            playerInteractWith(nextInteractions.First());
            nextInteractions.RemoveAt(0);
        }
        else
        {
            //Callback
            if (callbackOnEnd != null)
            {
                callbackOnEnd();
                callbackOnEnd = null;
            }
        }
    }
    #endregion
}
