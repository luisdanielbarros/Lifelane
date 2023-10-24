using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class utilMono : MonoBehaviour
{
    //Instance
    private static utilMono instance = null;
    public static utilMono Instance { get { return instance; } }
    //Player
    [SerializeField]
    private GameObject playerObj;
    [SerializeField]
    private GameObject playerGraphics;
    //Particle Effects
    [SerializeField]
    private GameObject particleGlowEffect;
    //Optimization Variables
    playerController playerCtrl;
    //Runtime Variables
    private string runtimeAnimCntrlPath;

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
        DontDestroyOnLoad(playerObj);
        //Runtime Variables
        runtimeAnimCntrlPath = "Models/Characters/Body/Body Anim Ctrl";
    }
    #region Player
    //Get Player Coordinates
    public Transform getPlayerCoordinates()
    {
        return playerObj.transform;
    }
    //Set Player Coordinates
    public void setPlayerCoordinates(Vector3 _Position)
    {
        playerCtrl = playerObj.GetComponent<playerController>();
        StartCoroutine(setPlayerCoordinatesCoroutine(_Position));
    }
    private IEnumerator setPlayerCoordinatesCoroutine(Vector3 _Position)
    {
        playerCtrl.stopAnimationWalking();
        playerCtrl.enabled = false;
        playerObj.transform.localPosition = _Position;
        yield return new WaitForSeconds(0.25f);
        playerCtrl.enabled = true;
    }
    //Set Player Rotation
    public void setPlayerRotation(float _Rotation)
    {
        playerObj.transform.localRotation = Quaternion.AngleAxis(_Rotation, Vector3.up);
    }
    //Set Player Movement
    public void setPlayerMovement(playerMovement _Movement)
    {
        if (_Movement == playerMovement.Unchanged) return;
        playerObj.GetComponent<playerController>().movementProfile = _Movement;
    }
    //Get Player Object
    public GameObject getPlayerObject()
    {
        return playerObj;
    }
    //Get Player Graphics
    public GameObject getPlayerGraphics()
    {
        return playerGraphics;
    }
    //Add to Player Model
    public void addToPlayerModel(modelData _modelData)
    {
        if (_modelData.modelId == "none") return;
        //Add the Player Model
        string modelPath = "Models/Characters/" + _modelData.Path;
        ////Load Resource
        GameObject objToSpawn = (GameObject)LoadFromFile(modelPath);
        ////Load Materials
        bool areMaterialsChangeable = true;
        Material Material1 = null, Material2 = null, Material3 = null;
        if (_modelData.Material1 == modelDataMaterial.Unchangeable || _modelData.Material2 == modelDataMaterial.Unchangeable || _modelData.Material3 == modelDataMaterial.Unchangeable) areMaterialsChangeable = false;
        else
        {
            Material1 = LoadMaterialFromName(_modelData.Material1);
            Material2 = LoadMaterialFromName(_modelData.Material2);
            Material3 = LoadMaterialFromName(_modelData.Material3);
        }
        ////Instantiate it and attach it to its parent
        Transform playerCharacterGraphics = playerObj.transform.Find("Player Graphics").Find("Character");
        GameObject Obj = Instantiate(objToSpawn, _modelData.modelPosition, _modelData.modelRotation, playerCharacterGraphics);
        ////Change its material
        if (areMaterialsChangeable) Obj.GetComponentInChildren<SkinnedMeshRenderer>().materials = new Material[] { Material1, Material2, Material3 };
        ////Change its transform
        Obj.transform.localPosition = _modelData.modelPosition;
        Obj.transform.localRotation = _modelData.modelRotation;
        Obj.transform.localScale = _modelData.modelScale;

        //Add the Animator to the model
        Animator objAnimator = Obj.AddComponent<Animator>();
        objAnimator.runtimeAnimatorController = (RuntimeAnimatorController)LoadFromFile(runtimeAnimCntrlPath);

        //Add the Animator to the Player Controller
        if (objAnimator != null) playerObj.GetComponent<playerController>().addAnimator(objAnimator);
    }
    //Destroy Player Model
    public void destroyPlayerModel()
    {
        //Remove all the Animators from the Player Controller
        playerObj.GetComponent<playerController>().resetAnimators();

        //Delete the Player Model
        Transform playerCharacterGraphics = playerObj.transform.Find("Player Graphics").Find("Character");
        foreach (Transform child in playerCharacterGraphics.transform) GameObject.Destroy(child.gameObject);
    }
    #endregion
    #region Fade
    //Fade UI In or Out
    public IEnumerator fadeUI(object UIElem, string type)
    {
        //Set Variables
        float Counter = 0f;
        float Duration = 1f;
        //Set Color
        float colorRedChannel = 157f / 255f;
        float colorBlueChannel = 231f / 255f;
        float colorGreenChannel = 215f / 255f;
        //Set Invisible
        if (type == "Image") ((Image)UIElem).color = new Color(colorRedChannel, colorBlueChannel, colorGreenChannel, 0);

        //Fade In
        while (Counter < Duration)
        {
            gameStates currentGameSate = gameState.Instance.currentState;
            if (currentGameSate != gameStates.OverWorld && currentGameSate != gameStates.overworldMenu)
            {
                if (type == "Image") ((Image)UIElem).color = new Color(colorRedChannel, colorBlueChannel, colorGreenChannel, 0);
                else if (type == "Text") ((TextMeshProUGUI)UIElem).alpha = 0;
                yield return null;
                continue;
            }
            Counter += Time.deltaTime;
            if (type == "Image") ((Image)UIElem).color = new Color(colorRedChannel, colorBlueChannel, colorGreenChannel, Mathf.Lerp(0, 0.5f, Counter / Duration));
            else if (type == "Text") ((TextMeshProUGUI)UIElem).alpha = Mathf.Lerp(0, 1, Counter / Duration);
            yield return null;
        }
        //Wait & Reset
        Counter = 0;
        yield return new WaitForSeconds(2);
        //Fade Out
        while (Counter < Duration)
        {
            gameStates currentGameSate = gameState.Instance.currentState;
            if (currentGameSate != gameStates.OverWorld && currentGameSate != gameStates.overworldMenu)
            {
                if (type == "Image") ((Image)UIElem).color = new Color(colorRedChannel, colorBlueChannel, colorGreenChannel, 0);
                else if (type == "Text") ((TextMeshProUGUI)UIElem).alpha = 0;
                yield return null;
                continue;
            }
            Counter += Time.deltaTime;
            if (type == "Image") ((Image)UIElem).color = new Color(colorRedChannel, colorBlueChannel, colorGreenChannel, Mathf.Lerp(0.5f, 0, Counter / Duration));
            else if (type == "Text") ((TextMeshProUGUI)UIElem).alpha = Mathf.Lerp(1, 0, Counter / Duration);
            yield return null;
        }
    }
    #endregion
    #region Particle Effects
    //Glow Effect
    public void createParticleGlowEffect(GameObject _Container)
    {
        GameObject particleEffectObj = Instantiate(particleGlowEffect);
        particleEffectObj.transform.SetParent(_Container.transform, false);
        particleEffectObj.transform.localPosition = new Vector3(0, 0, 0);
        particleEffectObj.transform.localRotation = Quaternion.AngleAxis(0, Vector3.up);
    }
    #endregion
    #region NPC
    //Search Memory
    //public interactionFeedback searchMemory(string uniqueId, LinkedList<characterMemory> charMemory)
    //{
    //    //Remember the GameObject
    //    bool Remembered = false;
    //    int playerIndex = -1, playerProgress = 0;
    //    for (int i = 0; i < charMemory.Count; i++)
    //    {
    //        //If the GameObject matches one in the memory
    //        if (charMemory.ElementAt(i).uniqueId == uniqueId)
    //        {
    //            Remembered = true;
    //            playerIndex = i;
    //            charRelation[] currentRelation = charMemory.ElementAt(i).charRelations;
    //            //Find the state of the relation with the matched GameObject
    //            for (int j = 0; j < currentRelation.Length; j++)
    //            {
    //                if (currentRelation[j].Progress < 100 || j == currentRelation.Length - 1)
    //                {
    //                    playerProgress = j;
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    if (charMemory.Count == 0 || playerIndex == -1)
    //    {
    //        charMemory.AddLast(new characterMemory(uniqueId));
    //        playerIndex = charMemory.Count - 1;
    //    }
    //    return new interactionFeedback(Remembered, playerIndex, playerProgress);
    //}
    #endregion
    #region Private Utilities
    //Load Material (based on Load Resource)
    private Material LoadMaterialFromName(modelDataMaterial _Material)
    {
        string modelMatePath = "Materials/";
        switch (_Material)
        {
            case modelDataMaterial.EyesSamuel:
                modelMatePath += "Eyes/Eyes, Samuel/Eyes, Samuel";
                break;
            case modelDataMaterial.HairSamuelA:
                modelMatePath += "Hair/Hair, Samuel/Hair, Samuel, A";
                break;
            case modelDataMaterial.HairSamuelB:
                modelMatePath += "Hair/Hair, Samuel/Hair, Samuel, B";
                break;
            case modelDataMaterial.HairSamuelC:
                modelMatePath += "Hair/Hair, Samuel/Hair, Samuel, C";
                break;
            case modelDataMaterial.HeadSamuel:
                modelMatePath += "Head/Head, Samuel/Head, Samuel";
                break;
            case modelDataMaterial.BodySamuel:
                modelMatePath += "Skin/Skin (Samuel)";
                break;
            case modelDataMaterial.CashmereWhite:
                modelMatePath += "Cashmere/Cashmere (White)";
                break;
            case modelDataMaterial.CashmereBlack:
                modelMatePath += "Cashmere/Cashmere (Black)";
                break;
            case modelDataMaterial.CashmereLightBlue:
                modelMatePath += "Cashmere/Cashmere (Light Blue)";
                break;
            case modelDataMaterial.CashmereDarkBlue:
                modelMatePath += "Cashmere/Cashmere (Dark Blue)";
                break;
            case modelDataMaterial.CashmereImage1:
                modelMatePath += "Cashmere/Cashmere (Image 1)";
                break;
            case modelDataMaterial.DenimBlack:
                modelMatePath += "Denim/Denim (Black)";
                break;
            case modelDataMaterial.DenimLightBlue:
                modelMatePath += "Denim/Denim (Light Blue)";
                break;
            case modelDataMaterial.DenimDarkBlue:
                modelMatePath += "Denim/Denim (Dark Blue)";
                break;
            case modelDataMaterial.MedievalMetal:
                modelMatePath += "Medieval Metal/Medieval Metal (Resources)";
                break;
            case modelDataMaterial.ItalianLeather:
                modelMatePath += "Italian Leather/Italian Leather";
                break;
            case modelDataMaterial.ChainMail:
                modelMatePath += "Chain Mail/Chain Mail";
                break;
            default:
                modelMatePath += "None";
                break;
        }
        return (Material)LoadFromFile(modelMatePath);
    }
    //Load Resource
    public  UnityEngine.Object LoadFromFile(string filename)
    {
        var loadedObject = Resources.Load(filename);
        if (loadedObject == null)
        {
            Debug.Log(filename);
            errorManager.Instance.createErrorReport("utilMono", "LoadFromFile", errorType.fileNotFound);
        }
        return loadedObject;
    }
    #endregion
}
