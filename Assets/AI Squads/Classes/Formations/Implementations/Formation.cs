using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Formation/EmptyFormation")]
public class Formation : ScriptableObject
{
    public Sprite formation_image;


    public virtual List<Transform> GetFormation(int _member_count)
    {
        return null;
    }


    protected void CreateWaypointObjects(ref List<Transform> _waypoints, List <Vector3> _positions)
    {
        foreach (Vector3 position in _positions)
        {
            _waypoints.Add(new GameObject().transform);
            _waypoints.Last().name = "Squad Waypoint";
            _waypoints.Last().transform.position = position;
        }
    }


    protected void SetFormationLead(ref List<Transform> _waypoints, Transform _lead)
    {
        _waypoints.Swap(0, _waypoints.IndexOf(_lead));//make lead first in list
        foreach (Transform waypoint in _waypoints)
        {
            if (waypoint != _lead)
                waypoint.parent = _lead;//link points to lead point
        }  
    }
}