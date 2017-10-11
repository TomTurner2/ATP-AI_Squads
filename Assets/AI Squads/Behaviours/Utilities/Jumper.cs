using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private Rigidbody target_rigidbody;

    [HideInInspector] public bool can_jump { get; set; }

    private bool jumping = false;
    private Vector3 jump_force = Vector3.zero;


    private void Start()
    {
        can_jump = true;
    }


    public void CanJump(bool _can_jump)
    {
        can_jump = _can_jump;
    }


    public void Jump(Vector3 _force)
    {
        if (target_rigidbody == null)
            return;

        jumping = true;
        jump_force = _force;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (target_rigidbody == null)
            return;

        if (!jumping)
            return;

        if (can_jump)
        {
            target_rigidbody.AddForce(jump_force, ForceMode.Impulse);
            can_jump = false;
        }
        
        jumping = false;
    }
}
