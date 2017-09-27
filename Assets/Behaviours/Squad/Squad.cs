using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [SerializeField] private SquadCommander squad_commander;
    [SerializeField] private List<AIController> squad_members;


    public void SetSquadAreaWaypoint(Vector3 _waypoint_pos, float _radius)
    {
        Debug.Log("waypoint set");


        List<Vector3> way_points = TacticalAssessor.FindOptimalCoverInArea(_waypoint_pos, _radius);

        if (way_points == null)
            return;

        
        for (int i = 0; i < squad_members.Count; ++i)
        {
            if (i < way_points.Count)
            {
                squad_members[i].waypoint = way_points[i];
            }
        }
    }

    void DistributeWaypointsToSquad()
    {
        
    }
}
