using System;
[Serializable]
public class jsonEventData
{
    //0. Story Flag
    public int storyflag;
    public int storyflagindex;
    public bool incstoryflag;
    public bool incstoryflagindex;

    //1. Music
    public string music;

    //2. Player Coordinates
    public bool teleportsplayer;
    public float playercoordinatesx;
    public float playercoordinatesy;
    public float playercoordinatesz;
    //2. Player Rotation
    public float playerrotation;
    //2. Player Movement
    public string playermovement;

    //4. Interactions
    public string[] interactioncutscene;
    public string[] interactionregular;
    public string[] interactionpickupitem;

    //5. Collectible Item
    public string collectibleitemid;

    //5. Map Warp
    public string warpdata;

    //5. Battles Data
    public bool battlesdata;

    //6. Game Modes
    public bool storymode;
    public bool explorationmode;

    //7. Repeat
    public bool repeat;
}
