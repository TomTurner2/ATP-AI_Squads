using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Formation/WedgeFormation")]
public class WedgeFormation : Formation
{
    [SerializeField] float spacing_multiplier = 3;
    [SerializeField] float character_radius = 0.5f;

    public override List<Transform> GetFormation(int _member_count)
    {
        if (_member_count <= 0)
            return null;

        List<Vector3> positions = new List<Vector3>();

        int x = 0, y = 0, count = 0;

        for (y = 0; count < _member_count; ++y)
        {
            for (x = 0; x < y + 1; ++x)
            {
                ++count;
                float x_offset = (x - y * 0.5f);
                positions.Add(new Vector3(x_offset * character_radius*spacing_multiplier, 0, y * character_radius * spacing_multiplier));
            }
        }

        List<Transform> waypoints = new List<Transform>();
        CreateWaypointObjects(ref waypoints, positions);
        SetFormationLead(ref waypoints, waypoints[0]);

        return waypoints;
    }
}
