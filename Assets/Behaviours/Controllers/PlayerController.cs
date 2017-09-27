using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
    public CustomEvents.Vector3Event on_move_event;
    public CustomEvents.BooleanEvent on_sprint_event;
    public UnityEvent on_jump_event;
    
    void Update ()
	{
	    Vector3 move_input = new Vector3(Input.GetAxisRaw("Right"), -Input.GetAxisRaw("Forward"));
        on_move_event.Invoke(move_input.normalized);
        on_sprint_event.Invoke(Input.GetButton("Sprint"));
	    
        if (Input.GetButton("Jump"))
            on_jump_event.Invoke();
	}
}