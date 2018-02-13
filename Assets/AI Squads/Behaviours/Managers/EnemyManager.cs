using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


public class EnemyManager : MonoBehaviour
{  
    [SerializeField] float round_end_delay = 2;
    [SerializeField] UnityEvent on_all_dead;
    [SerializeField] CustomEvents.AIControllerEvent on_enemy_added;
    [SerializeField] CustomEvents.IntEvent on_enemies_refreshed;
    [SerializeField] Faction enemy_faction;

    private List<AIController> all_enemy_ai = new List<AIController>();
    private bool restart = false;


    void Start()
    {
        RefreshList();
    }


    void Update()
    {
        if (EnemyRemains())//check if all enemies are dead
            return;

        Invoke("NextLevelDelay", round_end_delay);//next level if no enemies left
        restart = true;
    }


    bool EnemyRemains()
    {
        return (all_enemy_ai.Any(enemy => !enemy.controlled_character.dead) || all_enemy_ai.Count <= 0 || restart);
    }


    public void RefreshList()
    {
        all_enemy_ai = FindObjectsOfType<AIController>().ToList();
        all_enemy_ai.RemoveAll(c => c == null || c.controlled_character.faction != enemy_faction);//clean up garbage references and non enemies
        all_enemy_ai.ForEach(a => on_enemy_added.Invoke(a));//enemy added event for each element
        on_enemies_refreshed.Invoke(GetAliveEnemyCount());//broadcast how many alive enemies in level
    }


    public void NextLevelDelay()
    {
        on_all_dead.Invoke();
        restart = false;
    }


    public int GetAliveEnemyCount()
    {     
        return all_enemy_ai.Count(a => !a.controlled_character.dead);
    }


    public void EnableAI(bool _enabled)
    {
        RefreshList();
        foreach (AIController ai_controller in all_enemy_ai)
        {
            if (ai_controller == null)
                continue;

            ai_controller.enabled = _enabled;
            ai_controller.nav_mesh_agent.enabled = _enabled;//set ai behaviours on or off
        }
    }


    public void UpdateDestinations()
    {
        foreach (AIController ai_controller in all_enemy_ai)
        {
            if (ai_controller == null)
                continue;

            if (ai_controller.nav_mesh_agent.enabled)//update waypoint to local positions
                ai_controller.nav_mesh_agent.destination = ai_controller.transform.localPosition;
        }
    }

}
