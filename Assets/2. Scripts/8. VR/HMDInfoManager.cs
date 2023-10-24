using UnityEngine;
using UnityEngine.XR;
public class HMDInfoManager : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (!XRSettings.isDeviceActive)
        {
            Debug.Log("No VR Headset plugged.");
        }
        else if (XRSettings.isDeviceActive && (XRSettings.loadedDeviceName == "Mock HMD" || XRSettings.loadedDeviceName == "MockHMD Display"))
        {
            Debug.Log("Mock VR Headset plugged.");
        }
        else
        {
            Debug.Log("VR Headset " + XRSettings.loadedDeviceName + " plugged.");
        }
    }
}
