using UnityEngine;
public class spriteController : MonoBehaviour
{
    private GameObject playerObj;
    private SpriteRenderer spriteRndrr;
    [SerializeField]
    private Sprite frontSprite, frontDiagSprite, sideSprite, backDiagSprite, backSprite;
    private int previousAngle;
    private bool previousFlip = false;
    void Start()
    {
        spriteRndrr = GetComponent<SpriteRenderer>();
        playerObj = utilMono.Instance.getPlayerObject();
    }
    void Update()
    {
        //Always face camera
        Vector3 camPosition = Camera.main.transform.position;
        transform.LookAt(camPosition, Vector3.up);

        //Calculate the angle between the supposed front of the sprite and the camera
        Vector3 targetDir = camPosition - transform.position;
        Vector3 Cross = Vector3.Cross(targetDir, playerObj.transform.right);
        Vector3 crossFlip = Vector3.Cross(targetDir, playerObj.transform.forward);
        float Angle = Cross.y;
        bool flipSprite = (crossFlip.y > 0);
        ////Instead of going from -8 to 8, goes from 0 to 16
        Angle += 8f;

        //Use the calculated angle to switch sprites (from the front sprite to its side sprite, back, etc.)
        ////Flip Sprite
        if (previousFlip != flipSprite) spriteRndrr.flipX = flipSprite;
        ////Switch Sprite 
        float maxAngle = 16f;
        float angleSplits = maxAngle / 5;
        if (Angle <= angleSplits)
        {
            if (previousAngle != 0)
            {
                previousAngle = 0;
                spriteRndrr.sprite = backSprite;
            }
        }
        else if (Angle <= angleSplits * 2)
        {
            if (previousAngle != 1)
            {
                previousAngle = 1;
                spriteRndrr.sprite = backDiagSprite;
            }
        }
        else if (Angle <= angleSplits * 3)
        {
            if (previousAngle != 2)
            {
                previousAngle = 2;
                spriteRndrr.sprite = sideSprite;
            }
        }
        else if (Angle <= angleSplits * 4)
        {
            if (previousAngle != 3)
            {
                previousAngle = 3;
                spriteRndrr.sprite = frontDiagSprite;
            }
        }
        else if (Angle <= angleSplits * 5)
        {
            if (previousAngle != 4)
            {
                previousAngle = 4;
                spriteRndrr.sprite = frontSprite;
            }
        }
        previousFlip = flipSprite;
    }
}
