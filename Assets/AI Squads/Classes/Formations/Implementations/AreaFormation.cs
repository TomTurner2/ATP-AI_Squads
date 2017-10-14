using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Formation/AreaFormation")]
public class AreaFormation : Formation
{
    [SerializeField] float radius = 4; 

    public override List<Transform> GetFormation(int _member_count)
    {
        if (_member_count <= 0)
            return null;

        List<Transform> waypoints = new List<Transform>();
        CreateWaypointObjects(ref waypoints, CustomMath.DistributedPointsInRadius(Vector3.zero, radius * _member_count, 1,
            (int)(_member_count * 0.5f), 2f));

        SetFormationLead(ref waypoints, waypoints[0]);

        return waypoints;
    }
}
