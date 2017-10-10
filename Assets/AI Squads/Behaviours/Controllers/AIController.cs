using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    public Vector3 waypoint { get; set; }
    public GameObject closest_enemy = null;
    public NavMeshAgent nav_mesh_agent;
    public AIStateSystem.StateMachine ai_state_machine;
    public Character controlled_character;

    public float target_arrival_threshold = 0.5f;
    public float burst_fire_cooldown = 2f;
    public bool is_shooting = false;
    public int max_shot_count = 5;

    [HideInInspector] public CountdownTimer burst_fire_cooldown_timer = new CountdownTimer();
    [HideInInspector] public int shot_count = 0;


    void Start()
    {
        waypoint = transform.position;
        burst_fire_cooldown_timer.InitCountDownTimer(burst_fire_cooldown, false);

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
