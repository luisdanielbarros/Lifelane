using TMPro;
using UnityEngine;

public class fogOverlay : MonoBehaviour
{
    //Quote
    private string[] Quotes = new string[] {
        "Wrong way",
        "The fog's too thick",
    };
    private string currentQuote;
    //UI
    [SerializeField]
    private TMP_Text UIText;
    public void genQuote()
    {
        currentQuote = Quotes[Random.Range(0, Quotes.Length)];
        UIText.text = currentQuote;
    }
}
