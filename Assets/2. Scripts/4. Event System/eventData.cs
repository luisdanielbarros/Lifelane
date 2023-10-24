using System;
using UnityEngine;
using UnityEngine.Playables;
[Serializable]
public class eventData
{
    //Is Initialized
    private bool isinitialized = false;
    public bool isInitialized { get { return isinitialized; } }
    //Has Loaded
    private bool hasloaded = false;
    public bool hasLoaded { get { return hasloaded; } }

    //0. Story Flag
    private storyFlags storyflag;
    public storyFlags storyFlag { get { return storyflag; } }
    private int storyflagindex;
    public int storyFlagIndex { get { return storyflagindex; } }
    private bool incstoryflag;
    public bool incStoryFlag { get { return incstoryflag; } }
    private bool incstoryflagindex;
    public bool incStoryFlagIndex { get { return incstoryflagindex; } }

    //1. Music
    private soundLibEntry[] music;
    public soundLibEntry[] Music { get { return music; } }

    //2. Player Coordinates
    private bool teleportsplayer = false;
    public bool teleportsPlayer { get { return teleportsplayer; } }
    private Vector3 playercoordinates;
    public Vector3 playerCoordinates { get { return playercoordinates; } }

    //2. Player Rotation
    private float playerrotation;
    public float playerRotation { get { return playerrotation; } }


    //2. Player Movement
    private playerMovement playermovement;
    public playerMovement playerMovement { get { return playermovement; } }

    //3. Cutscene
    private PlayableDirector cutscene;
    public PlayableDirector Cutscene { get { return cutscene; } }
    private GameObject cutscenecamera;
    public GameObject cutsceneCamera { get { return cutscenecamera; } }

    //4. Interactions
    private Interactable[] interactions;
    public Interactable[] Interactions { get { return interactions; } }

    //5. Collectible Item
    private string collectibleitemid;
    public string collectibleItemId { get { return collectibleitemid; } }

    //5. Map Warp
    private string mapwarp;
    public string mapWarp { get { return mapwarp; } }

    //5. Battles
    private battleData[] battlesdata;
    public battleData[] battlesData { get { return battlesdata; } }

    //6. Game Modes
    private bool storymode;
    public bool storyMode { get { return storymode; } }
    private bool explorationmode;
    public bool explorationMode { get { return explorationmode; } }

    //7. Repeat
    private bool repeat;
    public bool Repeat { get { return repeat; } }

    //Overwrite
    public void overwritePlayerCoordinates(Vector3 _playerCoordinates)
    {
        playercoordinates = _playerCoordinates;
    }
    //Constructor
    public eventData(storyFlags _storyFlag, int _storyFlagIndex, bool _incStoryFlag, bool _incStoryFlagIndex, soundLibEntry[] _Music, bool _teleportsPlayer, Vector3 _playerCoordinates, float _playerRotation, playerMovement _playerMovement, Interactable[] _Interactions, string _collectibleItemId, string _mapWarp, battleData[] _battlesData, bool _storyMode, bool _explorationMode, bool _Repeat)
    {
        storyflag = _storyFlag;
        storyflagindex = _storyFlagIndex;
        incstoryflag = _incStoryFlag;
        incstoryflagindex = _incStoryFlagIndex;
        music = _Music;
        teleportsplayer = _teleportsPlayer;
        playercoordinates = _playerCoordinates;
        playerrotation = _playerRotation;
        playermovement = _playerMovement;
        interactions = _Interactions;
        collectibleitemid = _collectibleItemId;
        mapwarp = _mapWarp;
        battlesdata = _battlesData;
        storymode = _storyMode;
        explorationmode = _explorationMode;
        repeat = _Repeat;
        isinitialized = true;
    }
    //Load Unity References
    public void loadUnityReferences(PlayableDirector _Cutscene, GameObject _cutsceneCamera)
    {
        cutscene = _Cutscene;
        cutscenecamera = _cutsceneCamera;
    }
    //Can Trigger
    public bool canTrigger()
    {
        bool storyFlagCheck = (storyFlag == storyFlags.Always || storyFlag == runtimeSaveData.Instance.storyFlag);
        bool storyFlagIndexCheck = (storyFlagIndex == -1 || storyFlagIndex == runtimeSaveData.Instance.storyFlagIndex);
        return ((gameState.Instance.currentMode == gameModes.StoryMode && storyMode && storyFlagCheck && storyFlagIndexCheck)
            || (gameState.Instance.currentMode == gameModes.ExplorationMode && explorationMode && storyFlagCheck && storyFlagIndexCheck));
    }
    public void setAsLoaded()
    {
        hasloaded = true;
    }
}
