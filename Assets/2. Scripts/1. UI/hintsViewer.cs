using TMPro;
using UnityEngine;
public class hintsViewer : MonoBehaviour
{
    //Story Flags
    private storyFlags Flag;
    private int Subflag;
    //Prefab
    [SerializeField]
    private GameObject hintPrefab;
    [SerializeField]
    private GameObject hintsContainer;
    //Visibility
    [SerializeField]
    private GameObject openHints;
    [SerializeField]
    private GameObject closedHints;
    private bool isOpen = false;
    void Start()
    {
        if (gameState.Instance.currentMode == gameModes.StoryMode) toggleVisibility();
        else
        {
            openHints.SetActive(false);
            closedHints.SetActive(false);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && gameState.Instance.currentMode == gameModes.StoryMode && gameState.Instance.currentState == gameStates.OverWorld)
        {
            toggleVisibility();
        }
    }
    //Update Script
    public void updateScript(storyFlags _Flag, int _Subflag)
    {
        string[] Hints = new string[8] { "No hints currently.", "", "", "", "", "", "", "" }; ;
        //Flag = _Flag;
        //Subflag = _Subflag;
        //if (Flag == storyFlags.Introduction)
        //{
        //    string firstHint = "Mouse rotates the Player's view.";
        //    string secondHint = "WASD & Arrow Keys move the Player.";
        //    string thirdHint = "Space Bar makes the Player jump.";
        //    string fourthHint = "Holding Left Shift sprints.";
        //    if (Subflag >= 1 && Subflag < 4) Hints = new string[8] { firstHint, secondHint, "", "", "", "", "", "" };
        //    else Hints = new string[8] { firstHint, secondHint, thirdHint, fourthHint, "", "", "", "" };
        //}
        //else if (Flag == storyFlags.Drifting)
        //{
        //    string firstHint = "Q interacts with objects the Player's facing.";
        //    string secondHint = "The game notifies whenever the Player's facing an interactable object.";
        //    string thirdHint = "You can read the Player's journal at Overworld Menu > Journal > (The journal entry).";
        //    if (Subflag == 1) Hints = new string[8] { firstHint, secondHint, "", "", "", "", "", "" };
        //    else if (Subflag == 2) Hints = new string[8] { firstHint, secondHint, thirdHint, "", "", "", "", "" };
        //}
        //else Hints = new string[8] { "", "", "", "", "", "", "", "" };
        //Generate the UI
        //Clear all the previous Instatiated Prefabs
        foreach (Transform button in hintsContainer.transform)
        {
            if (button.name == "Hint Text(Clone)") Destroy(button.gameObject);
        }
        for (int i = 0; i < Hints.Length; i++)
        {
            string currentHint = Hints[i];
            if (string.IsNullOrWhiteSpace(currentHint)) continue;
            //Instatiate & Parent Button Prefab
            GameObject newHint = Instantiate(hintPrefab);
            newHint.transform.SetParent(hintsContainer.transform, false);
            //Update the Button's Text
            newHint.GetComponentInChildren<TextMeshProUGUI>().SetText(currentHint);
            newHint.SetActive(true);
        }
    }
    //Toggle Visibility
    public void toggleVisibility()
    {
        isOpen = !isOpen;
        openHints.SetActive(false);
        closedHints.SetActive(false);
        if (isOpen) openHints.SetActive(true);
        else closedHints.SetActive(true);
    }
}
