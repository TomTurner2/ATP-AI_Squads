using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] bool is_player_gun = false;
    [Header("Stats")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float effective_range = 20;
    [SerializeField] private float accuracy = 0.2f;

    [Header("Visuals")]
    [SerializeField] private Animator gun_animator;
    [SerializeField] private ParticleSystem muzzle_flash;
    [SerializeField] private Transform muzzle_position;
    [SerializeField] private GameObject bullet_prefab;

    [Header("Audio")]
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

        if (muzzle_flash == null)
            return;

        muzzle_flash.Emit(30);
        FireBullet();
    }


    void Update()
    {
        if (gun_animator == null)
            return;

        gun_animator.SetBool("firing", firing_weapon);
    }


    void LateUpdate()
    {
        Aim();
    }


    private void FireBullet()
    {
        RaycastHit hit;

        GameObject bullet = Instantiate(bullet_prefab);
        LineRenderer bullet_trail = bullet.GetComponent<LineRenderer>();

        bullet_trail.SetPosition(0, muzzle_position.position);
        Vector3 end_point = muzzle_position.position + (transform.forward * effective_range);
        bullet_trail.SetPosition(1, end_point);

        Destroy(bullet, .3f);

        if (!Physics.Raycast(muzzle_position.position, muzzle_position.forward, out hit,
            effective_range))
            return;

        if (hit.distance < effective_range)
            end_point = hit.point;
        
        bullet_trail.SetPosition(1, end_point);


        LifeForce target_life = hit.collider.gameObject.GetComponent<LifeForce>() ??
                                hit.collider.gameObject.GetComponentInParent<LifeForce>();

        if (target_life != null)
        {
            target_life.Damage(damage, hit);
        }

        //TODO add impact effect
    }


    void Aim()
    {
        if (!is_player_gun)
            return;

        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height) * 0.5f);

        if (Physics.Raycast(ray, out hit))
            muzzle_position.LookAt(hit.point);
    }

}
