using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadingManager : MonoBehaviour
{
    //Instance
    private static loadingManager instance = null;
    public static loadingManager Instance { get { return instance; } }
    //Operation
    AsyncOperation Operation;
    //UI
    [SerializeField]
    private Slider loadingSlider;
    [SerializeField]
    private GameObject levelLoadedMenu;
    [SerializeField]
    private Image levelLoadedContainer;
    [SerializeField]
    private TextMeshProUGUI levelName;
    //Scene Loaders
    private warpData sceneLoader;
    //Previous Scene, for the Battle System
    private warpData previousSceneLoader;
    //Loading Screen, for notifying when the level's finished loading
    [SerializeField]
    private LoadingScreen loadingScreen;
    //Micro-states
    ////Is Overworld Loaded
    private bool showPlayerModel = false;
    ////Current Level Index
    private int currentlevelindex = 0;
    public int currentLevelIndex { get { return currentlevelindex; } }
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
        //UI
        levelLoadedMenu.SetActive(false);
    }
    #region Level Loading "API"
    //Load Previous Level
    public void LoadPreviousLevel()
    {
        LoadLevel(previousSceneLoader);
    }
    //Load Virtual Hub
    public void loadVirtualHub()
    {
        StartCoroutine(loadAsynchronously(1));
    }
    //Load Level
    public void LoadLevel(warpData _sceneLoader = null, bool savePreviousWarpData = false)
    {
        //UI
        //Previous Scene Loader
        if (savePreviousWarpData) previousSceneLoader = sceneLoader;
        else previousSceneLoader = null;
        //Current Scene Loader
        sceneLoader = _sceneLoader;
        //Coroutine
        StartCoroutine(loadAsynchronously(_sceneLoader.buildIndex));
    }
    IEnumerator loadAsynchronously(int sceneIndex)
    {
        currentlevelindex = sceneIndex;
        Operation = SceneManager.LoadSceneAsync(sceneIndex);
        if (sceneIndex >= 2 && sceneIndex <= 6)
        {
            Operation.allowSceneActivation = false;
            loadingScreen.Unready();
        }
        //Game State in function of Scene Index
        switch (sceneIndex)
        {
            //To Main Menu
            case 0:
                gameState.Instance.setGameState(gameStates.MainMenu);
                break;
            //To Virtual Hub
            case 1:
            //To OverWorld
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            //To Backrooms
            case 9:
            //To Render Room
            case 10:
                gameState.Instance.setGameState(gameStates.LoadingScreen);
                if ((sceneIndex >= 2 && sceneIndex <= 6) || sceneIndex == 9 || sceneIndex == 10)
                {
                    loadingScreen.playAnimations();
                    clearSystems();
                }
                break;
            //To Battle
            case 7:
                gameState.Instance.setGameState(gameStates.Battle);
                break;
            //To Credits
            case 8:
                clearSystems();
                gameState.Instance.setGameState(gameStates.Credits);
                break;
            default:
                errorManager.Instance.createErrorReport("loadingManager", "loadAsynchronously", errorType.switchCase); break;
        }
        while (!Operation.isDone)
        {
            float Progress = Mathf.Clamp(Operation.progress / 0.9f, 0, 1);
            loadingSlider.value = Progress;
            if (Progress == 1)
            {
                //Overworld or Backrooms
                if (sceneIndex >= 2 && sceneIndex <= 6 || sceneIndex == 9 || sceneIndex == 10)
                {
                    configLevelSettings();
                    loadingScreen.Ready();
                    yield break;
                }
            }
            yield return null;
        }
    }
    //Update Player Overworld Model
    public void updatePlayerModel()
    {
        playerPerspective playersPers = cameraManager.Instance.playerPers;
        if (!showPlayerModel && (playersPers == playerPerspective.Controlled || playersPers == playerPerspective.Mapped))
        {
            showPlayerModel = true;
            runtimeSaveData.Instance.loadOverworldData(showPlayerModel);
        }
        else if (showPlayerModel && playersPers == playerPerspective.POV)
        {
            utilMono.Instance.destroyPlayerModel();
            showPlayerModel = false;
        }
    }
    #endregion
    #region Level Loading Lifecycle Methods
    //Clear Systems
    private void clearSystems()
    {
        cameraManager.Instance.clearMappedCameras();
        eventManager.Instance.clearEvents();
    }
    //Configure Level Settings
    private void configLevelSettings()
    {
        //Level Name
        if (sceneLoader.showLevelName)
        {
            levelName.SetText(sceneLoader.levelName);
            levelLoadedMenu.gameObject.SetActive(true);
            StartCoroutine(utilMono.Instance.fadeUI(levelLoadedContainer, "Image"));
            StartCoroutine(utilMono.Instance.fadeUI(levelName, "Text"));
        }
        //Audio
        AudioManager.Instance.changeTrack(sceneLoader.audioMusic, true);
        AudioManager.Instance.changeTrack(sceneLoader.audioIndustral, true);
        AudioManager.Instance.changeTrack(sceneLoader.audioNature, true);
        AudioManager.Instance.changeTrack(sceneLoader.audioWeather, true);
        //Weather
        timeWeather.Instance.currWeather = sceneLoader.levelWeather;
    }
    //Configure Level Settings, Delayed
    public void configLevelSettingsDelayed()
    {
        //Camera Manager, which in turn does the Player Overworld Model
        if (gameState.Instance.currentMode == gameModes.StoryMode) cameraManager.Instance.playerPers = sceneLoader.playerPerspec;
        else cameraManager.Instance.resetCameras();
        //Player Coordinates & Rotation
        utilMono.Instance.setPlayerCoordinates(sceneLoader.playerCoordinates);
        utilMono.Instance.setPlayerRotation(sceneLoader.playerRotation);
    }
    #endregion
    #region UI
    //Enter Level
    public void enterLevel()
    {
        Operation.allowSceneActivation = true;
    }
    //Exit Game
    public void exitGame()
    {
        Application.Quit();
    }
    #endregion
}
