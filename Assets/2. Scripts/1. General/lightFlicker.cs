using System.Collections;
using UnityEngine;
public enum lightFlickerMode { Slow, Medium, Fast }
public class lightFlicker : MonoBehaviour
{
    private bool isFlickering = false;
    private float timeDelay;
    [SerializeField]
    private bool originalState = true;
    [SerializeField]
    private lightFlickerMode flickerMode = lightFlickerMode.Slow;
    //Objects
    [SerializeField]
    private Light[] targetLightSources;
    [SerializeField]
    private GameObject targetModel;
    [SerializeField]
    private int targetMaterialIndex;
    [SerializeField]
    private Material originalMaterial;
    [SerializeField]
    private Material lightOffMaterial;
    void Update()
    {
        if (!isFlickering)
        {
            StartCoroutine(FlickerLight());
        }
    }
    IEnumerator FlickerLight()
    {
        isFlickering = true;

        //Calculate the time delays
        float maxFlickerDelay = 0.02f;
        float maxTimeDelay = 8f;
        switch (flickerMode)
        {
            case lightFlickerMode.Slow:
                maxTimeDelay = 8f;
                break;
            case lightFlickerMode.Medium:
                maxTimeDelay = 2f;
                break;
            case lightFlickerMode.Fast:
                maxTimeDelay = 0.05f;
                break;
            default:
                errorManager.Instance.createErrorReport("lightFlicker", "FlickerLight", errorType.switchCase);
                break;
        }

        //Set the new list of materials
        MeshRenderer targetModelRenderer = targetModel.GetComponent<MeshRenderer>();
        Material[] newMaterials = new Material[targetModelRenderer.materials.Length];
        for (int i = 0; i < targetModelRenderer.materials.Length; i++)
        {
            if (i != targetMaterialIndex) newMaterials[i] = targetModelRenderer.materials[i];
        }
        newMaterials[targetMaterialIndex] = lightOffMaterial;

        //Turn to its abnormal state and wait its flicker delay before returning back to its normal state
        for (int i = 0; i < targetLightSources.Length; i++) targetLightSources[i].enabled = !originalState;
        targetModel.GetComponent<MeshRenderer>().materials = newMaterials;
        timeDelay = Random.Range(0.01f, maxFlickerDelay);
        yield return new WaitForSeconds(timeDelay);

        //Reset the list of materials
        newMaterials[targetMaterialIndex] = originalMaterial;

        //Return back to its normal state and wait its flicker mode delay before flickering again
        for (int i = 0; i < targetLightSources.Length; i++) targetLightSources[i].enabled = originalState;
        targetModel.GetComponent<MeshRenderer>().materials = newMaterials;
        timeDelay = Random.Range(0.01f, maxTimeDelay);
        yield return new WaitForSeconds(timeDelay);

        //Get ready to flicker again
        isFlickering = false;
    }
}
