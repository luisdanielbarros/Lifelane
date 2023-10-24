using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class interactionTip : MonoBehaviour
{
    //Pickup UI
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float pickupTime = 1f;
    [SerializeField]
    private RectTransform interactPanel;
    [SerializeField]
    private Image UIProgressImage;
    [SerializeField]
    private TextMeshProUGUI UIText;
    //Stored values
    private Interactable storedInteraction;
    private inventorySlot currentPickupItem;
    private float currentPickupTimerElapsed;
    private bool previousRayCastResult = true;
    private eventInteractable storedInteractableEvent;
    //Player Reference
    GameObject playerObj;
    void Start()
    {
        interactPanel.gameObject.SetActive(false);
        playerObj = utilMono.Instance.getPlayerObject();
    }
    void Update()
    {
        //If the Player's in a Dialogue Game State, listen to his Key Inputs and nothing else
        if (gameState.Instance.currentState == gameStates.Dialogue && Input.GetKeyDown(KeyCode.Q))
        {
            interactionController.Instance.proceedInteraction();
            return;
        }
        //Else if the Player's in the OverWorld Game State, cast Rays to search for Interactions
        else if (gameState.Instance.currentState == gameStates.OverWorld)
        {
            getInteraction();
            //If an Interaction was found
            if (storedInteraction != null || storedInteractableEvent != null)
            {
                //Show Interaction Tip
                if (!previousRayCastResult)
                {
                    interactPanel.gameObject.SetActive(true);
                    previousRayCastResult = true;
                }
                if (storedInteractableEvent != null)
                {
                    if (Input.GetKey(KeyCode.Q))
                    {
                        interactPanel.gameObject.SetActive(false);
                        previousRayCastResult = false;
                        storedInteractableEvent.Interact();
                    }
                }
                else if (storedInteraction != null)
                {
                    //Interaction of Type PickupItem
                    if (storedInteraction.Type == interactableType.PickupItem)
                    {
                        if (Input.GetKey(KeyCode.Q)) updatePickupProgress();
                        else if (!Input.GetKey(KeyCode.Q)) currentPickupTimerElapsed = 0f;
                        updatePickupGraphics();
                    }
                    //Interaction of Type Object & Entity & Warp (Changes Game State to Dialogue)
                    else if (Input.GetKeyDown(KeyCode.Q))
                    {
                        if (storedInteraction.Type == interactableType.Regular)
                        {
                            interactPanel.gameObject.SetActive(false);
                            previousRayCastResult = false;
                            interactionController.Instance.playerInteractWith(storedInteraction);
                        }
                        else if (storedInteraction.Type == interactableType.NPC)
                        {
                            interactPanel.gameObject.SetActive(false);
                            previousRayCastResult = false;
                            interactionController.Instance.playerInteractWith(storedInteraction);
                        }
                    }
                }
            }
            //Else hide Interaction Tip
            else if (previousRayCastResult)
            {
                interactPanel.gameObject.SetActive(false);
                currentPickupTimerElapsed = 0f;
                previousRayCastResult = false;
            }
        }
    }
    private void updatePickupProgress()
    {
        currentPickupTimerElapsed += Time.deltaTime;
        if (currentPickupTimerElapsed >= pickupTime) MoveItemToInventory();
    }
    private void updatePickupGraphics()
    {
        float relativePickupTime = currentPickupTimerElapsed / pickupTime;
        UIProgressImage.fillAmount = relativePickupTime;
    }
    private void getInteraction()
    {
        Vector3 playerForward = playerObj.transform.TransformDirection(Vector3.forward);
        RaycastHit interactionInfo;
        if (Physics.Raycast(playerObj.transform.position, playerForward, out interactionInfo, 2, layerMask))
        {
            GameObject interactedObject = interactionInfo.collider.gameObject;
            //If the Raycast hits an Interaction-tag Object
            if (interactedObject.tag == "Interaction")
            {
                //Interactable currentInteraction = interactedObject.transform.parent.GetComponent<Interactable>();
                Interactable currentInteraction = null;
                //If the current Interaction's null
                if (currentInteraction == null)
                {
                    //If instead of an Interactable, it's an Event
                    eventInteractable currentEvent = interactedObject.transform.parent.GetComponent<eventInteractable>();
                    //If the current Event's null
                    if (currentEvent == null)
                    {
                        storedInteraction = null;
                        storedInteractableEvent = null;
                    }
                    else
                    {
                        //If the current Event can trigger as it's the next one in the story
                        if (currentEvent.canTrigger())
                        {
                            //If the current Event is the same as the stored Event, do nothing
                            if (storedInteractableEvent == currentEvent) return;
                            //Else do the Event
                            else
                            {
                                storedInteraction = null;
                                storedInteractableEvent = currentEvent;
                                UIProgressImage.gameObject.SetActive(true);
                                UIText.text = "Interact with " + "a";
                            }
                        }
                    }
                }
                //Else if the current Interaction is the same as the stored Interaction, do nothing
                else if (currentInteraction == storedInteraction) return;
                //Else do the Interaction
                else
                {
                    storedInteraction = currentInteraction;
                    storedInteractableEvent = null;

                    //Check the Interaction Type
                    //If the Interaction is of Type Pickupitem
                    if (currentInteraction.Type == interactableType.PickupItem)
                    {
                        //Inventory
                        currentPickupItem = interactedObject.GetComponent<inventorySlot>();
                        //If an Inventory Slot Component was found
                        if (currentPickupItem != null)
                        {
                            UIProgressImage.gameObject.SetActive(true);
                            UIText.text = "Pick up " + currentPickupItem.slotItem.Name;
                        }
                    }
                    //If the Interaction is of Type Object
                    else if (currentInteraction.Type == interactableType.Regular)
                    {
                        interactableRegular castInteraction = (interactableRegular)currentInteraction;
                        UIProgressImage.gameObject.SetActive(false);
                        UIText.text = "Interact with " + castInteraction.Name;
                    }
                    //If the Interaction is of Type Entity
                    else if (currentInteraction.Type == interactableType.NPC)
                    {
                        interactableNPC castInteraction = (interactableNPC)currentInteraction;
                        UIProgressImage.gameObject.SetActive(false);
                        UIText.text = "Warp to " + castInteraction.Name;
                    }
                }
            }
        }
        //Else if no Raycast Collision was found
        else
        {
            storedInteraction = null;
            storedInteractableEvent = null;
        }
    }
    private void MoveItemToInventory()
    {
        inventoryManager.Instance.addItem(currentPickupItem);
        //Destroy(storedInteraction.gameObject);
        storedInteraction = null;
    }
}
