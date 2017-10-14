using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Formation/CircleFormation")]
public class CircleFormation : Formation
{
    [SerializeField] float spacing_multiplier = 1;
    [SerializeField] float character_radius = 0.5f;

    public override List<Transform> GetFormation(int _member_count)
    {
        if (_member_count <= 0)
            return null;

        float radius = (_member_count + character_radius) * spacing_multiplier;
        List<Transform> waypoints = new List<Transform>();
        
        CreateWaypointObjects(ref waypoints, CustomMath.PointsOnCircumference(Vector3.zero, radius, _member_count));
        SetFormationLead(ref waypoints, waypoints.Last());
        
        return waypoints;
    }
}
