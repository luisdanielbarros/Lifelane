using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Waypoint : MonoBehaviour
{
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;
    [Range(0f, 5f)]
    public float Width = 1f;
    public List<Waypoint> Branches = new List<Waypoint>();
    [Range(0f, 1f)]
    public float branchRatio = 0.5f;
    public bool isWaiting = false;
    public bool isBranchWaiting = false;
    public Vector3 getPosition()
    {
        Vector3 minBound = transform.position + transform.right * Width / 2f;
        Vector3 maxBound = transform.position - transform.right * Width / 2f;
        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}
