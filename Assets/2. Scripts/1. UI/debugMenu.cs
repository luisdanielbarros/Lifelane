using UnityEngine;

public class debugMenu : MonoBehaviour
{
    //Barriers
    private bool barriersVibility = false;
    //Toggle Barriers
    public void toggleBarriers()
    {
        barriersVibility = !barriersVibility;
        GameObject[] barrierList = GameObject.FindGameObjectsWithTag("Barrier");
        foreach (GameObject Barrier in barrierList)
        {
            Barrier.GetComponent<MeshRenderer>().enabled = barriersVibility;
        }
    }
    //Time
    private bool timeScaled = false;
    public void toggleTimeScale()
    {
        if (!timeScaled)
        {
            Time.timeScale = 4;
            timeScaled = true;
        }
        else Time.timeScale = 1;
        gameState.Instance.setDebugIsTimeScaled(timeScaled);
    }
}
