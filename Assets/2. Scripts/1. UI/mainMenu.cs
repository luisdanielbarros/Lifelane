using UnityEngine;
public class mainMenu : MonoBehaviour
{
    public void Play()
    {
        gameState.Instance.currentMode = gameModes.StoryMode;
        gameState.Instance.setGameState(gameStates.SaveMenu);
    }
    public void Explore()
    {
        gameState.Instance.currentMode = gameModes.ExplorationMode;
        gameState.Instance.setGameState(gameStates.SaveMenu);
        runtimeSaveData.Instance.loadSaveFile((persistentSaveData)serializationManager.Load("Exploration Mode"));
        loadingManager.Instance.loadVirtualHub();
    }
    public void Options()
    {
        gameState.Instance.setGameState(gameStates.OptionsMenu);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
