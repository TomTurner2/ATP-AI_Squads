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
    public Color debug_color = Color.white;
    public CustomEvents.Vector3Event on_ray_hit_event;
    public UnityEvent on_ray_miss_event;
}


public class RayCastArray : MonoBehaviour
{
    [SerializeField] private float update_delay = 0;
    [SerializeField] List<RayInfo> rays /*= new List<RayInfo>()*/;

    private CountdownTimer timer = new CountdownTimer();


    void Start()
    {
        timer.InitCountDownTimer(update_delay);
    }


    void Update ()
    {
        timer.timer_duration = update_delay;//in case updated

        if (!timer.UpdateTimer() && timer.timer_duration > 0)//only execute after timed delay
            return;

        FireRays();
    }


    private void FireRays()
    {
        foreach (RayInfo ray in rays)
        {
            Vector3 start = transform.position + ray.offset;
            RaycastHit hit;
            if (Physics.Raycast(start, ray.direction + ray.offset, out hit, ray.distance, ray.layers_to_check))
            {
                ray.on_ray_hit_event.Invoke(hit.point);
                continue;
            }
            ray.on_ray_miss_event.Invoke();
        }
    }


    private void OnDrawGizmos()
    {
        foreach (RayInfo ray in rays)
        {
            Vector3 start = transform.position + ray.offset;
            Vector3 end = start + ray.direction.normalized * ray.distance ;

            Gizmos.color = ray.debug_color;
            Gizmos.DrawSphere(start, 0.05f);
            Gizmos.DrawLine(start, end);
            Gizmos.DrawSphere(end, 0.05f);
        }
    }
}
