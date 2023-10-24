using UnityEngine;
using UnityEngine.AI;
public class waypointNavigator : MonoBehaviour
{
    NavMeshAgent Agent;
    public Waypoint currentWaypoint;
    //Public Inspector Settings
    public bool speedVariation = false;
    public bool Bidirectional = false;
    //Public Script Settings
    [System.NonSerialized]
    public int Direction = 0;
    //Traffic System Reference
    [System.NonSerialized]
    public int trafficPathIndex;
    public delegate void recycleTrafficObject(int trafficPathIndex, GameObject obj);
    [System.NonSerialized]
    public recycleTrafficObject selfDestroy;
    //Private state
    private bool willBranch = false;
    //Audio
    public Sound[] Sounds;
    private AudioSource Idle, Startup;
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        if (speedVariation)
        {
            Agent.speed = Random.Range(3.5f, 7f);
        }
        Agent.SetDestination(currentWaypoint.transform.position);
        //Audio
        //Create the Audio Sources in the Scene
        foreach (Sound sound in Sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
            sound.Source.loop = sound.Loop;
            //if (sound.Type == soundType.waypointIdle) Idle = sound.Source;
            //else if (sound.Type == soundType.waypointStartup) Startup = sound.Source;
        }
        if (Idle != null) Idle.Play();
    }
    void Update()
    {
        //if the Current Waypoint isn't Waiting
        if (currentWaypoint.isWaiting == true) return;
        //If the Navigator has reached its destination Waypoint
        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                {
                    //If it should Branch
                    bool shouldBranch = false;
                    if (currentWaypoint.Branches != null && currentWaypoint.Branches.Count > 0) shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;
                    //In case the Branch is on Wait, remember to Branch next iteration
                    if (!willBranch) willBranch = shouldBranch;
                    if (shouldBranch || willBranch)
                    {
                        //If it's Waiting for Branch, Wait at a Stopping Distance of 0
                        if (currentWaypoint.isBranchWaiting)
                        {
                            Agent.radius = 0.01f;
                            //Agent.isStopped = true;
                            //Agent.stoppingDistance = 0.11f;
                            //Agent.SetDestination(currentWaypoint.transform.position);
                            return;
                        }
                        //After it's finished Waiting for Branch, resume the normal Stopping Distance
                        else
                        {
                            Agent.radius = 0.5f;
                            //Agent.isStopped = false;
                            //Agent.stoppingDistance = 3.7f;
                        }
                        willBranch = false;
                        currentWaypoint = currentWaypoint.Branches[Random.Range(0, currentWaypoint.Branches.Count - 1)];
                        Agent.SetDestination(currentWaypoint.transform.position);
                        if (Startup != null) Startup.Play();
                    }
                    else
                    {
                        //If it shouldn't Branch, if it's Direction is 0
                        if (Direction == 0)
                        {
                            //If its Next Waypoint isn't Null, move
                            if (currentWaypoint.nextWaypoint != null && currentWaypoint.isWaiting == false)
                            {
                                currentWaypoint = currentWaypoint.nextWaypoint;
                                Agent.SetDestination(currentWaypoint.transform.position);
                                if (Startup != null) Startup.Play();
                            }
                            else if (currentWaypoint.nextWaypoint == null)
                            {
                                selfDestroy(trafficPathIndex, this.gameObject);
                            }
                        }
                        //Else if it's Direction is 1
                        else
                        {
                            //If its Previous Waypoint isn't Null, move
                            if (currentWaypoint.previousWaypoint != null && currentWaypoint.isWaiting == false)
                            {
                                currentWaypoint = currentWaypoint.previousWaypoint;
                                Agent.SetDestination(currentWaypoint.transform.position);
                                if (Startup != null) Startup.Play();
                            }
                            else if (currentWaypoint.previousWaypoint == null)
                            {
                                selfDestroy(trafficPathIndex, this.gameObject);
                            }
                        }
                    }

                }
            }
        }
    }
}
