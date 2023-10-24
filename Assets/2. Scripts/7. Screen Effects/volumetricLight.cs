using UnityEngine;
public class volumetricLight : MonoBehaviour
{
    [SerializeField]
    private Color lightColor;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "MainCamera")
        {
            Debug.Log("Collision Enter w/ " + other.tag);
            screenEffectsManager.Instance.createFogEffect(lightColor);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "MainCamera")
        {
            Debug.Log("Collision Leave w/ " + other.tag);
            screenEffectsManager.Instance.stopFogEffect();
        }
    }
}
