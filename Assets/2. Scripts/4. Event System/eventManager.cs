using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class eventManager : MonoBehaviour
{
    //Instance
    private static eventManager instance = null;
    public static eventManager Instance { get { return instance; } }
    //List of Events
    private List<eventData> customEvents = new List<eventData>();
    //List of Events in Wait
    private List<int> customEventsInWait = new List<int>();
    //Cutscene Cameras (w/ Cinemachine Brain)
    private List<GameObject> cutsceneCameras = new List<GameObject>();
    //Default Camera
    [SerializeField]
    private GameObject generalCamera;
    //Current Index
    private int currentIndex = -1;
    //List of Event Objects
    private List<eventInteractable> customEventInteractables = new List<eventInteractable>();
    private List<eventObject> customEventObjects = new List<eventObject>();
    private List<eventTrigger> customEventTriggers = new List<eventTrigger>();
    //Micro-state
    private bool isPlayingEvent = false;
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
    void Update()
    {
        //Skip Cutscene
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentIndex > -1 && gameState.Instance.currentState == gameStates.Cutscene)
            {
                skipCutscene();
            }
        }
    }
    //Add Cutscene
    public int addEvent(eventData newEvent)
    {
        if (newEvent == null) return -1;
        if (newEvent.Cutscene != null) newEvent.Cutscene.stopped += OnCutsceneEnd;
        customEvents.Add(newEvent);
        return customEvents.IndexOf(newEvent);
    }
    #region Event Lifecycle
    //1. Play Music
    private void playMusic()
    {
        for (int i = 0; i < customEvents[currentIndex].Music.Length; i++)
        {
            soundLibEntry Music = customEvents[currentIndex].Music[i];
            AudioManager.Instance.changeTrack(Music, true);
        }
    }
    //2. Set Player Coordinates
    private void setPlayerCoordinates()
    {
        Vector3 playerCoordinates = customEvents[currentIndex].playerCoordinates;
        if (playerCoordinates != null)
        {
            GameObject emptyGameObject = new GameObject();
            Transform playerTransform = emptyGameObject.transform;
            playerTransform.position = playerCoordinates;
            utilMono.Instance.setPlayerCoordinates(playerTransform.position);
        }
    }
    //2. Set Player Rotation
    private void setPlayerRotation()
    {
        float playerRotation = customEvents[currentIndex].playerRotation;
        utilMono.Instance.setPlayerRotation(playerRotation);
    }
    //2. Set Player Movement
    private void setPlayerMovement()
    {
        playerMovement playerMovement = customEvents[currentIndex].playerMovement;
        utilMono.Instance.setPlayerMovement(playerMovement);
    }
    //3. Start Event
    public void startEvent(int Index)
    {
        if (isPlayingEvent)
        {
            customEventsInWait.Add(Index);
            return;
        }
        isPlayingEvent = true;
        updateEventObjects();
        currentIndex = Index;
        if (Index >= customEvents.Count && customEvents[Index] == null) return;
        //1. Play Music
        if (customEvents[currentIndex].Music.Length > 0) playMusic();
        //2. Set Player Coordinates & Rotation
        if (customEvents[currentIndex].teleportsPlayer)
        {
            setPlayerCoordinates();
            setPlayerRotation();
        }
        //2. Set Player Movement
        setPlayerMovement();
        //3. If Cutscene exists, play it
        if (customEvents[currentIndex].Cutscene != null && customEvents[currentIndex].cutsceneCamera != null)
        {
            if (generalCamera != null) generalCamera.SetActive(false);
            customEvents[currentIndex].Cutscene.Play();
            customEvents[currentIndex].cutsceneCamera.SetActive(true);
            gameState.Instance.setGameState(gameStates.Cutscene);
        }
        //4. Else skip to the Interaction
        else playInteraction();
    }
    //On Cutscene end
    private void OnCutsceneEnd(PlayableDirector playDir)
    {
        playDir.Stop();
        if (generalCamera != null) generalCamera.SetActive(true);
        customEvents[currentIndex].cutsceneCamera.SetActive(false);
        //Go to the Interaction
        playInteraction();
    }
    //Skip Cutscene
    private void skipCutscene()
    {
        customEvents[currentIndex].Cutscene.Stop();
    }
    //Clear Events
    public void clearEvents()
    {
        customEvents.Clear();
        cutsceneCameras.Clear();
        customEventObjects.Clear();
    }
    //4. Play Interaction
    private void playInteraction()
    {
        Interactable[] eventInteractions = customEvents[currentIndex].Interactions;
        //If the array of Interaction is null or empty
        if (eventInteractions != null && eventInteractions.Length > 0)
        {
            for (int i = 0; i < eventInteractions.Length; i++)
            {
                interactionController.Instance.setCallbackOnEnd(endInteraction);
                interactionController.Instance.playerInteractWith(eventInteractions[i]);
            }
        }
        else
        {
            if (!playJournal()) if (!playBattle()) playWarp();
        }
    }
    //4. End Interaction
    private void endInteraction()
    {
        if (!playJournal()) if (!playBattle()) playWarp();
    }
    //5. Journal
    private bool playJournal()
    {
        string eventItemId = customEvents[currentIndex].collectibleItemId;
        if (!string.IsNullOrWhiteSpace(eventItemId))
        {
            Journal.Instance.loadNote(eventItemId);
            endEvent();
            return true;
        }
        else return false;
    }

    //6. Play Battle
    private bool playBattle()
    {
        battleData[] eventBattles = customEvents[currentIndex].battlesData;
        if (eventBattles == null) return false;
        else
        {
            battleSystem.Instance.setupBattle(eventBattles, utilMono.Instance.getPlayerCoordinates());
            endEvent();
            return true;
        }
    }
    //6. Play Warp
    private void playWarp()
    {
        string eventWarp = customEvents[currentIndex].mapWarp;
        if (eventWarp != null && !string.IsNullOrWhiteSpace(eventWarp))
        {
            warpData warpData = jsonLoader.Instance.loadWarpData(eventWarp, jsonWarpType.Literal);
            loadingManager.Instance.LoadLevel(warpData);
            endEvent(true);
        }
        else endEvent();
    }
    //99. End Event
    private void endEvent(bool isWarp = false)
    {
        isPlayingEvent = false;
        if (isWarp)
        {
            customEventsInWait.Clear();
            return;
        }
        if (customEventsInWait.Count > 0)
        {
            int eventInWait = customEventsInWait[0];
            customEventsInWait.RemoveAt(0);
            startEvent(eventInWait);
        }
        else gameState.Instance.setGameState(gameStates.OverWorld);
    }
    #endregion
    #region List of Event Objects
    public void addEventInteractable(eventInteractable _eventInteractable)
    {
        customEventInteractables.Add(_eventInteractable);
    }
    public void addEventObject(eventObject _eventObject)
    {
        customEventObjects.Add(_eventObject);
    }
    public void addEventTrigger(eventTrigger _eventTrigger)
    {
        customEventTriggers.Add(_eventTrigger);
    }
    private void updateEventObjects()
    {
        for (int i = 0; i < customEventInteractables.Count; i++) customEventInteractables[i].updateVisibility();
        for (int i = 0; i < customEventObjects.Count; i++) customEventObjects[i].updateVisibility();
        for (int i = 0; i < customEventTriggers.Count; i++) customEventTriggers[i].updateVisibility();
    }
    #endregion
}
