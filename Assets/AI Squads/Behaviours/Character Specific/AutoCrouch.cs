using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Directions
{
    NONE = 0,
    FORWARD,
    BACKWARD,//possible future cover animation directions
    LEFT,
    RIGHT,
    UP,
    DOWN
}


public class AutoCrouch : MonoBehaviour
{
    [SerializeField] private CustomEvents.BooleanEvent on_crouch_event;
    private Directions detections = Directions.NONE;


    public void DetectionUp()
    {
        Debug.Log("up");
        BitmaskHelper.Set<Directions>(ref detections, Directions.UP);
    }


    public void DetectionDown()
    {
        Debug.Log("down");
        BitmaskHelper.Set<Directions>(ref detections, Directions.DOWN);
    }


    void Update()
    {
        bool crouch = false;

        if (BitmaskHelper.IsSet<Directions>(detections, Directions.DOWN))
            crouch = true;

        //if (BitmaskHelper.IsSet<Directions>(detections, Directions.UP))
        //    crouch = false;

        on_crouch_event.Invoke(crouch);
    }
	

	void LateUpdate ()
    {
		ResetBitmask();
    }


    private void ResetBitmask()
    {
        BitmaskHelper.Unset(ref detections, Directions.DOWN);
        BitmaskHelper.Unset(ref detections, Directions.UP);
    }
}
