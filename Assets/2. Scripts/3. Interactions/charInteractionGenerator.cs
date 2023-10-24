using UnityEngine;

public enum interactionGeneratorSymbols { endOfLine, subjectName, targetName, sharedInterest }
public enum interactionGeneratorSymbolsM { endOfLine, subjectName, targetName, sharedInterest }
public class charInteractionGenerator : MonoBehaviour
{
    //Variables
    #region Variables
    //Random
    private readonly System.Random rnd = new System.Random();
    //Possibilities
    //Greeting
    private readonly string[] Greetings = new string[] {
        "Hello"  + interactionGeneratorSymbolsM.targetName + interactionGeneratorSymbolsM.endOfLine
    };
    //Weather
    private readonly string[] niceWeather = new string[]
    {
        "Nice weather today, isn't it?"
    };
    //How are you?
    private readonly string[] howAreYou = new string[]
    {
        "How are you?"
    };
    //Shared Interest
    private readonly string[] StrsharedInterest = new string[]
    {
        "Do you like " + interactionGeneratorSymbolsM.sharedInterest + "?"
    };
    //Flirting
    ////Big Butt
    private readonly string[] StrbB = new string[]
    {
        "bB"+interactionGeneratorSymbolsM.endOfLine
    };
    ////Big Chest
    private readonly string[] StrbC = new string[]
    {
        "bC"+interactionGeneratorSymbolsM.endOfLine
    };
    //Temporary stores
    //Useful strings
    private string subjectName;
    private string targetName;
    private string sharedInterest;
    //Flags
    private bool pF;
    private bool bB;
    private bool bC;
    #endregion
    //Randomness Functions
    #region Randomness Functions
    private string addRandomness(string myCodedString)
    {
        //End Of Line
        interactionGeneratorSymbolsM currentSymbolM = interactionGeneratorSymbolsM.endOfLine;
        if (myCodedString.Contains(currentSymbolM.ToString())) myCodedString = myCodedString.Replace(currentSymbolM.ToString(), blankChance(fiftyChance("!", ".")));
        //Subject Name
        currentSymbolM = interactionGeneratorSymbolsM.subjectName;
        if (myCodedString.Contains(currentSymbolM.ToString())) myCodedString = myCodedString.Replace(currentSymbolM.ToString(), blankChance(" " + subjectName));
        //Target Name
        currentSymbolM = interactionGeneratorSymbolsM.targetName;
        if (myCodedString.Contains(currentSymbolM.ToString())) myCodedString = myCodedString.Replace(currentSymbolM.ToString(), blankChance(" " + targetName));
        //Shared Interest
        interactionGeneratorSymbols currentSymbolA = interactionGeneratorSymbols.sharedInterest;
        if (myCodedString.Contains(currentSymbolA.ToString())) myCodedString = myCodedString.Replace(currentSymbolA.ToString(), " " + sharedInterest);
        return myCodedString;
    }
    private string fiftyChance(string firstPoss, string secondPoss)
    {
        if (rnd.Next(0, 2) == 0) return firstPoss;
        else return secondPoss;
    }
    private string blankChance(string firstPoss)
    {
        if (rnd.Next(0, 2) == 0) return firstPoss;
        else return "";
    }
    #endregion
    public static charInteractionGenerator Instance { get; set; }
    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    public Interaction[] genInteraction(jsonCharacterData Subject, jsonCharacterData Target)
    {
        return null;
        ////Remember the GameObject
        //interactionFeedback inteFB = utilMono.Instance.searchMemory(Target.uniqueId, Subject.Memory);
        //charRelationFlag rememberedRelationFlag = Subject.Memory.ElementAt(inteFB.getMemoryIndex()).charRelations[inteFB.getRelationIndex()].Flag;

        ////Temporarily store some values for the convenience of not having to pass them through ALL (most, I'm exaggerating) of the
        ////executed functions below
        //subjectName = Subject.name;
        //targetName = Target.name;
        //sharedInterest = "Football";
        //pF = true;
        //bB = true;
        //bC = true;

        ////Obtain information about the Game Object besides the memories to generate new dialogue
        //characterGender targetGender = Target.Gender;
        //characterPhysic targetPhysic = Target.Physic;
        //LinkedList<Interaction> interactionDialogue = new LinkedList<Interaction>();
        //switch (rememberedRelationFlag)
        //{
        //    case charRelationFlag.emotionalTopics:
        //        interactionDialogue.AddLast(new Interaction(addRandomness(StrbB[rnd.Next(0, StrbB.Length)])));
        //        break;
        //    case charRelationFlag.sharedInterests:
        //        interactionDialogue.AddLast(new interactionYesNo(addRandomness(StrsharedInterest[rnd.Next(0, StrsharedInterest.Length)])));
        //        break;
        //    case charRelationFlag.Formalities:
        //        interactionDialogue.AddLast(new interactionYesNo(addRandomness(niceWeather[rnd.Next(0, niceWeather.Length)])));
        //        interactionDialogue.AddLast(new interactionMultipleChoice(addRandomness(howAreYou[rnd.Next(0, howAreYou.Length)]), new string[] { "I'm good.", "Not so good." })); break;
        //    case charRelationFlag.Greeting:
        //        interactionDialogue.AddLast(new Interaction(addRandomness(Greetings[rnd.Next(0, Greetings.Length)]), 5));
        //        interactionDialogue.AddLast(new interactionYesNo(addRandomness(niceWeather[rnd.Next(0, niceWeather.Length)]), new int[] { 5, -5 }));
        //        interactionDialogue.AddLast(new interactionMultipleChoice(addRandomness(howAreYou[rnd.Next(0, howAreYou.Length)]), new string[] { "I'm good.", "Not so good." }, new int[] { 5, -5 }));
        //        break;
        //}
        ////Update the relation after output
        ////Subject.charMemory.ElementAt(inteFB.getMemoryIndex()).charRelations[inteFB.getRelationIndex()].Progress++;
        //return interactionDialogue.ToArray();
    }

}
