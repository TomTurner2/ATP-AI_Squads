using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class DamageFlash : MonoBehaviour
{
    public bool test = false;
    public float flash_speed = 1;
    public float flash_ammount = 0.22f;
    [SerializeField] PostProcessingProfile camera_effects;
	
	
	// Update is called once per frame
	void Update ()
	{
	    if (!test || camera_effects == null)
	        return;

	    var vinette_settings = camera_effects.vignette.settings;
        vinette_settings.intensity = Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup) * flash_ammount);
	    camera_effects.vignette.settings = vinette_settings;
	}


    void OnDestroy()
    {
        var vinette_settings = camera_effects.vignette.settings;
        vinette_settings.intensity = 0;
        camera_effects.vignette.settings = vinette_settings;
    }
}
