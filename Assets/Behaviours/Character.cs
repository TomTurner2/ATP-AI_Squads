using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveRigidbody))]
public class Character : MonoBehaviour
{
    [HideInInspector] public Vector3 move_dir { get; set; }
    [HideInInspector] public bool sprinting { get; set; }
    [HideInInspector] public bool crouching { get; set; }
    [HideInInspector] public bool jumping { get; set; }

    public float walk_speed = 5;
    public float sprint_speed = 10;
    public float crouch_speed = 2;
    public Vector3 jump_force = Vector3.up;

    [SerializeField] private bool scale_model_on_crouch = true;
    
    [Space]
    [Header("References")]
    [SerializeField] private MoveRigidbody character_mover;
    [SerializeField] private Jumper character_jump;
    [SerializeField] private Transform character_model;
    [SerializeField] private CapsuleCollider character_collider;

    private float collider_start_height = 0;
    private float character_model_start_height = 0;


    void Start ()
    {
        move_dir = Vector3.zero;
        sprinting = false;
        jumping = false;
        CheckReferences();
	}


    void CheckReferences()
    {
        if (character_mover == null)
            character_mover = GetComponent<MoveRigidbody>();//in case not manually assigned

        if (character_collider != null)
            collider_start_height = character_collider.height;

        if (character_model)
            character_model_start_height = character_model.localScale.y;
    }


    void Update()
    {
        ProcessStance();
    }


	void FixedUpdate ()
    {
	   MoveCharacter();
	}


    public void Jump()
    {
        if (character_jump == null)
            return;

        jumping = true;
        character_jump.Jump(jump_force);
    }


    private void ProcessStance()
    {
        if (character_collider == null)
            return;

        if (crouching)
        {
            character_collider.height *= collider_start_height * 0.5f;

            if (scale_model_on_crouch)
                character_model.localScale = new Vector3(character_model.localScale.x,
                    character_model_start_height * 0.5f, character_model.localScale.z);

            return;
        }

        if (character_model.localScale.y != character_model_start_height)
            character_model.localScale = new Vector3(character_model.localScale.x,
                character_model_start_height, character_model.localScale.z);

        character_collider.height = collider_start_height;
    }


    private void MoveCharacter()
    {
        if (crouching)
        {
            character_mover.MoveRelative(move_dir, crouch_speed);
            move_dir = Vector3.zero;
            return;
        }

        if (sprinting)
        {
            character_mover.MoveRelative(move_dir, sprint_speed);
            move_dir = Vector3.zero;
            return;
        }

        character_mover.MoveRelative(move_dir, walk_speed);
        move_dir = Vector3.zero;
    }


    public void KillCharacter()
    {
        Destroy(gameObject);
    }

}
