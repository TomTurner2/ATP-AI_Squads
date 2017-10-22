using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SquadCommander : MonoBehaviour
{
    [SerializeField] Squad owned_squad;

    [SerializeField] List<Formation> formations;

    [Space][Header("Indicator")]
    public float pointer_length = 100;
    public float pointer_radius = 5.0f;
    [SerializeField] Transform indicator;
    [SerializeField] ProjectorScaler projector_scaler;

    [Space][Header("Audio")]
    [SerializeField] AudioSource command_sound_source;
    [SerializeField] AudioClip command_sound;

    [Space][Header("Events")]
    [SerializeField] CustomEvents.AreaWaypointEvent waypoint_set;
    [SerializeField] private CustomEvents.SpriteSwapEvent formation_toggle_event;
    [SerializeField] private UnityEvent follow_event, un_follow_event;
    [SerializeField] private UnityEvent find_cover_event, no_cover_event;
    [SerializeField] private UnityEvent weapons_free_event, hold_fire_event;

    private Vector3 hit_location = Vector3.zero;
    private int current_formation_index = 0;

    public bool issuing_command { get; set; }


    void Start()
    {
        indicator.gameObject.SetActive(false);
        //kick start formation
        owned_squad.SetFormation(formations[current_formation_index]);
        formation_toggle_event.Invoke(formations[current_formation_index].formation_image);
    }


    void LateUpdate()
    {
        if (!issuing_command)
        {
            indicator.gameObject.SetActive(false);//hide indicator
        }
    }


    private void UpdateIndicator()
    {
        indicator.gameObject.SetActive(true);
        indicator.position = hit_location;
        projector_scaler.SetOrthographicSize(pointer_radius);//change indicator size
    }


    public void UpdateHitLocation(Vector3 _current_look)//current look position from external source
    {
        var dist = (transform.position - _current_look).sqrMagnitude;
        if (dist > pointer_length * pointer_length)//if in range
            return;

        hit_location = _current_look;
        UpdateIndicator();//update the indicators position
    }


    public void SetWaypoint()
    {
        if (!issuing_command)
            return;    

        waypoint_set.Invoke(hit_location, pointer_radius);

        if (owned_squad.follow_commander)//if squad is commander
            ToggleFollowLeader();//stop them following

        if (command_sound_source.isPlaying)
            return;

        PlayCommandSound();
    }


    private void PlayCommandSound()
    {
        if (command_sound_source == null)
            return;

        if (command_sound == null)
            return;

        command_sound_source.PlayOneShot(command_sound);//play sound
    }



    public void ToggleFollowLeader()
    {
        PlayCommandSound();
        if (owned_squad.ToggleFollowCommander())
        {
            follow_event.Invoke();
            return;
        }

        un_follow_event.Invoke();
    }


    public void ToggleFormation()
    {
        PlayCommandSound();
        ++current_formation_index;

        if (current_formation_index > formations.Count - 1)
            current_formation_index = 0;

        owned_squad.SetFormation(formations[current_formation_index]);
        formation_toggle_event.Invoke(formations[current_formation_index].formation_image);
    }


    public void ToggleCover()
    {
        PlayCommandSound();
        if (owned_squad.ToggleStickToCover())
        {
            find_cover_event.Invoke();
            return;
        }

        no_cover_event.Invoke();
    }


    public void ToggleWeaponsFree()
    {
        PlayCommandSound();
        if (owned_squad.ToggleWeaponsFreeCover())
        {
            weapons_free_event.Invoke();
            return;
        }

        hold_fire_event.Invoke();
    }


    public void ToggleFreeEngage()
    {

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;  
        Gizmos.DrawLine(transform.position, transform.forward * pointer_length);
        UnityEditor.Handles.DrawWireDisc(transform.forward * pointer_length, Vector3.up, pointer_radius);
    }
}
