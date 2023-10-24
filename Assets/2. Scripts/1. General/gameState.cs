using UnityEngine;
//Game States
public enum gameStates { MainMenu, SaveMenu, OptionsMenu, LoadingScreen, OverWorld, PauseMenu, Cutscene, Dialogue, overworldMenu, collectiblesInventory, Battle, Credits };
//Game Modes
public enum gameModes { None, StoryMode, ExplorationMode }
//Game State
public class gameState : MonoBehaviour
{
    #region Instance & State Variables
    //Instance
    private static gameState instance = null;
    public static gameState Instance { get { return instance; } }
    //State
    private static gameStates currentstate = gameStates.MainMenu;
    public gameStates currentState { get { return currentstate; } set { setGameState(value); } }
    //Previous State
    private gameStates previousstate;
    public gameStates previousState { get { return previousstate; } }
    //Mode
    private gameModes currentmode = gameModes.None;
    public gameModes currentMode { get { return currentmode; } set { setGameMode(value); } }
    //Previous Overworld State's Music
    private soundLibEntry previousOverworldMusic;
    //Micro-State
    private bool debugIsTimeScaled = false;
    #endregion
    #region UI Variables
    //Canvas
    [SerializeField]
    private GameObject UICanvas;
    //Main Menu
    [SerializeField]
    private GameObject MainMenu;
    //Save Menu
    [SerializeField]
    private GameObject SaveMenu;
    //Options Menu
    [SerializeField]
    private GameObject OptionsMenu;
    //Loading Screen
    [SerializeField]
    private GameObject LoadingScreen;
    //Pause Menu
    [SerializeField]
    private GameObject PauseMenu;
    //Dialogue Panel
    private GameObject dialoguePanel;
    //Overworld Menu
    private GameObject overworldMenu;
    //Inventory Item Viewer
    private GameObject inventoryItemViewer;
    //Journal Menu
    private GameObject journalMenu;
    //Battle Menu
    private GameObject battleMenu;
    //Goals & Hints Menu
    private GameObject goalsHintsViewer;
    //Screen Effects Overlay
    [SerializeField]
    private GameObject screenEffectsOverlay;
    //Player UI
    [SerializeField]
    private GameObject playerUI;
    #endregion
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
        //Canvas
        DontDestroyOnLoad(UICanvas);
        //State
        setGameState(gameStates.MainMenu);
    }
    void Update()
    {
        //Pause Menu & Collectibles Inventory
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentstate == gameStates.OverWorld) setGameState(gameStates.PauseMenu);
            else if (currentstate == gameStates.collectiblesInventory)
            {
                setGameState(gameStates.overworldMenu);
                Journal.Instance.clearNote();
            }
        }
        //Overworld Menu
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentstate == gameStates.OverWorld)
            {
                setGameState(gameStates.overworldMenu);
                overworldMenu.GetComponent<overworldMenu>().Open(currentMode);
            }
            else if (currentstate == gameStates.overworldMenu) setGameState(gameStates.OverWorld);
        }
    }
    //Game State
    #region Game State
    public void setGameState(gameStates _currentState)
    {
        previousstate = currentstate;
        if (AudioManager.Instance != null) previousOverworldMusic = AudioManager.Instance.getCurrentMusic();
        currentstate = _currentState;
        manageGameStates();
    }
    private void manageGameStates()
    {
        MainMenu.SetActive(false);
        SaveMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        LoadingScreen.SetActive(false);
        PauseMenu.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (overworldMenu != null) overworldMenu.SetActive(false);
        if (journalMenu != null) journalMenu.SetActive(false);
        if (inventoryItemViewer != null) inventoryItemViewer.SetActive(false);
        if (battleMenu != null) battleMenu.SetActive(false);
        if (goalsHintsViewer != null) goalsHintsViewer.SetActive(false);
        screenEffectsOverlay.SetActive(false);
        playerUI.SetActive(false);
        switch (currentstate)
        {
            case gameStates.MainMenu:
                MainMenu.SetActive(true);
                break;
            case gameStates.SaveMenu:
                saveManager.Instance.showSaveFiles();
                SaveMenu.SetActive(true);
                break;
            case gameStates.OptionsMenu:
                OptionsMenu.SetActive(true);
                break;
            case gameStates.LoadingScreen:
                Cursor.lockState = CursorLockMode.None;
                if (!debugIsTimeScaled) Time.timeScale = 1f;
                LoadingScreen.SetActive(true);
                break;
            case gameStates.OverWorld:
                Cursor.lockState = CursorLockMode.None;
                if (!debugIsTimeScaled) Time.timeScale = 1f;
                goalsHintsViewer.SetActive(true);
                screenEffectsOverlay.SetActive(true);
                //playerUI.SetActive(true);
                break;
            case gameStates.PauseMenu:
                Time.timeScale = 0f;
                PauseMenu.SetActive(true);
                break;
            case gameStates.Cutscene:
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case gameStates.Dialogue:
                Cursor.lockState = CursorLockMode.Locked;
                if (!debugIsTimeScaled) Time.timeScale = 0f;
                dialoguePanel.SetActive(true);
                break;
            case gameStates.overworldMenu:
                Cursor.lockState = CursorLockMode.None;
                if (!debugIsTimeScaled) Time.timeScale = 1f;
                overworldMenu.SetActive(true);
                if (previousstate == gameStates.collectiblesInventory) AudioManager.Instance.changeTrack(previousOverworldMusic, true);
                break;
            case gameStates.collectiblesInventory:
                Cursor.lockState = CursorLockMode.None;
                if (!debugIsTimeScaled) Time.timeScale = 0f;
                journalMenu.SetActive(true);
                if (previousstate == gameStates.overworldMenu) AudioManager.Instance.changeTrack(soundLibEntry.MUSIC_COLLECTIBLES, true);
                break;
            case gameStates.Battle:
                Cursor.lockState = CursorLockMode.None;
                if (!debugIsTimeScaled) Time.timeScale = 1f;
                battleMenu.SetActive(true);
                break;
            case gameStates.Credits:
                break;
            default:
                errorManager.Instance.createErrorReport("gameState", "manageGameStates", errorType.switchCase);
                break;
        }
    }
    #endregion
    //Game Mode
    #region Game Mode
    private void setGameMode(gameModes _currentMode)
    {
        currentmode = _currentMode;
    }
    #endregion
    //Dependency Injection
    public void dependencyInjection(GameObject _dialoguePanel, GameObject _overworldMenu, GameObject _inventoryItemViewer, GameObject _journalMenu, GameObject _battleMenu, GameObject _goalsHintsViewer)
    {
        dialoguePanel = _dialoguePanel;
        overworldMenu = _overworldMenu;
        inventoryItemViewer = _inventoryItemViewer;
        journalMenu = _journalMenu;
        battleMenu = _battleMenu;
        goalsHintsViewer = _goalsHintsViewer;
        manageGameStates();
    }
    //Micro-State
    public void setDebugIsTimeScaled(bool isTimeScaled)
    {
        debugIsTimeScaled = isTimeScaled;
    }
}
