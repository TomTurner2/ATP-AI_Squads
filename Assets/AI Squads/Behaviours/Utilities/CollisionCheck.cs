using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class CollisionCheck : MonoBehaviour
{
    [SerializeField] private LayerMask layers_to_check;
    [SerializeField] private UnityEvent on_collision_enter;
    [SerializeField] private UnityEvent on_collision_stay;
    [SerializeField] private UnityEvent on_collision_exit;


    private void OnCollisionEnter(Collision _collision)
    {
        if (!LayerContains(layers_to_check, _collision.gameObject.layer))
            return;
        
        on_collision_enter.Invoke();
    }


    private void OnCollisionStay(Collision _collision)
    {
        if (!LayerContains(layers_to_check, _collision.gameObject.layer))
            return;

        on_collision_stay.Invoke();
    }


    private void OnCollisionExit(Collision _collision)
    {
        if (!LayerContains(layers_to_check, _collision.gameObject.layer))
            return;

        on_collision_exit.Invoke();  
    }


    public bool LayerContains(LayerMask _mask, int _layer)
    {
        return _mask != (_mask | (1 << _layer));//check if layer is in layer mask
    }
}
