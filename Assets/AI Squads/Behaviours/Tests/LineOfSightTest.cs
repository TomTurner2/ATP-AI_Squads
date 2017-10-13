using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightTest : MonoBehaviour
{
    Transform target = null;
    [SerializeField] List<Collider> ignore;

	void Start ()
    {
        GameObject target_object = new GameObject();
        target = target_object.transform;
        target.parent = transform;
        target.position = transform.position;
        target.name = "Line Of Sight Target";
    }


    private void OnDrawGizmos()
    {
        if (target == null)
            return;

        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(target.transform.position, 0.1f);
        Gizmos.DrawSphere(transform.position, 0.1f);

        if (ignore.Count > 0)
        {
            if (LineOfSightCheck.CheckLineOfSight(transform.position, target.position, ignore))
            {
                Gizmos.color = Color.white;
            }
            else
            {
                Gizmos.color = Color.red;
            }
        }
        else
        {
            if (LineOfSightCheck.CheckLineOfSight(transform.position, target.position))
            {
                Gizmos.color = Color.white;
            }
            else
            {
                Gizmos.color = Color.red;
            }
        }


        Gizmos.DrawLine(transform.position, target.position);
            
    }


}
