using UnityEngine;
public class virtualHub : MonoBehaviour
{
    //Game State Dependency Injection
    [SerializeField]
    private GameObject dialoguePanel;
    [SerializeField]
    private GameObject overworldMenu;
    [SerializeField]
    private GameObject inventoryItemViewer;
    [SerializeField]
    private GameObject journalMenu;
    [SerializeField]
    private GameObject battleMenu;
    [SerializeField]
    private GameObject goalsHintsViewer;
    //Save Manager Dependency Injection
    [SerializeField]
    private GameObject playerObject;
    //Save Data Dependency Injection
    [SerializeField]
    private goalsViewer goalsView;
    [SerializeField]
    private hintsViewer hintsView;
    void Start()
    {
        gameState.Instance.dependencyInjection(dialoguePanel, overworldMenu, inventoryItemViewer, journalMenu, battleMenu, goalsHintsViewer);
        runtimeSaveData.Instance.dependencyInjection(goalsView, hintsView);
        //Save Manager populates Runtime Save Data
        saveManager.Instance.dependencyInjection(playerObject);
    }
}
