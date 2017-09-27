using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadCommander : MonoBehaviour
{
    [SerializeField] private Squad owned_squad;
    [SerializeField] private float pointer_length = 100;
    [SerializeField] private float pointer_radius = 5.0f;
    [SerializeField] private CustomEvents.AreaWaypointEvent waypoint_set;

    void Start()
    {
        //waypoint_set = new CustomEvents.AreaWaypointEvent();
    }


    public void SetWaypoint()
    {
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height ) * 0.5f);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, pointer_length);
        
        waypoint_set.Invoke(hit.point, pointer_radius);
        Debug.Log("invoked");
    }


    public void FollowLeader()
    {
        
    }


    public void ToggleFormation()
    {
        
    }


    public void ToggleCover()
    {
        
    }
}
