using UnityEngine;
public class eventInteractable : MonoBehaviour
{
    //Json Path
    [SerializeField]
    private string jsonPath;
    //Event Data
    private eventData myEvent;
    //Event Index
    private int eventIndex = -1;
    void Update()
    {
        if (!string.IsNullOrEmpty(jsonPath) && myEvent == null)
        {
            myEvent = jsonLoader.Instance.loadEventData(jsonPath);
            myEvent.setAsLoaded();
            eventManager.Instance.addEventInteractable(this);
            updateVisibility();
            Interactable[] eventInteractions = myEvent.Interactions;
            for (int i = 0; i < eventInteractions.Length; i++)
            {
                Interactable currentInteraction = eventInteractions[i];
                if (currentInteraction.Type == interactableType.PickupItem)
                {
                    //Particle Effect
                    particleEffect currentInteractionEffect = ((interactablePickupItem)currentInteraction).Effect;
                    switch (currentInteractionEffect)
                    {
                        case particleEffect.Glow:
                            utilMono.Instance.createParticleGlowEffect(transform.Find("Object").gameObject);
                            break;
                    }
                }
            }
        }
    }
    public void Interact()
    {
        if (!myEvent.isInitialized || !myEvent.hasLoaded) return;
        if (myEvent.canTrigger())
        {
            if (eventIndex == -1) eventIndex = eventManager.Instance.addEvent(myEvent);
            if (myEvent.incStoryFlag) runtimeSaveData.Instance.storyFlag = myEvent.storyFlag + 1;
            if (myEvent.incStoryFlagIndex) runtimeSaveData.Instance.storyFlagIndex = myEvent.storyFlagIndex + 1;
            eventManager.Instance.startEvent(eventIndex);
            if (!myEvent.Repeat) transform.Find("Trigger").GetComponent<Collider>().enabled = false;
        }
    }
    public bool canTrigger()
    {
        if (myEvent == null) return false;
        return myEvent.canTrigger();
    }
    //Update Visibility
    public void updateVisibility()
    {
        if (myEvent.canTrigger()) transform.Find("Trigger").GetComponent<Collider>().enabled = true;
        else transform.Find("Trigger").GetComponent<Collider>().enabled = false;
    }
}
