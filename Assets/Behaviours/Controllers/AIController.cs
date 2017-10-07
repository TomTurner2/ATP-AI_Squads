﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    public Vector3 waypoint { get; set; }
    public NavMeshAgent nav_mesh_agent;

    [SerializeField] Character controlled_character;
 

    void Start()
    {
        waypoint = transform.position; 
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
        //controlled_character.current_speed = nav_mesh_agent.velocity.z;
        return nav_mesh_agent.SetDestination(_position);
    }
    
}
