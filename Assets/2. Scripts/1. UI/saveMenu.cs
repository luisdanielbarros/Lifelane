using TMPro;
using UnityEngine;
public class saveMenu : MonoBehaviour
{
    //Delete Mode
    [SerializeField]
    private TextMeshProUGUI deleteButtonText;
    //Back
    public void Back()
    {
        gameStates previousGameState = gameState.Instance.previousState;
        if (previousGameState == gameStates.MainMenu || previousGameState == gameStates.overworldMenu)
        {
            gameState.Instance.setGameState(previousGameState);
        }
    }
    //Delete Mode
    public void deleteMode()
    {
        bool deleteMode = saveManager.Instance.toggleDeleteMode();
        if (deleteMode) deleteButtonText.SetText("Delete mode. (toggle)");
        else deleteButtonText.SetText("Selection mode. (toggle)");
    }
}
