using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [SerializeField] private SquadCommander squad_commander;
    [SerializeField] private List<AIController> squad_members;


    public void SetSquadAreaWaypoint(Vector3 _waypoint_pos, float _radius)
    {
        List<Vector3> way_points = GameManager.scene_refs.tactical_assessor.FindOptimalCoverInArea(_waypoint_pos, _radius);
        DistributeWaypointsToSquad(way_points);  
    }


    void DistributeWaypointsToSquad(List<Vector3> _way_points)
    {
        if (_way_points == null)
            return;

        for (int i = 0; i < squad_members.Count; ++i)
        {
            if (i < _way_points.Count)
            {
                squad_members[i].waypoint = _way_points[i];
            }
            else
            {
                return;
            }
        }
    }
}
