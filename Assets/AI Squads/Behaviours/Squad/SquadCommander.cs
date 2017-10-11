using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadCommander : MonoBehaviour
{
    [SerializeField] Squad owned_squad;
    [SerializeField] float pointer_length = 100;
    [SerializeField] float pointer_radius = 5.0f;
    [SerializeField] Transform indicator;
    [SerializeField] AudioSource command_sound_source;
    [SerializeField] AudioClip command_sound;
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
        }
    }


    private void UpdateIndicator()
    {
        indicator.gameObject.SetActive(true);
        indicator.position = hit_location;
        projector_scaler.SetOrthographicSize(pointer_radius);
    }


    public void UpdateHitLocation(Vector3 _current_look)
    {
        var dist = (transform.position - _current_look).sqrMagnitude;
        if (dist > pointer_length * pointer_length)
            return;

        hit_location = _current_look;
        UpdateIndicator();
    }


    public void SetWaypoint()
    {
        if (!issuing_command)
            return;    

        waypoint_set.Invoke(hit_location, pointer_radius);

        if (command_sound_source == null)
            return;

        if (command_sound_source.isPlaying)
            return;

        if (command_sound == null)
            return;

        command_sound_source.PlayOneShot(command_sound);
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
