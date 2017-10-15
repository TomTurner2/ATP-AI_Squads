using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowFormationLeader : MonoBehaviour
{
    public Transform follow_target { get; set; }
    public float distance_before_align = 0.5f;
    public Vector3 previous_position { get; set; }


    public void InitFollow(Transform _follow_target)
    {
        follow_target = _follow_target.transform;
        previous_position = _follow_target.transform.position;
        transform.forward = -_follow_target.transform.forward;
    }


    void Update ()
    {
        if (follow_target == null)
            return;

        Vector3 move = follow_target.position - previous_position;
        float dist = move.magnitude;

        if (dist >= distance_before_align)
        {
            transform.forward = -move.normalized;
            previous_position = follow_target.position;
        }

        transform.position = follow_target.position;
    }
}
