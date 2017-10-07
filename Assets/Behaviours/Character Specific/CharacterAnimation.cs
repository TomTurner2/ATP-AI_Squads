using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] Character character_controller;
    [SerializeField] Animator character_animator;


    void Update()
    {
        if (character_controller == null || character_animator == null)
            return;

        character_animator.SetFloat("speed", character_controller.current_speed);
        character_animator.SetBool("crouching", character_controller.crouching);
    }

}
