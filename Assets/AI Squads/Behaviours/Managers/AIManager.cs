using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AIManager : MonoBehaviour
{
    private List<AIController> ai = new List<AIController>();
    [SerializeField] private float round_end_delay = 2;
    [SerializeField] private UnityEvent on_all_dead;
    [SerializeField] private Faction enemy_faction;
    private int death_count = 0;
    private bool restart = false;

    void Start()
    {
        RefreshList();
    }


    void Update()
    {
        if (ai.Any(a => a.controlled_character.dead == false) || ai.Count <= 0 || restart == true)
            return;

        Invoke("NextLevelDelay", round_end_delay);
        restart = true;
    }


    public void RefreshList()
    {
        ai = GameObject.FindObjectsOfType<AIController>().ToList();

        ai.RemoveAll(c => c == null);
        ai.RemoveAll(c => c.controlled_character.faction != enemy_faction);
        death_count = ai.Count;

        foreach (AIController ai_controller in ai)
        {
            LifeForce life_force = ai_controller.GetComponent<LifeForce>() ??
                                   ai_controller.GetComponentInChildren<LifeForce>();

            if (life_force == null)
                return;

            life_force.on_death_event.AddListener(OnEnemyDeath);
        }
    }


    public void OnEnemyDeath(GameObject _victim)
    {
        --death_count;

        if (death_count <= 0)
        {
            //Invoke("NextLevelDelay", round_end_delay);
            death_count = 0;
        }
    }


    public void NextLevelDelay()
    {
        on_all_dead.Invoke();
        restart = false;
    }


    public void EnableAI(bool _enabled)
    {
        RefreshList();
        foreach (AIController ai_controller in ai)
        {
            if (ai_controller == null)
                continue;

            ai_controller.enabled = _enabled;
            ai_controller.nav_mesh_agent.enabled = _enabled;
        }
    }


    public void UpdateDestinations()
    {
        foreach (AIController ai_controller in ai)
        {
            if (ai_controller == null)
                continue;

            if (ai_controller.nav_mesh_agent.enabled)
                ai_controller.nav_mesh_agent.destination = ai_controller.transform.localPosition;
        }
    }
}
