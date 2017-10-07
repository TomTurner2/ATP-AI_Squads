using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Animator gun_animator;
    [SerializeField] private AudioSource audio_source;
    [SerializeField] private AudioClip gun_shot_clip;
    [SerializeField] private float min_pitch = 1;
    [SerializeField] private float max_pitch = 2;

    public bool firing_weapon { get; set; }

    public void Shoot()
    {
        if (audio_source == null || gun_shot_clip == null)
            return;

        audio_source.pitch = Random.Range(min_pitch, max_pitch);
        audio_source.PlayOneShot(gun_shot_clip);
    }


    void Update()
    {

        if (gun_animator == null)
            return;

        gun_animator.SetBool("firing", firing_weapon);
    }

}
