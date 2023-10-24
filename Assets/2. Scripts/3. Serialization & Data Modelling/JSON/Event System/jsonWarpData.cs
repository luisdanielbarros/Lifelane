using System;
[Serializable]
public class jsonWarpData
{
    //Level Build Index
    public int buildindex;
    //Level Name
    public string levelname;
    //Show Level Name
    public bool showlevelname;
    //Audio
    public string music;
    public string audioindustrial;
    public string audionature;
    public string audioweather;
    //Player Coordinates
    public int playercoordinatesx;
    public int playercoordinatesy;
    public int playercoordinatesz;
    //Player Rotation
    public int playerrotation;
    //Player Perspective
    public string playerperspective;
    //Time & Weather
    public string weather;
    public bool istimerunning;
}
