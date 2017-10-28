using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Formation/LineFormation")]
public class LineFormation : Formation
{
    [SerializeField] float spacing_multiplier = 1;
    [SerializeField] float character_radius = 0.5f;


    public override List<Transform> GetFormation(int _member_count)
    {
        if (_member_count <= 0)
            return null;

        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < _member_count; ++i)
        {
            positions.Add(new Vector3((i + character_radius) * spacing_multiplier, 0, 0));
        }

        List<Transform> waypoints = new List<Transform>();
        CreateWaypointObjects(ref waypoints, positions);
        SetFormationLead(ref waypoints, waypoints[(int)(_member_count * 0.5f)]);

        return waypoints;
    }
}
