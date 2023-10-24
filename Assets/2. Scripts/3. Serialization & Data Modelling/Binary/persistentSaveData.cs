using System;
using UnityEngine;
[Serializable]
public class persistentSaveData
{
    //Story Flag
    private storyFlags storyflag = storyFlags.Introduction;
    public storyFlags storyFlag { get { return storyflag; } set { storyflag = value; } }
    //Story Flag Index
    private int storyflagindex = 0;
    public int storyFlagIndex { get { return storyflagindex; } set { storyflagindex = value; } }
    //Level Index
    private int levelindex = 0;
    public int levelIndex { get { return levelindex; } }

    //Map Coordinates X
    private float mapcoordsx = 0;
    public float mapCoordsX { get { return mapcoordsx; } }
    //Map Coordinates Y
    private float mapcoordsy = 0;
    public float mapCoordsY { get { return mapcoordsy; } }
    //Map Coordinates Z
    private float mapcoordsz = 3;
    public float mapCoordsZ { get { return mapcoordsz; } }
    //Player Rotation
    private float playerrotation = 0;
    public float playerRotation { get { return playerrotation; } }


    //Player Name
    public string playerdatafile = "sam";
    public string playerDataFile { get { return playerdatafile; } }

    //Partner Name
    public string partnerdatafile = null;
    public string partnerDataFile { get { return partnerdatafile; } }

    //Character Main Experience
    private int charmainexperience = 0;
    public int charMainExperience { get { return charmainexperience; } }

    //Character Partner Marie Experience
    private int charmarieexperience = 250;
    public int charMarieExperience { get { return charmarieexperience; } }

    //Journal
    private storedItemData[] journal;
    public storedItemData[] Journal { get { return journal; } }

    public persistentSaveData(storyFlags _storyFlag, int _storyFlagIndex, int _levelIndex, int _mapCoordsX, int _mapCoordsY, int _mapCoordsZ, float _playerRotation, int _charMainExperience)
    {
        storyflag = _storyFlag;
        storyflagindex = _storyFlagIndex;
        levelindex = _levelIndex;
        mapcoordsx = _mapCoordsX;
        mapcoordsy = _mapCoordsY;
        mapcoordsz = _mapCoordsY;
        playerrotation = _playerRotation;
        charmainexperience = _charMainExperience;
        journal = new storedItemData[] { };
    }
    public void Save(int _levelIndex, Transform playerTransform, storedItemData[] _Journal)
    {
        //Player Level
        levelindex = _levelIndex;
        //Player Coordinates
        Vector3 playerPosition;
        if (playerTransform != null) playerPosition = playerTransform.position;
        else playerPosition = new Vector3(0, 0, 0);
        mapcoordsx = playerPosition[0];
        mapcoordsy = playerPosition[1];
        mapcoordsz = playerPosition[2];
        //Player Rotation
        if (playerTransform != null) playerrotation = playerTransform.rotation.y * 180;
        else playerrotation = 0;
        //Journal
        journal = _Journal;
    }
}
