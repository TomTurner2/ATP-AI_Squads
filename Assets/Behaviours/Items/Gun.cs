using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Animator gun_animator;

    public bool firing_weapon { get; set; }

    public void Shoot()
    {
        
    }


    void Update()
    {

        if (gun_animator == null)
            return;

        gun_animator.SetBool("firing", firing_weapon);
    }

}
