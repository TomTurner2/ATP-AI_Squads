using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform follow_target { get; set; }

    void Update ()
    {
        transform.position = follow_target.position;   
    }
}
