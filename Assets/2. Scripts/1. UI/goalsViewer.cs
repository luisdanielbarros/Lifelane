using TMPro;
using UnityEngine;
public class goalsViewer : MonoBehaviour
{
    //Story Flags
    private storyFlags Flag;
    private int Subflag;
    //UI
    [SerializeField]
    private TextMeshProUGUI goalText;
    //Visibility
    [SerializeField]
    private GameObject openGoals;
    [SerializeField]
    private GameObject closedGoals;
    private bool isOpen = false;
    void Start()
    {
        if (gameState.Instance.currentMode == gameModes.StoryMode) toggleVisibility();
        else
        {
            openGoals.SetActive(false);
            closedGoals.SetActive(false);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && gameState.Instance.currentMode == gameModes.StoryMode && gameState.Instance.currentState == gameStates.OverWorld)
        {
            toggleVisibility();
        }
    }
    //Update Script
    public void updateScript(storyFlags _Flag, int _Subflag)
    {
        goalText.SetText("No goals currently.");
    }
    //Toggle Visibility
    public void toggleVisibility()
    {
        isOpen = !isOpen;
        openGoals.SetActive(false);
        closedGoals.SetActive(false);
        if (isOpen) openGoals.SetActive(true);
        else closedGoals.SetActive(true);
    }
}
