using UnityEngine;
[SerializeField]
public class warpData
{
    //Level Build Index
    private int buildindex;
    public int buildIndex { get { return buildindex; } }

    //Level Name
    private string levelname;
    public string levelName { get { return levelname; } }

    //Show Level Name
    private bool showlevelname;
    public bool showLevelName { get { return showlevelname; } }

    //Audio
    //The Battle System needs to override the default Battle Warp's Music
    private soundLibEntry audiomusic;
    public soundLibEntry audioMusic { get { return audiomusic; } set { audiomusic = value; } }

    private soundLibEntry audioindustrial;
    public soundLibEntry audioIndustral { get { return audioindustrial; } }

    private soundLibEntry audionature;
    public soundLibEntry audioNature { get { return audionature; } }

    private soundLibEntry audioweather;
    public soundLibEntry audioWeather { get { return audioweather; } }

    //Player Coordinates
    private Vector3 playercoordinates;
    public Vector3 playerCoordinates { get { return playercoordinates; } }
    //Player Rotation
    private float playerrotation;
    public float playerRotation { get { return playerrotation; } }
    //Player Perspective
    private playerPerspective playerperspective;
    public playerPerspective playerPerspec { get { return playerperspective; } }

    //Time & Weather
    private Weather levelweather;
    public Weather levelWeather { get { return levelweather; } }

    private bool istimerunning;
    public bool isTimeRunning { get { return istimerunning; } }

    //Constructor
    public warpData(int _buildIndex, string _levelName, bool _showLevelName, soundLibEntry _levelMusic, soundLibEntry _levelAmbientIndustrialAudio,
        soundLibEntry _levelAmbientNatureAudio, soundLibEntry _levelWeatherAudio, Vector3 _playerCoordinates, float _playerRotation, playerPerspective _playerPerspective, Weather _levelWeather,
        bool _isTimeRunning)
    {
        buildindex = _buildIndex;
        levelname = _levelName;
        showlevelname = _showLevelName;
        audiomusic = _levelMusic;
        audioindustrial = _levelAmbientIndustrialAudio;
        audionature = _levelAmbientNatureAudio;
        audioweather = _levelWeatherAudio;
        playercoordinates = _playerCoordinates;
        playerrotation = _playerRotation;
        playerperspective = _playerPerspective;
        levelweather = _levelWeather;
        istimerunning = _isTimeRunning;
    }
}
