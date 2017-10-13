﻿using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    public NavMeshAgent nav_mesh_agent;
    public AIStateSystem.StateMachine ai_state_machine;
    public Character controlled_character;
    public AIStateSystem.Knowledge knowledge;
    public Gun weapon;


    void Start()
    {
        knowledge = knowledge != null ? Instantiate(knowledge) : new Knowledge();

        knowledge.waypoint = transform.position;
        knowledge.best_cover = transform.position;
        knowledge.burst_fire_cooldown_timer.InitCountDownTimer(knowledge.burst_fire_cooldown, false);

        if (ai_state_machine == null)
            return;

        ai_state_machine = Instantiate(ai_state_machine);
        ai_state_machine.InitStateMachine();
    }


    void Update()
    {
        if (ai_state_machine != null)
            ai_state_machine.UpdateState(this);
    }


    private void OnDestroy()
    {
        if (ai_state_machine != null)
            Destroy(ai_state_machine);
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


    public void MoveToPosition(Vector3 _position)
    {
        DetermineSpeed();
        nav_mesh_agent.SetDestination(_position);
    }

}
