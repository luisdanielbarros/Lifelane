using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class trafficSystem : MonoBehaviour
{
    //Traffic Objects
    public trafficObjectType prefabsType;
    private trafficObject[] trafficObjects;
    private float[] prefabRarity;
    //Traffic Paths
    public trafficPath[] trafficPaths;
    //Traffic Switches
    public trafficSwitch[] trafficSwitchs;
    public trafficSwitch[] trafficBranchSwitches;
    public float switchInterval = 10f, switchTransition = 10f;
    private int trafficIndex = 0;
    void Start()
    {
        //Traffic Objects
        switch (prefabsType)
        {
            case trafficObjectType.NPC:
                trafficObjects = new trafficObject[] {
                new trafficObject("NPCs/NPC/NPC", 1) };
                break;
            default:
                trafficObjects = new trafficObject[] {
                new trafficObject("Vehicles/Car 1A/Car 1A", 1) };
                break;
        }
        prefabRarity = new float[trafficObjects.Length];
        for (int i = 0; i < prefabRarity.Length; i++)
        {
            if (i == 0) prefabRarity[i] = trafficObjects[i].spawnRatio;
            else prefabRarity[i] = trafficObjects[i].spawnRatio + prefabRarity[i - 1];
        }
        //Traffic Switches
        //Traffic Switches -> Normal
        for (int i = 0; i < trafficSwitchs.Length; i++)
        {
            trafficSwitch currentTrafficSwitch = trafficSwitchs[i];
            for (int j = 0; j < currentTrafficSwitch.Waypoints.Length; j++)
            {
                Waypoint currentWaypoint = currentTrafficSwitch.Waypoints[j];
                currentWaypoint.isWaiting = true;
            }
        }
        //Traffic Switches -> Branches
        for (int i = 0; i < trafficBranchSwitches.Length; i++)
        {
            trafficSwitch currentTrafficSwitch = trafficBranchSwitches[i];
            for (int j = 0; j < currentTrafficSwitch.Waypoints.Length; j++)
            {
                Waypoint currentWaypoint = currentTrafficSwitch.Waypoints[j];
                currentWaypoint.isBranchWaiting = true;
            }
        }
        if (trafficSwitchs != null && trafficSwitchs.Length > 0) StartCoroutine("switchTraffic");
        //Coroutines
        for (int i = 0; i < trafficPaths.Length; i++)
        {
            StartCoroutine("Spawn", new object[2] { i, trafficPaths[i].secondsPerSpawn });
        }
    }
    //Traffic Objects

    IEnumerator Spawn(object[] obj)
    {
        while (true)
        {
            int i = (int)obj[0];
            float Seconds = (float)obj[1];
            spawnTrafficObject(i);
            yield return new WaitForSeconds(Seconds);
        }
    }
    public void spawnTrafficObject(int i)
    {
        trafficPath currentTrafficPath = trafficPaths[i];
        //Check Current Spawns
        if (currentTrafficPath.currentSpawns > currentTrafficPath.maximumSpawns) return;
        float prefabSelection = Random.Range(0, prefabRarity[prefabRarity.Length - 1]);
        GameObject Obj;
        //For each Traffic Object's Spawn Ratio Tier
        for (int j = 0; j < prefabRarity.Length; j++)
        {
            if (prefabSelection <= prefabRarity[j])
            {
                //Spawn the Object
                GameObject objToSpawn = (GameObject)utilMono.Instance.LoadFromFile(trafficObjects[j].prefabURL);
                Obj = Instantiate(objToSpawn);
                waypointNavigator objNavigator = Obj.GetComponent<waypointNavigator>();
                NavMeshAgent objAgent = Obj.GetComponent<NavMeshAgent>();
                //Set TrafficPath Index it's associated to
                objNavigator.trafficPathIndex = i;
                //Disable Agent
                objAgent.enabled = false;
                //Set Direction
                if (objNavigator.Bidirectional) objNavigator.Direction = Mathf.RoundToInt(Random.Range(0, 2));
                //Set Agent Priority
                objAgent.avoidancePriority = currentTrafficPath.currentPriority++;
                if (currentTrafficPath.currentPriority > 49) currentTrafficPath.currentPriority = 0;
                //Set Self-Destruction delegate
                objNavigator.selfDestroy = recycleTrafficObject;
                //Update Current Spawns
                currentTrafficPath.currentSpawns++;
                //Set Position, Rotation & Next Waypoint based on Direction
                if (objNavigator.Direction == 0)
                {
                    Obj.transform.position = currentTrafficPath.startingWaypoint.transform.position;
                    Obj.transform.rotation = currentTrafficPath.startingWaypoint.transform.rotation;
                    objNavigator.currentWaypoint = currentTrafficPath.startingWaypoint.nextWaypoint;
                }
                else
                {
                    Obj.transform.position = currentTrafficPath.endingWaypoint.transform.position;
                    Obj.transform.rotation = currentTrafficPath.endingWaypoint.transform.rotation;
                    objNavigator.currentWaypoint = currentTrafficPath.endingWaypoint.previousWaypoint;
                }
                //Enable Agent & Set its Destination (waypointNavigator won't automatically Set Destination if the Waypoint is Waiting)
                objAgent.enabled = true;
                objAgent.SetDestination(objNavigator.currentWaypoint.transform.position);
                break;
            }
        }
    }
    public void recycleTrafficObject(int trafficPathIndex, GameObject objToRecycle)
    {
        trafficPath currentTrafficPath = trafficPaths[trafficPathIndex];
        waypointNavigator objNavigator = objToRecycle.GetComponent<waypointNavigator>();
        NavMeshAgent objAgent = objToRecycle.GetComponent<NavMeshAgent>();
        //Disable Agent
        objAgent.enabled = false;
        //Set Position, Rotation & Next Waypoint based on Direction
        if (objNavigator.Direction == 0)
        {
            objToRecycle.transform.position = currentTrafficPath.startingWaypoint.transform.position;
            objToRecycle.transform.rotation = currentTrafficPath.startingWaypoint.transform.rotation;
            objToRecycle.GetComponent<waypointNavigator>().currentWaypoint = currentTrafficPath.startingWaypoint.nextWaypoint;
        }
        else
        {
            objToRecycle.transform.position = currentTrafficPath.endingWaypoint.transform.position;
            objToRecycle.transform.rotation = currentTrafficPath.endingWaypoint.transform.rotation;
            objNavigator.currentWaypoint = currentTrafficPath.endingWaypoint.previousWaypoint;
        }
        //Enable Agent & Set its Destination (waypointNavigator won't automatically Set Destination if the Waypoint is Waiting)
        objAgent.enabled = true;
        objAgent.SetDestination(objNavigator.currentWaypoint.transform.position);
    }
    //Traffic Switches
    IEnumerator switchTraffic()
    {
        while (true)
        {
            //For each Normal Traffic Switch
            for (int i = 0; i < trafficSwitchs.Length; i++)
            {
                trafficSwitch currentTrafficSwitch = trafficSwitchs[i];
                //For each Traffic Switch's Waypoint
                for (int j = 0; j < currentTrafficSwitch.Waypoints.Length; j++)
                {
                    Waypoint currentWaypoint = currentTrafficSwitch.Waypoints[j];
                    //If the current Traffic Switch is the one being pointed at by the trafficIndex variable, else...
                    if (i == trafficIndex) currentWaypoint.isWaiting = false;
                    else currentWaypoint.isWaiting = true;
                }
            }
            //For each Branch Traffic Switch
            for (int i = 0; i < trafficBranchSwitches.Length; i++)
            {
                trafficSwitch currentTrafficSwitch = trafficBranchSwitches[i];
                //For each Traffic Switch's Waypoint
                for (int j = 0; j < currentTrafficSwitch.Waypoints.Length; j++)
                {
                    Waypoint currentWaypoint = currentTrafficSwitch.Waypoints[j];
                    //If the current Traffic Switch is the one being pointed at by the trafficIndex variable, else...
                    if (i == trafficIndex) currentWaypoint.isBranchWaiting = false;
                    else currentWaypoint.isBranchWaiting = true;
                }
            }
            yield return new WaitForSeconds(switchInterval);
            //Turn off all Switches before turning the the Next Switch on
            for (int i = 0; i < trafficSwitchs.Length; i++) for (int j = 0; j < trafficSwitchs[i].Waypoints.Length; j++) trafficSwitchs[i].Waypoints[j].isWaiting = true;
            yield return new WaitForSeconds(switchTransition);
            for (int i = 0; i < trafficBranchSwitches.Length; i++) for (int j = 0; j < trafficBranchSwitches[i].Waypoints.Length; j++) trafficBranchSwitches[i].Waypoints[j].isBranchWaiting = true;
            yield return new WaitForSeconds(switchTransition);
            trafficIndex++;
            if (trafficIndex > trafficSwitchs.Length) trafficIndex = 0;
        }
    }
}
