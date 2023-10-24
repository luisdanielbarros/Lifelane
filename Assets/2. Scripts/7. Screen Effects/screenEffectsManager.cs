using UnityEngine;
using UnityEngine.UI;

public class screenEffectsManager : MonoBehaviour
{
    //Instance
    private static screenEffectsManager instance = null;
    public static screenEffectsManager Instance { get { return instance; } }
    //Fog Effect
    [SerializeField]
    private GameObject fogEffect;
    private bool isfoggy;
    private bool isFoggy { get { return isfoggy; } set { isfoggy = value; fogEffect.SetActive(value); } }
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
    void Start()
    {
        //Fog Effect
        isFoggy = false;
    }
    public void createFogEffect(Color fogColor)
    {
        fogColor.a = 0.25f;
        fogEffect.GetComponent<Image>().color = fogColor;
        isFoggy = true;
    }
    public void stopFogEffect()
    {
        isFoggy = false;
    }
}
