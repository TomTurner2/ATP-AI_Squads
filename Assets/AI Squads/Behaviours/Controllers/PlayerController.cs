using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
    [SerializeField] CustomEvents.Vector3Event update_look_target;
    [SerializeField] CustomEvents.Vector3Event on_move_event;
    [SerializeField] CustomEvents.BooleanEvent on_sprint_event;
    [SerializeField] CustomEvents.BooleanEvent on_command_event;
    [SerializeField] CustomEvents.BooleanEvent on_fire_held_event;
    [SerializeField] UnityEvent on_fire_event;  
    [SerializeField] UnityEvent on_jump_event;
    [SerializeField] UnityEvent on_issue_command_event;


    void Update ()
	{
	   AxisEvents();
       ButtonPressEvents();
       ButtonHeldEvents();  
	}


    void LateUpdate()
    {
        CheckLookTarget();
    }


    private void CheckLookTarget()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height) * 0.5f);

        if (Physics.Raycast(ray, out hit))
            update_look_target.Invoke(hit.point);
    }


    private void AxisEvents()
    {
        Vector3 move_input = new Vector3(Input.GetAxisRaw("Right"), -Input.GetAxisRaw("Forward"));
        on_move_event.Invoke(move_input.normalized);
    }


    private void ButtonHeldEvents()
    {
        on_sprint_event.Invoke(Input.GetButton("Sprint"));
        on_command_event.Invoke(Input.GetButton("IndicateCommand"));
        on_fire_held_event.Invoke(Input.GetButton("Fire1"));

    }


    private void ButtonPressEvents()
    {
        if (Input.GetButtonDown("Jump"))
            on_jump_event.Invoke();

        if (Input.GetButton("Fire1"))
            on_fire_event.Invoke();

        if (Input.GetButton("IssueCommand"))
            on_issue_command_event.Invoke();
    }
}