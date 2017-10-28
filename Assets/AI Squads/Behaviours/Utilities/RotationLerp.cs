using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLerp : MonoBehaviour
{
    [SerializeField] float lerp_speed;

    private bool rotating = false;
    private Quaternion from_rotation;
    private Quaternion target_rotation;
    private float t = 0;


    void Start()
    {
        from_rotation = transform.rotation;
        target_rotation = transform.rotation;
    }


	void LateUpdate ()
    {
		if (!rotating)
            return;

        t += Time.unscaledDeltaTime * lerp_speed;
        t = Mathf.Clamp(t, 0, 1);

        if (t >= 1.0f)
        {
            t = 0;
            rotating = false;
            return;
        }
       
        transform.rotation = Quaternion.Slerp(from_rotation, target_rotation, t);
    }


    public void Rotate(float _angle)
    {
        rotating = true;
        t = 0;
        from_rotation = transform.rotation;
        target_rotation *= Quaternion.Euler(Vector3.up * _angle);
    }
}
