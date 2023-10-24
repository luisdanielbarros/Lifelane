using UnityEngine;

public class eventObject : MonoBehaviour
{
    //Json Path
    [SerializeField]
    private string jsonPath;
    //Event Data
    private eventData myEvent;
    void Update()
    {
        if (!string.IsNullOrEmpty(jsonPath) && myEvent == null)
        {
            myEvent = jsonLoader.Instance.loadEventData(jsonPath);
            myEvent.setAsLoaded();
            eventManager.Instance.addEventObject(this);
            updateVisibility();
        }
    }
    //Update Visibility
    public void updateVisibility()
    {
        if (myEvent.canTrigger()) for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(true);
        else for (int i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(false);
    }
}
