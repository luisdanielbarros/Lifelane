using System.Collections.Generic;
using UnityEngine;
public class debugController : MonoBehaviour
{
    //Micro-State
    private bool showConsole = false;
    private bool showHelp = true;
    //Text Input
    private string textInput;
    //Commands
    public static debugCommand commandHelp;
    public static debugCommand commandExit;
    public static debugCommand<string> commandWarp;
    public static debugCommand<string> commandPlayerPerspective;
    public static debugCommand<string> commandPlayerMovementProfile;
    public static debugCommand<string> commandDebugGlasses;
    //Command List
    public List<object> commandList = new List<object>();
    //Messages
    ////Invalid Syntax
    private int errInvalidSyntaxId = 0;
    private int errUnexpectedArgumentId = 1;
    private string errInvalidSyntax = "Command syntax is invalid, you can consult the proper syntax via tha help command.";
    private string errUnexpectedArgument = "Unexpected argument, you can consult the available arguments via the help command.";
    //Help Command
    Vector2 commandHelpScroll;
    //Set up all the Commands
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //Commands
        ////Help
        commandHelp = new debugCommand("help", "Shows a list of commands.", "help", () =>
        {
            showHelp = !showHelp;
        });
        ////Help
        commandExit = new debugCommand("exit", "Close the debug GUI.", "exit", () =>
        {
            showConsole = !showConsole;
        });
        ////Warp
        commandWarp = new debugCommand<string>("warp", "Warps to a map. Options: deadend, main_plaza, river_route, shelter, residential_district, backrooms, render_room.", "warp <map_name>", (x) =>
        {
            warpData WarpData = null;
            switch (x)
            {
                case "deadend":
                    WarpData = jsonLoader.Instance.loadWarpData("City, Deadend, Warppoint 1", jsonWarpType.Literal);
                    break;
                case "main_plaza":
                    WarpData = jsonLoader.Instance.loadWarpData("City, Main Plaza, Warppoint 1", jsonWarpType.Literal);
                    break;
                case "river_route":
                    WarpData = jsonLoader.Instance.loadWarpData("City, River Route, Warppoint 1", jsonWarpType.Literal);
                    break;
                case "residential_district":
                    WarpData = jsonLoader.Instance.loadWarpData("City, Residential District, Warppoint 1", jsonWarpType.Literal);
                    break;
                case "shelter":
                    WarpData = jsonLoader.Instance.loadWarpData("City, Shelter, Warppoint 1", jsonWarpType.Literal);
                    break;
                case "backrooms":
                    WarpData = jsonLoader.Instance.loadWarpData("Backrooms, Warppoint 1", jsonWarpType.Literal);
                    break;
                case "render_room":
                    WarpData = jsonLoader.Instance.loadWarpData("Render Room, Warppoint 1", jsonWarpType.Literal);
                    break;
                default:
                    printError(errUnexpectedArgumentId);
                    break;
            }
            if (WarpData != null) loadingManager.Instance.LoadLevel(WarpData);
        });
        ////Player Perspective
        commandPlayerPerspective = new debugCommand<string>("perspective", "Change the player's perspective. Options: controlled, pov, mapped.", "perspective <perspective>", (x) =>
        {
            switch (x)
            {
                case "controlled":
                    cameraManager.Instance.playerPers = playerPerspective.Controlled;
                    break;
                case "pov":
                    cameraManager.Instance.playerPers = playerPerspective.POV;
                    break;
                case "mapped":
                    cameraManager.Instance.playerPers = playerPerspective.Mapped;
                    break;
                default:
                    printError(errUnexpectedArgumentId);
                    break;
            }
        });
        ////Player Movement Profile
        commandPlayerMovementProfile = new debugCommand<string>("movement_profile", "Change the player's movement profile. Options: normal, quiet.", "movement_profile <profile>", (x) =>
        {
            switch (x)
            {
                case "normal":
                    utilMono.Instance.getPlayerObject().GetComponent<playerController>().movementProfile = playerMovement.Normal;
                    break;
                case "quiet":
                    utilMono.Instance.getPlayerObject().GetComponent<playerController>().movementProfile = playerMovement.Normal;
                    break;
                default:
                    printError(errUnexpectedArgumentId);
                    break;
            }
        });
        ////Debug Glasses
        commandDebugGlasses = new debugCommand<string>("debug_glasses", "Filter the types of visible items. Options: normal, collisions.", "debug_glasses <glasses>", (x) =>
        {
            GameObject[] barrierList = GameObject.FindGameObjectsWithTag("Barrier");
            switch (x)
            {
                case "normal":
                    foreach (GameObject Barrier in barrierList) Barrier.GetComponent<MeshRenderer>().enabled = false;
                    break;
                case "collisions":
                    foreach (GameObject Barrier in barrierList) Barrier.GetComponent<MeshRenderer>().enabled = true;
                    break;
                default:
                    printError(errUnexpectedArgumentId);
                    break;
            }
        });
        //Command List
        commandList = new List<object> { commandHelp, commandExit, commandWarp, commandPlayerPerspective, commandPlayerMovementProfile, commandDebugGlasses };
    }
    //Set up the player Inputs
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) showConsole = !showConsole;
    }
    //Take care of the GUI & set up more player Inputs
    private void OnGUI()
    {
        //Check Game Mode
        if (gameState.Instance.currentMode != gameModes.ExplorationMode || gameState.Instance.currentState == gameStates.Cutscene) return;

        //Styles
        float defaultPadding = 5;
        ////Box Style
        GUIStyle boxStyle = new GUIStyle(GUI.skin.box)
        {
        };
        ////Text Style
        GUIStyle textStyle = new GUIStyle(GUI.skin.textField)
        {
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset((int)defaultPadding * 2, (int)defaultPadding, (int)defaultPadding, (int)defaultPadding),
            fontSize = 18
        };
        textStyle.normal.textColor = Color.white;
        ////Label Style
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset((int)defaultPadding, (int)defaultPadding, (int)defaultPadding, (int)defaultPadding),
            fontSize = 18
        };
        labelStyle.normal.textColor = Color.white;

        //General Variables
        float y = 0;
        float height = 0;

        //UI
        if (!showConsole)
        {
            ////Toggle Command Menu
            height = 64;
            string label = "Press Enter to toggle the command menu.";
            Rect labelRect = new Rect(0, y, Screen.width, height);
            GUI.Label(labelRect, label, labelStyle);
            GUI.Box(new Rect(0, y, Screen.width, height), "", boxStyle);
            y += height;
        }
        else
        {
            //Player Inputs
            if (Event.current.keyCode == KeyCode.Return && !string.IsNullOrWhiteSpace(textInput)) handleInput();

            ////Help
            if (showHelp)
            {
                height = 320;
                int listEntryHeight = 32;
                Rect viewport = new Rect(0, 0, Screen.width, listEntryHeight * commandList.Count);
                commandHelpScroll = GUI.BeginScrollView(new Rect(0, y + defaultPadding, Screen.width, height), commandHelpScroll, viewport);
                for (int i = 0; i < commandList.Count; i++)
                {
                    debugCommandBase Command = commandList[i] as debugCommandBase;
                    string label = $"{Command.Format} - {Command.Description}";
                    Rect labelRect = new Rect(0, listEntryHeight * i, viewport.width, listEntryHeight);
                    GUI.Label(labelRect, label, labelStyle);
                }
                GUI.EndScrollView();
                GUI.Box(new Rect(0, y, Screen.width, height), "", boxStyle);
                y += height;
            }

            ////Text Input
            height = 64;
            textInput = GUI.TextField(new Rect(0, y, Screen.width, height), textInput, textStyle);
            y += height;
        }
    }
    //Handle the player's Inputs
    private void handleInput()
    {
        string[] commandProperties = textInput.Split(' ');
        for (int i = 0; i < commandList.Count; i++)
        {
            debugCommandBase Command = commandList[i] as debugCommandBase;
            if (textInput.Contains(Command.Id))
            {
                if (Command as debugCommand != null) (Command as debugCommand).Invoke();
                else if (Command as debugCommand<int> != null) (Command as debugCommand<int>).Invoke(int.Parse(commandProperties[1]));
                else if (Command as debugCommand<string> != null) (Command as debugCommand<string>).Invoke(commandProperties[1].ToString());
            }
        }
        textInput = "";
    }
    //Prints errors related to wrongly typed commands
    private void printError(int Id)
    {
        switch (Id)
        {
            case 0:
                textInput = errInvalidSyntax;
                break;
            case 1:
                textInput = errUnexpectedArgument;
                break;
            default:
                errorManager.Instance.createErrorReport("debugController", "printError", errorType.switchCase);
                break;
        }
    }
}
