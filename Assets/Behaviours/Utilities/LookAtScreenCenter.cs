using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtScreenCenter : MonoBehaviour
{
    [SerializeField] private Transform transform_target;
	
	// Update is called once per frame
	void Update ()
	{
	    if (transform_target == null)
	        return;
		
        transform_target.LookAt(Camera.main.transform.position + Camera.main.transform.forward * 2);
    }
}
