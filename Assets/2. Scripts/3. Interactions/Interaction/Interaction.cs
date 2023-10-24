using UnityEngine;

[System.Serializable]
public class Interaction
{
    //Text
    protected Dialogue dialogue;
    public Dialogue Dialogue { get { return dialogue; } }
    //Image
    protected interactionImgLibEntry[] images = new interactionImgLibEntry[3];
    public interactionImgLibEntry[] Images { get { return images; } }
    //Type
    protected interactionType type;
    public interactionType Type { get { return type; } }
    //Music
    protected soundLibEntry[] music;
    public soundLibEntry[] Music { get { return music; } }
    //Reward
    protected int[] reward;
    public int[] Reward { get { return reward; } }
    //Constructor
    public Interaction(Dialogue _Dialogue)
    {
        dialogue = _Dialogue;
        type = interactionType.Regular;
        reward = new int[] { 0 };
    }
    public Interaction(Dialogue _Dialogue, interactionImgLibEntry[] _Images, soundLibEntry[] _Music)
    {
        dialogue = _Dialogue;
        type = interactionType.Regular;
        if (_Images.Length <= images.Length)
        {
            for (int i = 0; i < _Images.Length; i++)
            {
                images[i] = _Images[i];
            }
        }
        if (_Music != null)
        {
            music = new soundLibEntry[_Music.Length];
            for (int i = 0; i < _Music.Length; i++)
            {
                music[i] = _Music[i];
            }
        }
        reward = new int[] { 0 };
    }
    public Interaction(Dialogue _Dialogue, int _Reward)
    {
        dialogue = _Dialogue;
        type = interactionType.Regular;
        reward = new int[] { _Reward };
    }
}
