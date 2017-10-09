using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    public Vector3 waypoint { get; set; }
    public NavMeshAgent nav_mesh_agent;

    [SerializeField] float target_arrival_threshold = 0.5f;
    [SerializeField] Character controlled_character;
 

    void Start()
    {
        waypoint = transform.position; 
    }


    void Update()
    {
        nav_mesh_agent.isStopped = controlled_character.dead;

        if (Vector3.Distance(transform.position, waypoint) >= 1)
            MoveToPosition(waypoint);
    }


    private void DetermineSpeed()
    {
        if (nav_mesh_agent == null)
            return;

        if (controlled_character.crouching)
        {
            nav_mesh_agent.speed = controlled_character.crouch_speed;//use crouch speed
            return;
        }

        if (controlled_character.sprinting)
        {
            nav_mesh_agent.speed = controlled_character.sprint_speed;//use sprint speed
            return;
        }

        nav_mesh_agent.speed = controlled_character.walk_speed;//default to walk speed
    }


    public bool MoveToPosition(Vector3 _position)
    {
        DetermineSpeed();
        return nav_mesh_agent.SetDestination(_position);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(waypoint, .5f);
    }

}
