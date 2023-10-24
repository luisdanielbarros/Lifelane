using UnityEngine;

public class mouseCursor : MonoBehaviour
{
    [SerializeField]
    private Texture2D cursorIdle, cursorClick, cursorHover;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (gameState.Instance.currentState != gameStates.MainMenu &&
            gameState.Instance.currentState != gameStates.LoadingScreen &&
            gameState.Instance.currentState != gameStates.PauseMenu)
            return;
        //Sprite
        if (Input.GetMouseButtonDown(0)) Cursor.SetCursor(cursorClick, new Vector2(0, 0), CursorMode.Auto);
        else if (Input.GetMouseButtonUp(0)) Cursor.SetCursor(cursorIdle, new Vector2(0, 0), CursorMode.Auto);

    }
}
