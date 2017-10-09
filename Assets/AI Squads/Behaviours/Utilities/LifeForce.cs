using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeForce : MonoBehaviour
{
    [SerializeField] private int max_health = 100;
    [SerializeField] private ParticleSystem hit_effect;
    [SerializeField] private UnityEvent on_death_event;
    [SerializeField] private CustomEvents.DamageEvent on_damage_event;

    private int current_health = 100;

    void Awake()
    {
        CreateEvents();
    }


    private void CreateEvents()
    {
        if (on_death_event == null)
            on_death_event = new UnityEvent();//create event

        if (on_damage_event == null)
            on_damage_event = new CustomEvents.DamageEvent();
    }


    public bool Damage(int _damage, RaycastHit _hit)
    {
        current_health -= _damage;//damage health

        if (hit_effect != null)
        {
            hit_effect.transform.position = _hit.point;
            hit_effect.transform.up = _hit.normal;
            hit_effect.Emit(30);
        }

        if (current_health > 0)
        {
            on_damage_event.Invoke(_damage, _hit);//trigger damage event if survived
            return false;
        }

        current_health = 0;
        on_death_event.Invoke();//trigger death event
        return true;
    }


    public void Heal(int _heal_amount)
    {
        current_health += _heal_amount;
        current_health = Mathf.Clamp(current_health, 0, max_health);//clamp to max value
    }


    public void ResetHealth()
    {
        current_health = max_health;
    }


    public void SetMaxHealth(int _max_health, bool _update_current_health = true)
    {
        max_health = _max_health;

        if (!_update_current_health)
            return;

        ResetHealth();//update current health if specified
    }

}
