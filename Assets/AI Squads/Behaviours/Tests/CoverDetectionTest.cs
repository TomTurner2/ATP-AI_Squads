using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverDetectionTest : MonoBehaviour
{
    public Faction test_faction;
    public WMEnemyLineOfSight test_line_of_site;
    public float radius = 5.0f;
    public float cover_radius =.5f;
    public int sample_count = 200;
    public int best_pick_count = 4;
    public float update_delay = 2;
    private List<Vector3> nearest_cover = new List<Vector3>();
    private float t = 0;


    void Start()
    {
        t = update_delay;
        nearest_cover = GameManager.scene_refs.tactical_assessor.FindOptimalCoverInArea(transform.position, radius, test_faction, sample_count);
    }


	// Update is called once per frame
	void Update ()
	{
	    t -= Time.deltaTime;

	    if (t > 0)
	        return;

	    t = update_delay;
	    nearest_cover = GameManager.scene_refs.tactical_assessor.FindOptimalCoverInArea(transform.position, radius, test_faction, sample_count);
	}


    public void LockToGround(Vector3 _ground_pos)
    {
        transform.position = new Vector3(transform.position.x, _ground_pos.y, transform.position.z);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.5f);
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.green;
        int count = 0;
        float sphere_size = 0.2f;

        foreach (Vector3 point in nearest_cover)
        {
            ++count;

            if (count > best_pick_count)
            {
                Gizmos.color = Color.red;
                sphere_size = 0.1f;
            }

            Gizmos.DrawSphere(point, sphere_size);
        }
    }

}
