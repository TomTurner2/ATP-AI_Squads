using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    public Vector3 waypoint { get; set; }

    [SerializeField] private Character controlled_character;
    [SerializeField] private NavMeshAgent nav_mesh_agent;
    

    void Start()
    {
        waypoint = transform.position;
        controlled_character.sprinting = true;   
    }


    void Update()
    {
        MoveToPosition(waypoint);
    }


    private void DetermineSpeed()
    {
        if (nav_mesh_agent == null)
            return;

        if (controlled_character.crouching)
        {
            nav_mesh_agent.speed = controlled_character.crouch_speed;
            return;
        }

        if (controlled_character.sprinting)
        {
            nav_mesh_agent.speed = controlled_character.sprint_speed;
            return;
        }

        nav_mesh_agent.speed = controlled_character.walk_speed;
    }


    public bool MoveToPosition(Vector3 _position)
    {
        DetermineSpeed();
        return nav_mesh_agent.SetDestination(_position);
    }
    
}
