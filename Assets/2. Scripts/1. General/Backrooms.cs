using UnityEngine;
public class Backrooms : MonoBehaviour
{
    void Start()
    {
        cameraManager.Instance.playerPers = playerPerspective.POV;
    }
}
