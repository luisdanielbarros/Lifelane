using TMPro;
using UnityEngine;
public class LoadingScreen : MonoBehaviour
{
    //UI
    [SerializeField]
    private TextMeshProUGUI loadingQuote;
    [SerializeField]
    private TextMeshProUGUI loadingText;
    [SerializeField]
    private GameObject continueButton;
    //Story Flags
    private storyFlags Flag = storyFlags.Always;
    private int Subflag;
    //Animation-related Necessities
    private string loadingQuoteString = "", loadingTextString = "";
    void Start()
    {
        Unready();
        //Make the Loading Quote & Text invisible until the first animation, from then forward make them always visible
        loadingQuote.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        loadingText.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
    }
    //Play Animations
    public void playAnimations()
    {
        //Quote
        loadingQuote.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        typeWriterEffect animatedQuote = loadingQuote.GetComponent<typeWriterEffect>();
        animatedQuote.totalVisibleCharacters = loadingQuoteString.Length;
        animatedQuote.Play();
        //Text
        loadingText.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        typeWriterEffect animatedText = loadingQuote.GetComponent<typeWriterEffect>();
        animatedText.totalVisibleCharacters = loadingTextString.Length;
        animatedText.Play();
    }
    //Update Script
    public void updateScript(storyFlags _Flag, int _Subflag)
    {
        //Story Flags
        if (Flag == _Flag) return;
        Flag = _Flag;
        Subflag = _Subflag;
        ////Story Mode
        if (gameState.Instance.currentMode == gameModes.StoryMode)
        {
            if (Flag == storyFlags.Introduction) loadingQuoteString = "A strange dream";
            else if (Flag == storyFlags.Drifting) loadingQuoteString = "And so it loops in my head";
        }
        ////Exploration Mode Mode
        else
        {
            loadingQuoteString = "You can skip cutscenes with SPACE.";
        }
        loadingQuote.SetText(loadingQuoteString);
    }
    //Unready
    public void Unready()
    {
        loadingTextString = "Loading...";
        loadingText.SetText(loadingTextString);
        continueButton.SetActive(false);
        loadingQuote.GetComponent<typeWriterEffect>().totalVisibleCharacters = 0;
        loadingText.GetComponent<typeWriterEffect>().totalVisibleCharacters = 0;
    }
    //Ready
    public void Ready()
    {
        loadingTextString = "Finished loading!";
        loadingText.SetText(loadingTextString);
        continueButton.SetActive(true);
    }
    //Enter Level
    public void enterLevel()
    {
        loadingManager.Instance.enterLevel();
    }
}
