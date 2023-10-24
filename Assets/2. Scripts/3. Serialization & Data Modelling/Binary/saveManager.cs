using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class saveManager : MonoBehaviour
{
    //Instance
    private static saveManager instance = null;
    public static saveManager Instance { get { return instance; } }
    //Objects to track
    private GameObject playerObject;
    //List of levels
    private string firstLevel = "City, Deadend, Warppoint 1";
    private string secondLevel = "City, Main Plaza, Warppoint 1";
    private string thirdLevel = "Shelter, Warppoint 1";
    private string Backrooms = "Backrooms, Warppoint 1";
    //UI
    [SerializeField]
    private Transform saveFilesParent;
    [SerializeField]
    private GameObject loadBtnPrefab;
    //Save Files
    private string[] saveFiles;
    private int savefile;
    public int saveFile { get { return savefile; } }
    //Delete Mode
    private bool deleteMode;
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
    #region Load UI
    //Get Save Files
    private void getSaveFiles()
    {
        string saveFolder = Application.persistentDataPath + "/saves";
        if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);
        saveFiles = Directory.GetFiles(saveFolder);
        //if there are less than 4 save files, add an empty save file
        if (saveFiles.Length <= 4)
        {
            string newSaveFileString = "New Save File";
            bool emptySaveFileFound = false;
            for (int i = 0; i < saveFiles.Length; i++)
            {
                string[] savePath = saveFiles[i].Split('/');
                string saveFileName = savePath[savePath.Length - 1];
                if (saveFileName == newSaveFileString) emptySaveFileFound = true;
            }
            if (!emptySaveFileFound)
            {
                Save("", newSaveFileString);
            }
        }
        Array.Reverse(saveFiles);
        //Create/Reset the exploration mode save file
        Delete("Exploration Mode", false);
        Save("Exploration Mode", "Exploration Mode");
        saveFiles = Directory.GetFiles(saveFolder);
    }
    //Show Save Files
    public void showSaveFiles()
    {
        //Clear all the previous Instatiated Prefabs
        foreach (Transform button in saveFilesParent)
        {
            if (button.name == "Load Save Button(Clone)") Destroy(button.gameObject);
        }
        //Get Save Files in Directory
        getSaveFiles();
        for (int i = 0; i < saveFiles.Length; i++)
        {
            //Get Save Name from Path
            string[] savePath = saveFiles[i].Split('/');
            string saveFileName = savePath[savePath.Length - 1];
            savePath = saveFileName.Split('\\');
            saveFileName = savePath[savePath.Length - 1];
            savePath = saveFileName.Split('.');
            saveFileName = savePath[0];
            if (saveFileName == "Exploration Mode") continue;
            //Instatiate & Parent Button Prefab
            GameObject btnObj = Instantiate(loadBtnPrefab);
            btnObj.transform.SetParent(saveFilesParent.transform, false);
            //Update the Button's Text
            btnObj.GetComponentInChildren<TextMeshProUGUI>().SetText(saveFileName);
            //Add the Event Listeners
            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                gameStates previousGameState = gameState.Instance.previousState;
                switch (previousGameState)
                {
                    //Load
                    case gameStates.MainMenu:
                        //Delete
                        if (deleteMode) Delete(saveFileName);
                        else
                        {
                            runtimeSaveData.Instance.loadSaveFile((persistentSaveData)serializationManager.Load(saveFileName));
                            loadingManager.Instance.loadVirtualHub();
                        }
                        break;
                    //Save
                    case gameStates.overworldMenu:
                        //Delete
                        if (deleteMode) Delete(saveFileName);
                        else
                        {
                            string newSaveFileName = DateTime.UtcNow.ToString("HH*1mm*2ss*3 dd MMMM, yyyy");
                            newSaveFileName = newSaveFileName.Replace("*1", "h");
                            newSaveFileName = newSaveFileName.Replace("*2", "m");
                            newSaveFileName = newSaveFileName.Replace("*3", "s");
                            if (saveFileName != newSaveFileName)
                            {
                                Save(saveFileName, newSaveFileName);
                                gameState.Instance.setGameState(gameStates.overworldMenu);
                            }
                        }
                        break;
                    default:
                        errorManager.Instance.createErrorReport("saveManager", "showSaveFiles", errorType.switchCase);
                        break;
                }
            });
        }
    }
    //Toggle Delete Mode
    public bool toggleDeleteMode()
    {
        deleteMode = !deleteMode;
        return deleteMode;
    }
    #endregion
    #region CRUD
    //Save
    private void Save(string oldSaveFileName, string newSaveFileName)
    {

        //If the player object has been instatiated, meaning if the player has been playing in story mode, save all the proper information
        if (playerObject != null) runtimeSaveData.Instance.Save(loadingManager.Instance.currentLevelIndex, playerObject.transform, Journal.Instance.getSaveData());
        //Else if the player hasn't started playing yet, meaning this method is simply targetting dummy save files
        else
        {
            //Set the level index for the start of the game, which varies whether the save file is meant for story or exploration mode
            int startingGameLevelIndex = 2;
            if (newSaveFileName == "Exploration Mode") startingGameLevelIndex = 7;
            runtimeSaveData.Instance.Save(startingGameLevelIndex, null, new storedItemData[] { });
        }
        serializationManager.Save(oldSaveFileName, newSaveFileName, runtimeSaveData.Instance.saveFileData);
    }
    private void Delete(string saveFileName, bool updateUI = true)
    {
        serializationManager.Delete(saveFileName);
        if (updateUI) showSaveFiles();
    }
    #endregion
    #region Dependency Injections
    //Dependency Injection
    public void dependencyInjection(GameObject _playerObject)
    {
        //Link to Player Object
        if (_playerObject == null || playerObject != null)
        {
            errorManager.Instance.createErrorReport("saveManager", "dependencyInjection", errorType.nullReference);
            return;
        }
        playerObject = _playerObject;

        //Load Team Data
        runtimeSaveData.Instance.loadTeamData();

        //Load Overworld
        runtimeSaveData.Instance.loadOverworld(new string[] { null, null, firstLevel, secondLevel, thirdLevel, null, null, Backrooms });
    }
    #endregion
}
