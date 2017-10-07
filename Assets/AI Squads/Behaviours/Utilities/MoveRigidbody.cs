using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Axis
{
    public bool x;
    public bool y;
    public bool z;
}


public class MoveRigidbody : MonoBehaviour
{
    [SerializeField] private Rigidbody target_rigidbody;
    [SerializeField] private Transform forward_transform;
    [SerializeField] private bool override_velocity = true;
    [SerializeField] private Axis axis_constraints = new Axis();


    public void MoveRelative(Vector3 _direction, float _speed)
    {
        if (target_rigidbody == null)
            return;

        if (forward_transform == null)
            return;

        if (override_velocity)
            OverrideVelocity();

        _direction = SetConstraints(_direction);

        Vector3 new_position = target_rigidbody.position;
        new_position += _direction.normalized.y * forward_transform.forward * _speed * Time.deltaTime;
        new_position += _direction.normalized.x * transform.right * _speed * Time.deltaTime;
        target_rigidbody.MovePosition(new_position);
    }


    public void Move(Vector3 _direction, float _speed)
    {
        if (target_rigidbody == null)
            return;

        if (override_velocity)
            OverrideVelocity();

        _direction = SetConstraints(_direction);

        Vector3 new_position = target_rigidbody.position;
        new_position += _direction.normalized * _speed * Time.deltaTime;
        target_rigidbody.MovePosition(new_position);
    }


    private Vector3 SetConstraints(Vector3 _direction, bool _invert = false)
    {
        if (axis_constraints.x)//lock x movement
            _direction.x = 0;

        if (axis_constraints.y)//lock y movement
            _direction.y = 0;

        if (axis_constraints.z)//lock z movement
            _direction.z = 0;

        return _direction;
    }


    private void OverrideVelocity()//prevents drift when force is applied
    {
        if (target_rigidbody == null)
            return;

        target_rigidbody.velocity = new Vector3(0, target_rigidbody.velocity.y, 0);//TODO take into account axis constraints
    }

}
