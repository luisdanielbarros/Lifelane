using UnityEngine;
using UnityEngine.AI;

public class waypointNavigatorMesh : MonoBehaviour
{
    private NavMeshAgent Agent;
    private waypointNavigator Nav;
    void Start()
    {
        Agent = transform.parent.gameObject.GetComponent<NavMeshAgent>();
        Nav = transform.parent.gameObject.GetComponent<waypointNavigator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car" && Nav.currentWaypoint.isWaiting == true) Agent.isStopped = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Car")
        {
            Agent.isStopped = false;
        }
    }
}
