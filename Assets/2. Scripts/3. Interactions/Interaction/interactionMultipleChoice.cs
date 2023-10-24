using UnityEngine;
public class interactionMultipleChoice : Interaction
{
    [SerializeField]
    protected string[] options;
    public string[] Options { get { return options; } }
    public int selectedOption { get; set; }
    public interactionMultipleChoice(Dialogue _Dialogue, string[] _Options) : base(_Dialogue)
    {
        options = _Options;
        type = interactionType.MultipleChoice;
        reward = new int[_Options.Length];
        for (int i = 0; i < Reward.Length; i++) Reward[i] = 0;
    }
    public interactionMultipleChoice(Dialogue _Dialogue, string[] _Options, int[] _Reward) : base(_Dialogue)
    {
        options = _Options;
        type = interactionType.MultipleChoice;
        reward = _Reward;
    }
}
