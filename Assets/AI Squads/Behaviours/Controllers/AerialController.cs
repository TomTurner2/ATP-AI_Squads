using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.EventSystems;

public class AerialController : MonoBehaviour
{
    [SerializeField] CustomEvents.Vector3Event update_look_target;
    [SerializeField] CustomEvents.BooleanEvent on_command_event;
    [SerializeField] List<KeyEvent> key_events = new List<KeyEvent>();


    private void Start()
    {
        Cursor.visible = false;
    }


	private void Update ()
	{
	    if (EventSystem.current.IsPointerOverGameObject())
	    {
	        on_command_event.Invoke(false);//if over ui element disable command
            Cursor.visible = true;//enable cursor
        }
	    else
	    {
	        on_command_event.Invoke(true);
	        Cursor.visible = false;
        }
	    
        ProcessKeyInputs();
	}


    private void LateUpdate()
    {
        CheckLookTarget();
    }


    private void CheckLookTarget()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//ray from mouse position

        if (Physics.Raycast(ray, out hit))
            update_look_target.Invoke(hit.point);
    }


    private void ProcessKeyInputs()
    {
        foreach (KeyEvent key in key_events)
        {
            if (Input.GetKeyDown(key.key))
                key.on_press_event.Invoke();
        }
    }
}
