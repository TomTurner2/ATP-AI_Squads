using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookDirect : MonoBehaviour
{
    [SerializeField] private bool invert_pitch = false;
    [SerializeField] private Vector2 pitch_clamp = new Vector2(-80, 60);
    [SerializeField] private float speed = 10;
    [SerializeField] private Transform pitch_target;
    [SerializeField] private Transform yaw_target;

    private float pitch = 0;
    private float yaw = 0;
    
	void Start ()
	{
	    Cursor.lockState = CursorLockMode.Locked;
        InitTransformTargets();
	}
	

    void InitTransformTargets()
    {
        InitPitch();
        InitYawTarget();
    }


    void InitPitch()
    {
        if (pitch_target == null)
        {
            pitch_target = transform;
            Debug.LogWarning("Pitch target not found, defaulting to own transform");
        }
    }


    void InitYawTarget()
    {
        if (yaw_target == null)
        {
            yaw_target = transform;
            Debug.LogWarning("Pitch target not found, defaulting to own transform");
        }
    }


    void LateUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;

        if (Input.GetMouseButton(0))
            Cursor.lockState = CursorLockMode.Locked;
#endif
        UpdatePitch();  
        UpdateYaw();
    }


    void UpdatePitch()
    {
        if (pitch_target == null)
            return;

        float pitch_change = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;//calculate movement

        if (invert_pitch)//handle mouse inversion
        {
            pitch -= pitch_change;
        }
        else
        {
            pitch += pitch_change;
        }
        
        pitch = Mathf.Clamp(pitch, pitch_clamp.x, pitch_clamp.y);//clamp to angles
        pitch_target.eulerAngles = new Vector3(pitch, pitch_target.eulerAngles.y, pitch_target.eulerAngles.z);//update rotation
    }


    void UpdateYaw()
    {
        if (yaw_target == null)
            return;

        yaw += Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        yaw = Mathf.Repeat(yaw, 360f);//keep rotation within 0 and 360
        yaw_target.eulerAngles = new Vector3(yaw_target.eulerAngles.x, yaw, yaw_target.eulerAngles.z);
    }

}
