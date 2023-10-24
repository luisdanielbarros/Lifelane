using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour
{
    //Instance
    private static cameraManager instance = null;
    public static cameraManager Instance { get { return instance; } }
    //Controlled & POV Cameras
    [SerializeField]
    private GameObject firstPersonCamera, thirdPersonCamera, frontCamera, backCamera, rightCamera, leftCamera;
    private GameObject[] camerasList;
    private int camerasListIndex = 0;
    //Mapped Cameras
    private CinemachineClearShot mappedCamerasManager;
    private List<GameObject> mappedCameras = new List<GameObject>();
    //Camera References
    [SerializeField]
    private Transform firstPersonCameraFollow, thirdPersonCameraFollowLookAt;
    //Player Perspective
    private playerPerspective playerpers = playerPerspective.Controlled;
    public playerPerspective playerPers { get { return playerpers; } set { playerpers = value; resetCameras(); } }
    void Start()
    {
        //Instatiate
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        //Controlles & POV Cameras
        camerasList = new GameObject[6] { firstPersonCamera, thirdPersonCamera, frontCamera, rightCamera, backCamera, leftCamera };
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && (playerPers == playerPerspective.Controlled || playerPers == playerPerspective.POV) && gameState.Instance.currentState == gameStates.OverWorld)
        {
            int previousIndex = camerasListIndex;
            camerasListIndex++;
            if (camerasListIndex >= camerasList.Length) camerasListIndex = 0;
            camerasList[camerasListIndex].SetActive(true);
            camerasList[previousIndex].SetActive(false);
        }
    }
    //Reset Active Camera
    public void resetCameras()
    {
        //Player Perspective
        switch (playerPers)
        {
            case playerPerspective.Controlled:
                for (int i = 0; i < camerasList.Length; i++) camerasList[i].SetActive(false);
                for (int i = 0; i < mappedCameras.Count; i++) mappedCameras[i].SetActive(false);
                if (mappedCamerasManager != null) mappedCamerasManager.gameObject.SetActive(false);
                camerasListIndex = 1;
                camerasList[camerasListIndex].SetActive(true);
                camerasList[camerasListIndex].GetComponent<CinemachineFreeLook>().Follow = thirdPersonCameraFollowLookAt;
                camerasList[camerasListIndex].GetComponent<CinemachineFreeLook>().LookAt = thirdPersonCameraFollowLookAt;
                break;
            case playerPerspective.POV:
                for (int i = 0; i < camerasList.Length; i++) camerasList[i].SetActive(false);
                for (int i = 0; i < mappedCameras.Count; i++) mappedCameras[i].SetActive(false);
                if (mappedCamerasManager != null) mappedCamerasManager.gameObject.SetActive(false);
                camerasListIndex = 0;
                camerasList[camerasListIndex].SetActive(true);
                camerasList[camerasListIndex].GetComponent<CinemachineVirtualCamera>().Follow = firstPersonCameraFollow;
                break;
            case playerPerspective.Mapped:
                for (int i = 0; i < camerasList.Length; i++) camerasList[i].SetActive(false);
                for (int i = 0; i < mappedCameras.Count; i++) mappedCameras[i].SetActive(true);
                if (mappedCamerasManager != null) mappedCamerasManager.gameObject.SetActive(true);
                break;
        }
        //Player Overworld Model
        loadingManager.Instance.updatePlayerModel();
    }
    //Add Mapped Camera Manager
    public void addMappedCamerasManager(GameObject _Manager)
    {
        _Manager.GetComponent<CinemachineClearShot>().LookAt = utilMono.Instance.getPlayerGraphics().transform;
        mappedCamerasManager = _Manager.GetComponent<CinemachineClearShot>();
    }
    //Add Mapped Cameras
    public void addMappedCamera(GameObject _Camera)
    {
        mappedCameras.Add(_Camera);
    }
    //Clear Mapped Cameras
    public void clearMappedCameras()
    {
        for (int i = 0; i < mappedCameras.Count; i++)
        {
            Destroy(mappedCameras[i].gameObject);
        }
        mappedCameras.Clear();
    }
}
