using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //Instance
    private static AudioManager instance = null;
    public static AudioManager Instance { get { return instance; } }
    #region Track Listing
    //Array of Sounds
    private Sound AUDIO_TRACK_MUSIC,
        AUDIO_TRACK_AMBIENT_INDUSTRIAL,
        AUDIO_TRACK_AMBIENT_NATURE,
        AUDIO_TRACK_WEATHER,

        AUDIO_TRACK_SFX_UI_CLICK,
        AUDIO_TRACK_SFX_UI_HOVER,

        AUDIO_TRACK_SFX_INTERACTION_START,
        AUDIO_TRACK_SFX_INTERACTION_CONTINUE,
        AUDIO_TRACK_SFX_INTERACTION_END,

        AUDIO_TRACK_SFX_PLAYER_WALKING,

        AUDIO_TRACK_SFX_EXTRA_MACHINES,
        AUDIO_TRACK_SFX_EXTRA_HUMANS,
        AUDIO_TRACK_SFX_EXTRA_MONSTERS;
    #endregion
    private Sound[] audioTracks;
    //Previous Scene Index
    private int previousSceneIndex;
    //Audio Mixer
    [SerializeField]
    private AudioMixer audioMixer;
    void Awake()
    {
        //Instatiate
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        //Create and configure the Audio Sources in the Scene
        #region Track Set-up
        ////Music
        AUDIO_TRACK_MUSIC = soundLib.getEntry(soundLibEntry.MUSIC_NONE);
        AUDIO_TRACK_MUSIC.Source = gameObject.AddComponent<AudioSource>();
        AUDIO_TRACK_MUSIC.Source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/Music")[0];
        ////Ambient Industrial
        AUDIO_TRACK_AMBIENT_INDUSTRIAL = soundLib.getEntry(soundLibEntry.INDUSTRIAL_NONE);
        AUDIO_TRACK_AMBIENT_INDUSTRIAL.Source = gameObject.AddComponent<AudioSource>();
        AUDIO_TRACK_AMBIENT_INDUSTRIAL.Source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/Environment/Industrial")[0];
        ////Ambient Nature
        AUDIO_TRACK_AMBIENT_NATURE = soundLib.getEntry(soundLibEntry.NATURE_NONE);
        AUDIO_TRACK_AMBIENT_NATURE.Source = gameObject.AddComponent<AudioSource>();
        AUDIO_TRACK_AMBIENT_NATURE.Source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/Environment/Nature")[0];
        ////Ambient Weather
        AUDIO_TRACK_WEATHER = soundLib.getEntry(soundLibEntry.WEATHER_NONE);
        AUDIO_TRACK_WEATHER.Source = gameObject.AddComponent<AudioSource>();
        AUDIO_TRACK_WEATHER.Source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/Environment/Weather")[0];


        ////SFX UI Click
        AUDIO_TRACK_SFX_UI_CLICK = soundLib.getEntry(soundLibEntry.SFX_UI_CLICK);
        AUDIO_TRACK_SFX_UI_CLICK.Source = gameObject.AddComponent<AudioSource>();
        AUDIO_TRACK_SFX_UI_CLICK.Source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/SFX/SFX_UI")[0];
        ////SFX UI Hover
        AUDIO_TRACK_SFX_UI_HOVER = soundLib.getEntry(soundLibEntry.SFX_UI_HOVER);
        AUDIO_TRACK_SFX_UI_HOVER.Source = gameObject.AddComponent<AudioSource>();
        AUDIO_TRACK_SFX_UI_HOVER.Source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/SFX/SFX_UI")[0];

        ////SFX Interaction Start
        AUDIO_TRACK_SFX_INTERACTION_START = soundLib.getEntry(soundLibEntry.SFX_INTERACTION_START);
        AUDIO_TRACK_SFX_INTERACTION_START.Source = gameObject.AddComponent<AudioSource>();
        AUDIO_TRACK_SFX_INTERACTION_START.Source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/SFX/SFX_UI")[0];
        ////SFX Interaction Continue
        AUDIO_TRACK_SFX_INTERACTION_CONTINUE = soundLib.getEntry(soundLibEntry.SFX_INTERACTION_CONTINUE);
        AUDIO_TRACK_SFX_INTERACTION_CONTINUE.Source = gameObject.AddComponent<AudioSource>();
        AUDIO_TRACK_SFX_INTERACTION_CONTINUE.Source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/SFX/SFX_UI")[0];
        ////SFX Interaction Start
        AUDIO_TRACK_SFX_INTERACTION_END = soundLib.getEntry(soundLibEntry.SFX_INTERACTION_END);
        AUDIO_TRACK_SFX_INTERACTION_END.Source = gameObject.AddComponent<AudioSource>();
        AUDIO_TRACK_SFX_INTERACTION_END.Source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/SFX/SFX_UI")[0];

        //SFX Player Walking
        AUDIO_TRACK_SFX_PLAYER_WALKING = soundLib.getEntry(soundLibEntry.SFX_PLAYER_WALKING_CONCRETE);
        AUDIO_TRACK_SFX_PLAYER_WALKING.Source = gameObject.AddComponent<AudioSource>();
        AUDIO_TRACK_SFX_PLAYER_WALKING.Source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/Environment/SFX")[0];

        //SFX Extra Machines
        AUDIO_TRACK_SFX_EXTRA_MACHINES = soundLib.getEntry(soundLibEntry.SFX_EXTRA_MACHINES_NONE);
        AUDIO_TRACK_SFX_EXTRA_MACHINES.Source = gameObject.AddComponent<AudioSource>();
        AUDIO_TRACK_SFX_EXTRA_MACHINES.Source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/Environment/SFX")[0];

        //SFX Extra Humans
        AUDIO_TRACK_SFX_EXTRA_HUMANS = soundLib.getEntry(soundLibEntry.SFX_EXTRA_HUMANS_NONE);
        AUDIO_TRACK_SFX_EXTRA_HUMANS.Source = gameObject.AddComponent<AudioSource>();
        AUDIO_TRACK_SFX_EXTRA_HUMANS.Source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/Environment/SFX")[0];

        //SFX Extra Monsters
        AUDIO_TRACK_SFX_EXTRA_MONSTERS = soundLib.getEntry(soundLibEntry.SFX_EXTRA_MONSTERS_NONE);
        AUDIO_TRACK_SFX_EXTRA_MONSTERS.Source = gameObject.AddComponent<AudioSource>();
        AUDIO_TRACK_SFX_EXTRA_MONSTERS.Source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master/Environment/SFX")[0];

        audioTracks = new Sound[13] {
            AUDIO_TRACK_MUSIC,
            AUDIO_TRACK_AMBIENT_INDUSTRIAL,
            AUDIO_TRACK_AMBIENT_NATURE,
            AUDIO_TRACK_WEATHER,

            AUDIO_TRACK_SFX_UI_CLICK,
            AUDIO_TRACK_SFX_UI_HOVER,

            AUDIO_TRACK_SFX_INTERACTION_START,
            AUDIO_TRACK_SFX_INTERACTION_CONTINUE,
            AUDIO_TRACK_SFX_INTERACTION_END,

            AUDIO_TRACK_SFX_PLAYER_WALKING,

            AUDIO_TRACK_SFX_EXTRA_MACHINES,
            AUDIO_TRACK_SFX_EXTRA_HUMANS,
            AUDIO_TRACK_SFX_EXTRA_MONSTERS
        };
        for (int i = 0; i < audioTracks.Length; i++)
        {
            audioTracks[i].Source.clip = AUDIO_TRACK_MUSIC.Clip;
            audioTracks[i].Source.volume = AUDIO_TRACK_MUSIC.Volume;
            audioTracks[i].Source.pitch = AUDIO_TRACK_MUSIC.Pitch;
            audioTracks[i].Source.loop = AUDIO_TRACK_MUSIC.Loop;
        }
        //Configure the default Audio Tracks
        AudioManager.Instance.changeTrack(soundLibEntry.MUSIC_MAIN_MENU, true);

        AudioManager.Instance.changeTrack(soundLibEntry.SFX_UI_CLICK);
        AudioManager.Instance.changeTrack(soundLibEntry.SFX_UI_HOVER);

        AudioManager.Instance.changeTrack(soundLibEntry.SFX_INTERACTION_START);
        AudioManager.Instance.changeTrack(soundLibEntry.SFX_INTERACTION_CONTINUE);
        AudioManager.Instance.changeTrack(soundLibEntry.SFX_INTERACTION_END);
        #endregion
    }
    #region UnityEvents Functions
    public void UIClick()
    {
        Play(audioTrack.SFX_UI_CLICK, true);
    }
    public void UIHover()
    {
        Play(audioTrack.SFX_UI_HOVER, true);
    }
    #endregion
    #region Unity Scripts Functions
    //Play
    public void Play(audioTrack Track, bool playOnce = false)
    {
        for (int i = 0; i < audioTracks.Length; i++)
        {
            if (audioTracks[i].Track == Track)
            {
                if (playOnce == true) audioTracks[i].Source.Play();
                else audioTracks[i].Source.Play();
            }
        }
    }
    //Stop
    public void Stop(audioTrack Track)
    {
        for (int i = 0; i < audioTracks.Length; i++)
        {
            if (audioTracks[i].Track == Track) audioTracks[i].Source.Stop();
        }
    }
    public void changeTrack(soundLibEntry Entry, bool playAutomatically = false)
    {
        if (Entry == soundLibEntry.NO_CHANGE) return;
        Sound newSound = soundLib.getEntry(Entry);
        for (int i = 0; i < audioTracks.Length; i++)
        {
            if (audioTracks[i].Track == newSound.Track)
            {
                audioTracks[i].Source.Stop();
                audioTracks[i].Source.clip = newSound.Clip;
                audioTracks[i].Track = newSound.Track;
                audioTracks[i].Volume = newSound.Volume;
                audioTracks[i].Source.volume = newSound.Volume;
                audioTracks[i].Pitch = newSound.Pitch;
                audioTracks[i].Source.pitch = newSound.Pitch;
                audioTracks[i].Loop = newSound.Loop;
                audioTracks[i].Source.loop = newSound.Loop;
                if (playAutomatically) audioTracks[i].Source.Play();
            }
        }
    }
    public soundLibEntry getCurrentMusic()
    {
        for (int i = 0; i < audioTracks.Length; i++)
        {
            if (audioTracks[i].Track == audioTrack.MUSIC)
            {
                return audioTracks[i].Entry;
            }
        }
        return soundLibEntry.MUSIC_NONE;
    }
    #endregion
    #region Player
    //Player Walking
    public void playPlayerWalking()
    {
        for (int i = 0; i < audioTracks.Length; i++)
        {
            if (audioTracks[i].Track == audioTrack.SFX_PLAYER_WALKING) audioTracks[i].Source.Play();
        }
    }
    public void stopPlayerWalking()
    {
        for (int i = 0; i < audioTracks.Length; i++)
        {
            if (audioTracks[i].Track == audioTrack.SFX_PLAYER_WALKING) audioTracks[i].Source.Stop();
        }
    }
    #endregion
}
