using System;
using UnityEngine;
[Serializable]
public class runtimeSaveData : MonoBehaviour
{
    //Instance
    private static runtimeSaveData instance = null;
    public static runtimeSaveData Instance { get { return instance; } }

    //Cards
    ////Player Data
    private characterData playerdata;
    public characterData playerData { get { return playerdata; } }
    ////Partner Data
    private characterData partnerdata;
    public characterData partnerData { get { return partnerdata; } }

    //Battle Entities
    ////Player Battle Entity
    private battleEntity playerbattleentity;
    public battleEntity playerBattleEntity { get { return playerbattleentity; } }
    ////Partner Battle Entity
    private battleEntity partnerbattlentity;
    public battleEntity partnerBattleEntity { get { return partnerbattlentity; } }

    //Persistent Save Data
    private persistentSaveData savefiledata = new persistentSaveData(storyFlags.Introduction, 0, 0, 0, 0, 3, 0f, 0);
    public persistentSaveData saveFileData { get { return savefiledata; } }

    //Story Flags
    public storyFlags storyFlag { get { return savefiledata.storyFlag; } set { savefiledata.storyFlag = value; savefiledata.storyFlagIndex = 0; updateStoryDependentSystems(); } }
    public int storyFlagIndex { get { return savefiledata.storyFlagIndex; } set { savefiledata.storyFlagIndex = value; updateStoryDependentSystems(); } }
    //Story Dependent Systems
    [SerializeField]
    private LoadingScreen loadingScreen;
    private goalsViewer goalsView;
    private hintsViewer hintsView;
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
    #region Load
    //Load Save File, called by the Save Manager first
    public void loadSaveFile(persistentSaveData _saveFileData)
    {
        savefiledata = _saveFileData;
        loadingScreen.updateScript(storyFlag, storyFlagIndex);
    }
    //Load Team Stats, called by the Save Manager second
    public void loadTeamData()
    {
        //Player
        ////Load Player's character data
        string playerDataFile = savefiledata.playerDataFile;
        playerdata = jsonLoader.Instance.loadCharacterData(playerDataFile);

        ////Load Player's battle data
        playerbattleentity = jsonLoader.Instance.loadBattleEntity(new string[] { playerDataFile, savefiledata.charMainExperience.ToString() });

        //Partner
        ////If there's a Partner
        string partnerDataFile = savefiledata.partnerDataFile;
        if (!string.IsNullOrEmpty(partnerDataFile))
        {
            ////Load Partner's character data
            partnerdata = jsonLoader.Instance.loadCharacterData(savefiledata.partnerDataFile);
            ////Load Partner's battle data
            string partnerExperience;
            switch (partnerDataFile)
            {
                case "marie":
                    partnerExperience = savefiledata.charMarieExperience.ToString();
                    break;
                default:
                    partnerExperience = "0";
                    break;
            }
            partnerbattlentity = jsonLoader.Instance.loadBattleEntity(new string[] { partnerDataFile, partnerExperience });
        }

        //Journal
        Journal.Instance.loadSaveData(saveFileData.Journal);
    }
    //Load Overworld, called by the Save Manager third
    public void loadOverworld(string[] mapList)
    {
        gameModes currentMode = gameState.Instance.currentMode;
        //Story Mode
        if (currentMode == gameModes.StoryMode)
        {
            //Note: The Save File's Level Index follows the Level Loader's Build Index to avoid confusion
            int levelIndex = saveFileData.levelIndex;
            switch (levelIndex)
            {
                case 2:
                case 3:
                case 4:
                    warpData warpData = jsonLoader.Instance.loadWarpData(mapList[levelIndex], jsonWarpType.Literal);
                    loadingManager.Instance.LoadLevel(warpData);
                    break;
                default:
                    Debug.LogError("Error: Unexpected levelIndex of " + levelIndex.ToString() + " in runtimeSaveData.cs, method loadOverworld.");
                    break;
            }
        }
        //Exploration Mode
        else if (currentMode == gameModes.ExplorationMode)
        {
            warpData Backrooms = jsonLoader.Instance.loadWarpData("Backrooms, Warppoint 1", jsonWarpType.Literal);
            loadingManager.Instance.LoadLevel(Backrooms);
        }
    }
    //Load OverWorld Data, called by the Level Loader
    public void loadOverworldData(bool isOverworldLoaded)
    {
        //Player Model
        loadOverworldPlayerModel();
        //If the OverWorld isn't loaded yet, load every OverWorld Data
        //In other words, if the OverWorld has already been loaded, load only the destructible OverWorld Data, independent of Saved Data
        if (!isOverworldLoaded)
        {
            //Player Coordinates
            Vector3 playerPosition = new Vector3(saveFileData.mapCoordsX, saveFileData.mapCoordsY, saveFileData.mapCoordsZ);
            GameObject emptyGameObject = new GameObject();
            Transform playerTransform = emptyGameObject.transform;
            playerTransform.position = playerPosition;
            utilMono.Instance.setPlayerCoordinates(playerTransform.position);
            //Player Rotation
            float playerRotation = saveFileData.playerRotation;
            utilMono.Instance.setPlayerRotation(playerRotation);
        }
    }
    private void loadOverworldPlayerModel()
    {
        //Player Model
        utilMono.Instance.addToPlayerModel(playerdata.Model.Physic);
        utilMono.Instance.addToPlayerModel(playerdata.Model.Head);
        utilMono.Instance.addToPlayerModel(playerdata.Model.Hair);
        utilMono.Instance.addToPlayerModel(playerdata.Model.Eyes);
        utilMono.Instance.addToPlayerModel(playerdata.Model.Jacket);
        utilMono.Instance.addToPlayerModel(playerdata.Model.Shirt);
        utilMono.Instance.addToPlayerModel(playerdata.Model.Pants);
        utilMono.Instance.addToPlayerModel(playerdata.Model.Socks);
        utilMono.Instance.addToPlayerModel(playerdata.Model.Shoes);
        modelData[] Acessories = playerdata.Model.Acessories;
        for (int i = 0; i < Acessories.Length; i++)
        {
            modelData Acessory = Acessories[i];
            utilMono.Instance.addToPlayerModel(Acessory);
        }
    }
    #endregion
    #region Dependency Injections
    //Dependency Injection
    public void dependencyInjection(goalsViewer _goalsView, hintsViewer _hintsView)
    {
        //Goals & Hints System
        goalsView = _goalsView;
        hintsView = _hintsView;
    }
    #endregion
    #region Update
    //Update Story Dependent Systems
    private void updateStoryDependentSystems()
    {
        loadingScreen.updateScript(storyFlag, storyFlagIndex);
        goalsView.updateScript(storyFlag, storyFlagIndex);
        hintsView.updateScript(storyFlag, storyFlagIndex);
    }
    #endregion
    //On Save
    public void Save(int _levelIndex, Transform _playerTransform, storedItemData[] _Journal)
    {
        savefiledata.Save(_levelIndex, _playerTransform, _Journal);
    }
}
