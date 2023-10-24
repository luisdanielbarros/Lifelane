public class Dialogue
{
    //Text
    private string text;
    public string Text { get { return text; } }
    //Rich Text
    private string richtext;
    public string richText { get { return richtext; } }
    //Type
    private dialogueType type;
    public dialogueType Type { get { return type; } }
    //Constructor
    public Dialogue(string _Text, dialogueType _Type)
    {
        text = _Text;
        genRichText();
        type = _Type;
    }
    private string genRichText()
    {
        switch (type)
        {
            case dialogueType.Normal:
            default:
                richtext = text;
                char[] Letters = text.ToCharArray();
                if (Letters.Length == 0) return "";
                int fromIndex = 0;
                int toIndex = 1;
                bool waspreviousUppercase = char.IsUpper(Letters[0]);
                int Increments = 0;
                for (int i = 1; i < Letters.Length; i++)
                {
                    bool isUppercase = char.IsUpper(Letters[i]);
                    if (waspreviousUppercase == isUppercase && i != Letters.Length) toIndex = i;
                    else
                    {
                        if (i == Letters.Length) toIndex = i;
                        int fontSize = 64;
                        if (!waspreviousUppercase) fontSize = 48;
                        string fontSizeOpen = "<size=\"" + fontSize + "\">";
                        string fontSizeClose = "</size>";
                        richtext = richtext.Insert(fromIndex + Increments, fontSizeOpen);
                        Increments += fontSizeOpen.Length;
                        richtext = richtext.Insert(toIndex + Increments, fontSizeClose);
                        Increments += fontSizeClose.Length;
                        fromIndex = i;
                        toIndex = i + 1;
                    }
                    waspreviousUppercase = isUppercase;
                }
                return text;
        }
    }
}
