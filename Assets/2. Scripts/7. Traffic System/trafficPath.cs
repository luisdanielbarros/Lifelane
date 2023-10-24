using UnityEngine;

[System.Serializable]
public class trafficPath
{
    //Waypoints
    public GameObject Waypoints;
    public Waypoint startingWaypoint;
    public Waypoint endingWaypoint;
    //Spawn Ratio
    public int maximumSpawns = -1;
    [System.NonSerialized]
    public int currentSpawns = 0;
    public float secondsPerSpawn = 5f;
    //Priority
    [System.NonSerialized]
    public int currentPriority = 0;
}
