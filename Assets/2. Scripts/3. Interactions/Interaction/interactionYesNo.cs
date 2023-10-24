public class interactionYesNo : Interaction
{
    public int selectedOption { get; set; }

    public interactionYesNo(Dialogue _Dialogue) : base(_Dialogue)
    {
        type = interactionType.YesOrNo;
        reward = new int[] { 0, 0 };
    }
    public interactionYesNo(Dialogue _Dialogue, int[] _Reward) : base(_Dialogue)
    {
        type = interactionType.YesOrNo;
        reward = _Reward;
    }
}
