using System.Collections;
using UnityEngine;


public class Gun : MonoBehaviour
{
    [SerializeField] private GunStats stats;

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

    private CountdownTimer fire_rate_timer = new CountdownTimer();


    void Start()
    {
        if (stats == null)
            stats = new GunStats();

        fire_rate_timer.InitCountDownTimer(stats.shot_delay, false);//don't auto reset, this is done when a shot is fired
    }


    public bool Shoot()
    {
        if (audio_source == null || gun_shot_clip == null)
            return false;

        if (!fire_rate_timer.CheckFinished())//if gun not ready for next shot: bail
            return false;

        fire_rate_timer.Reset();//start the fire rate cooldown

        audio_source.pitch = Random.Range(min_pitch, max_pitch);
        audio_source.PlayOneShot(gun_shot_clip);//play pitch shifted sound

        if (muzzle_flash == null)
            return false;

        muzzle_flash.Emit(30);
        FireBullet();

        return true;//shoot was successfull
    }


    void Update()
    {
        fire_rate_timer.UpdateTimer();

        if (gun_animator == null)
            return;

        gun_animator.SetBool("firing", firing_weapon);
    }


    private void FireBullet()
    {
        RaycastHit hit;

        GameObject bullet = Instantiate(bullet_prefab);
        LineRenderer bullet_trail = bullet.GetComponent<LineRenderer>();//create bullet and get refs

        bullet_trail.SetPosition(0, muzzle_position.position);//set trail start
        Vector3 end_point = CalculateTrailEndPoint();
        bullet_trail.SetPosition(1, end_point);//set end point

        Destroy(bullet, .3f);//destroy the trail after some time

        if (!Physics.Raycast(muzzle_position.position, muzzle_position.forward, out hit,
            stats.effective_range))//shoot ray
            return;

        if (hit.distance < stats.effective_range)//if in guns range
            end_point = hit.point;
        
        bullet_trail.SetPosition(1, end_point);//set trail end point to hit location


        LifeForce target_life = hit.collider.gameObject.GetComponent<LifeForce>() ??
                                hit.collider.gameObject.GetComponentInParent<LifeForce>();//attempt to get life component

        if (target_life != null)
            target_life.Damage(stats.damage, hit);//if hit something with life, apply damage

        //TODO add impact effect
    }


    private Vector3 CalculateTrailEndPoint()
    {
        return muzzle_position.position + (transform.forward * stats.effective_range);
    }


    public void Aim(Vector3 _aim_target)
    {
        muzzle_position.LookAt(_aim_target);
    }

}
