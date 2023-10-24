using UnityEngine;
public class eventTrigger : MonoBehaviour
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
            eventManager.Instance.addEventTrigger(this);
            updateVisibility();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!myEvent.hasLoaded) return;
            if (myEvent.canTrigger())
            {
                if (eventIndex == -1) eventIndex = eventManager.Instance.addEvent(myEvent);
                if (myEvent.incStoryFlag) runtimeSaveData.Instance.storyFlag = myEvent.storyFlag + 1;
                if (myEvent.incStoryFlagIndex) runtimeSaveData.Instance.storyFlagIndex = myEvent.storyFlagIndex + 1;
                eventManager.Instance.startEvent(eventIndex);
                if (!myEvent.Repeat) { }
            }
        }
    }
    //Update Visibility
    public void updateVisibility()
    {
        if (myEvent.canTrigger()) { }
        else { }
    }
}
