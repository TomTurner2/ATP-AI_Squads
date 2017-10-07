using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBob : MonoBehaviour
{
    [SerializeField] float bob_speed = 1;
    [SerializeField] float max_bob = 1;

    private float start_y = 0;

    public bool is_bobbing {get; set;}


    void Start()
    {
        start_y = transform.position.y;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (!is_bobbing)
	        return;

	    float y = start_y + Mathf.Sin(Time.time * bob_speed) * (max_bob);

        transform.position = new Vector3(transform.position.x, y, transform.position.z);
	}
}
