using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadCommander : MonoBehaviour
{
    [SerializeField] Squad owned_squad;
    [SerializeField] float pointer_length = 100;
    [SerializeField] float pointer_radius = 5.0f;
    [SerializeField] Transform indicator;
    [SerializeField] ProjectorScaler projector_scaler;
    [SerializeField] CustomEvents.AreaWaypointEvent waypoint_set;

    private Vector3 hit_location = Vector3.zero;

    public bool issuing_command { get; set; }


    void Start()
    {
        indicator.gameObject.SetActive(false);
    }


    void LateUpdate()
    {
        if (!issuing_command)
        {
            indicator.gameObject.SetActive(false);
            return;
        }

        UpdateHitLocation();
        UpdateIndicator();
    }


    private void UpdateIndicator()
    {
        indicator.gameObject.SetActive(true);
        indicator.position = hit_location;
        projector_scaler.SetOrthographicSize(pointer_radius);
    }


    private void UpdateHitLocation()
    {
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height) * 0.5f);//cast from middle of screen
        RaycastHit hit;
        Physics.Raycast(ray, out hit, pointer_length);
        hit_location = hit.point;//store hit location
    }


    public void SetWaypoint()
    {
        if (!issuing_command)
            return;
    
        waypoint_set.Invoke(hit_location, pointer_radius);
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        
        Gizmos.DrawLine(transform.position, transform.forward * pointer_length);
        UnityEditor.Handles.DrawWireDisc(transform.forward * pointer_length, Vector3.up, pointer_radius);
    }
}
