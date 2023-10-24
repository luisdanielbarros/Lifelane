using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public void Resume()
    {
        gameState.Instance.setGameState(gameStates.OverWorld);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
