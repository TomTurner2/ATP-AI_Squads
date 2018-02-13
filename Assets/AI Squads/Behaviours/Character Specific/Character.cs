using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MoveRigidbody))]
public class Character : MonoBehaviour
{
    [HideInInspector] public bool dead { get; set; }
    [HideInInspector] public Vector3 move_dir { get; set; }
    [HideInInspector] public bool sprinting { get; set; }
    [HideInInspector] public bool crouching { get; set; }
    [HideInInspector] public bool jumping { get; set; }
    [HideInInspector] public float current_speed { get; set; }

    [Header("Character Stats")]
    public Faction faction;
    public float walk_speed = 5;
    public float sprint_speed = 10;
    public float crouch_speed = 2;
    public Vector3 jump_force = Vector3.up;
    public float current_move_speed = 0;
    
    [Space]
    [Header("References")]
    [SerializeField] MoveRigidbody character_mover;
    [SerializeField] Jumper character_jump;
    public Collider character_collider;

    private Vector3 previous_position;


    private void Start ()
    {
        Init();
        CheckReferences();
	}


    private void Init()
    {
        dead = false;
        current_move_speed = walk_speed;
        move_dir = Vector3.zero;
        sprinting = false;
        jumping = false;
    }


    private void CheckReferences()
    {
        if (character_mover == null)
            character_mover = GetComponent<MoveRigidbody>();//in case not manually assigned

        if (character_collider == null)
            character_collider = GetComponentInChildren<Collider>();
    }


    private void CheckSpeed()
    {
        Vector3 move = transform.position - previous_position;
        float speed = move.magnitude * current_move_speed / Time.deltaTime;
        previous_position = transform.position;
        current_speed = speed;
    }


    private void Update()
    {
        CheckSpeed();
        ProcessStance();
    }


	private void FixedUpdate ()
    {
	   MoveCharacter();
	}


    public void Jump()
    {
        if (character_jump == null)
            return;

        character_jump.Jump(jump_force);
        jumping = true;
    }


    private void ProcessStance()
    {
        if (crouching)
        {
            current_move_speed = crouch_speed;
            return;
        }

        if (sprinting)
        {
            current_move_speed = sprint_speed;
            return;
        }

        current_move_speed = walk_speed;
    }


    private void MoveCharacter()
    {
        character_mover.MoveRelative(move_dir, current_move_speed);
        move_dir = Vector3.zero;
    }


    public void KillCharacter()
    {
        dead = true;
    }

}
