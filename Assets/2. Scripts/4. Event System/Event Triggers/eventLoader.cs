using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class eventLoader : MonoBehaviour
{
    //Json Path
    [SerializeField]
    private string jsonPath;
    //Event Data
    private eventData myEvent;
    //Event Index
    private int eventIndex;
    //Cutscene
    [SerializeField]
    private PlayableDirector cutscene;
    [SerializeField]
    private GameObject cutscenecamera;
    //Mapped Cameras
    [SerializeField]
    private GameObject mappedCameras;
    void Start()
    {
        //Gathering the Event information
        ////Mapped Cameras
        if (mappedCameras != null)
        {
            cameraManager.Instance.addMappedCamerasManager(mappedCameras);
            foreach (Transform mappedCamera in mappedCameras.transform)
            {
                cameraManager.Instance.addMappedCamera(mappedCamera.gameObject);
            }
            mappedCameras = null;
        }
        ////Events
        myEvent = jsonLoader.Instance.loadEventData(jsonPath);
        myEvent.setAsLoaded();

        ////Cutscenes
        if (cutscene != null && cutscenecamera != null) myEvent.loadUnityReferences(cutscene, cutscenecamera);

        /*General Process
        1. Load the player's saved data (In the Loading Manager, by calling loadingManager.configLevelSettingsDelayed())
        2. Overwrite parts of the player's saved data with broad warp settings found in the WarpData object (In the Loading Manager, by calling loadingManager.configLevelSettingsDelayed())
        3. Overwrite parts of the previous data with narrower event settings found in the EventData object*/

        //Configure the level according to its broader warp settings found in the WarpData object
        loadingManager.Instance.configLevelSettingsDelayed();

        //Configure the level according to its narrower event settings found in the EventData object
        if (myEvent.canTrigger())
        {
            eventIndex = eventManager.Instance.addEvent(myEvent);
            if (myEvent.incStoryFlag) runtimeSaveData.Instance.storyFlag = myEvent.storyFlag + 1;
            if (myEvent.incStoryFlagIndex) runtimeSaveData.Instance.storyFlagIndex = myEvent.storyFlagIndex + 1;
            eventManager.Instance.startEvent(eventIndex);
        }
        //If for some reason the event wasn't meant to run
        else
        {
            //If it's loading the Overworld returning from a battle
            if (battleSystem.Instance.justBattled) utilMono.Instance.setPlayerCoordinates(battleSystem.Instance.playerTransform.position);
        }
    }
}
