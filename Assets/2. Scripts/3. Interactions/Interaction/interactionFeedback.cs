public class interactionFeedback
{
    private bool Remembered;
    private int memoryIndex, relationIndex;
    public interactionFeedback(bool _Remembered, int _memoryIndex, int _relationIndex)
    {
        Remembered = _Remembered;
        memoryIndex = _memoryIndex;
        relationIndex = _relationIndex;
    }
    public bool getRemembered()
    {
        return Remembered;
    }
    public int getMemoryIndex()
    {
        return memoryIndex;
    }
    public int getRelationIndex()
    {
        return relationIndex;
    }
}
