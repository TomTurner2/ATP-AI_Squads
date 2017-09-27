using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class RayInfo
{
    public LayerMask layers_to_check;
    public Vector3 offset;
    public Vector3 direction;
    public float distance;
    public UnityEvent on_ray_hit_event;
}

public class RayCastArray : MonoBehaviour
{
    [SerializeField] private List<RayInfo> rays = new List<RayInfo>();


    void Update ()
    {
        foreach (RayInfo ray in rays)
        {
            Vector3 start = transform.position + ray.offset;

            if (Physics.Raycast(start, ray.direction, ray.distance, ray.layers_to_check))
                ray.on_ray_hit_event.Invoke();
        }
    }


    private void OnDrawGizmos()
    {
        foreach (RayInfo ray in rays)
        {
            Vector3 start = transform.position + ray.offset;
            Vector3 end = transform.position + ray.direction.normalized * ray.distance;

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(start, 0.05f);
            Gizmos.DrawLine(start, end);
            Gizmos.DrawSphere(end, 0.05f);
        }
    }
}
