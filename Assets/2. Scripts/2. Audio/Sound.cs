using UnityEngine;

[System.Serializable]
public class Sound
{
    //Clip
    private AudioClip clip;
    public AudioClip Clip { get { return clip; } set { clip = value; } }
    //Entry
    private soundLibEntry entry;
    public soundLibEntry Entry { get { return entry; } set { entry = value; } }
    //Track
    private audioTrack track;
    public audioTrack Track { get { return track; } set { track = value; } }
    //Volume
    [Range(0f, 1f)]
    private float volume;
    public float Volume { get { return volume; } set { volume = value; } }
    //Pitch
    [Range(0.1f, 2f)]
    private float pitch;
    public float Pitch { get { return pitch; } set { pitch = value; } }
    //Loop
    private bool loop;
    public bool Loop { get { return loop; } set { loop = value; } }
    //Source
    [HideInInspector]
    private AudioSource source;
    public AudioSource Source { get { return source; } set { source = value; } }
    public Sound(soundLibEntry _Entry, audioTrack _Track, float _Volume, float _Pitch, bool _Loop, AudioClip _Clip)
    {
        entry = _Entry;
        track = _Track;
        volume = _Volume;
        pitch = _Pitch;
        loop = _Loop;
        clip = _Clip;
    }
}
