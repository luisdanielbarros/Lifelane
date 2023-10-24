using System.Collections;
using TMPro;
using UnityEngine;
public class typeWriterEffect : MonoBehaviour
{
    //UI
    private TextMeshProUGUI targetText;
    [SerializeField]
    //Text Speed
    private typeWriterSpeed textSpeed = typeWriterSpeed.Medium;
    private float coroutineInterval;
    //Visible versus Invisible Character Count
    private int totalvisiblecharacters = 256;
    public int totalVisibleCharacters { get { return totalvisiblecharacters; } set { totalvisiblecharacters = value; } }
    //Was Initiated, Has Finished Animating
    private int currentVisibleCharacters;
    private bool wasInitiated = false;
    private bool hasfinishedanimation = false;
    public bool hasFinishedAnimation { get { return hasfinishedanimation; } }
    //Play the animation
    public void Play()
    {
        if (!wasInitiated)
        {
            //UI
            targetText = gameObject.GetComponent<TextMeshProUGUI>();
            //Text Speed
            switch (textSpeed)
            {
                case typeWriterSpeed.Slow:
                    coroutineInterval = 0.075f;
                    break;
                case typeWriterSpeed.Medium:
                    coroutineInterval = 0.05f;
                    break;
                case typeWriterSpeed.Fast:
                    coroutineInterval = 0.025f;
                    break;
                case typeWriterSpeed.SuperFast:
                    coroutineInterval = 0.01f;
                    break;
            }
            //Visible versus Invisible Character Count
            targetText.maxVisibleCharacters = 0;
            //Was Initiated, Has Finished Animating
            wasInitiated = true;
        }
        hasfinishedanimation = false;
        StartCoroutine(playCoroutine());
    }
    //Continue playing the animation as a coroutine
    private IEnumerator playCoroutine()
    {
        currentVisibleCharacters = 0;
        while (true)
        {
            targetText.maxVisibleCharacters = currentVisibleCharacters;
            if (currentVisibleCharacters >= totalVisibleCharacters)
            {
                hasfinishedanimation = true;
                yield return null;
            }
            currentVisibleCharacters += 1;
            yield return new WaitForSecondsRealtime(coroutineInterval);
        }
    }
    //Finish the animation instantly, used by the Interaction Controller if the player has chosen to proceed and if the current animation hasn't finished yet
    public void finishAnimationInstantly()
    {
        currentVisibleCharacters = totalVisibleCharacters;
    }
}
