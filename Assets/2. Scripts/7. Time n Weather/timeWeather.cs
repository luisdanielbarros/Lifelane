using UnityEngine;
//[ExecuteInEditMode]
public class timeWeather : MonoBehaviour
{
    //Instance
    private static timeWeather instance = null;
    public static timeWeather Instance { get { return instance; } }
    //Weather
    private Weather currweather;
    public Weather currWeather { get { return currweather; } set { currweather = value; changeWeather(); } }
    //Particle System Container
    [SerializeField]
    private GameObject particleSystemContainer;
    //Current Particle System
    private GameObject currParticleSystem;
    //Prefabs
    [SerializeField]
    private GameObject prefabRain;
    void Awake()
    {
        //Instatiate
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void changeWeather()
    {
        if (currParticleSystem != null) Destroy(currParticleSystem.gameObject);
        switch (currweather)
        {
            case Weather.Rain:
                currParticleSystem = Instantiate(prefabRain, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                currParticleSystem.transform.parent = particleSystemContainer.transform;
                currParticleSystem.transform.localPosition = new Vector3(0, 8, 0);
                currParticleSystem.transform.localRotation = Quaternion.AngleAxis(90, Vector3.right);
                break;
        }

    }
}
