﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeForce : MonoBehaviour
{
    [SerializeField] private int max_health = 100;
    [SerializeField] private UnityEvent on_death_event;
    [SerializeField] private CustomEvents.IntEvent on_damage_event;

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
            on_damage_event = new CustomEvents.IntEvent();
    }


    public void Damage(int _damage)
    {
        current_health -= _damage;//damage health

        if (current_health > 0)
        {
            on_damage_event.Invoke(_damage);//trigger damage event if survived
            return;
        }

        current_health = 0;
        on_death_event.Invoke();//trigger death event
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
