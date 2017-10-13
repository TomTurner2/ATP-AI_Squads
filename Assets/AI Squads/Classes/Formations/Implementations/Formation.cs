using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class Formation : ScriptableObject
{
    public Sprite formation_image;
    public abstract List<Transform> GetFormation(int _member_count);

    protected void CreateWaypointObjects(ref List<Transform> _waypoints, List <Vector3> _positions)
    {
        foreach (Vector3 position in _positions)
        {
            _waypoints.Add(new GameObject().transform);
            _waypoints.Last().name = "Squad Waypoint";
            _waypoints.Last().transform.position = position;
        }

        SetFormationLead(ref _waypoints, _waypoints[0]);
    }

    protected void SetFormationLead(ref List<Transform> _waypoints, Transform _lead)
    {
        foreach (Transform waypoint in _waypoints)
        {
            if (waypoint != _lead)
                waypoint.parent = _lead;//link points to lead point
        }
    }
}
