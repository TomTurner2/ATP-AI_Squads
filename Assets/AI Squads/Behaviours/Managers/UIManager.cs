using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject gameplay_ui_canvas;
    [SerializeField] GameObject game_over_ui_canvas;
    [SerializeField] Text enemy_counter_text;
    [SerializeField] Text enemies_killed_text;
    [SerializeField] Text score_text;
    [SerializeField] Text final_score_text;

    private int enemy_count = 0;
    private int enemies_killed = 0;
    private int score = 0;
    private List<GameObject> killed_enemies = new List<GameObject>();


    public void EnableGameplayUI(bool _enabled)
    {
        gameplay_ui_canvas.SetActive(_enabled);
    }


    public void EnableGameOverUI(bool _enabled)
    {
        game_over_ui_canvas.SetActive(_enabled);
    }


    public void UpdateEnemyCount(int _count)
    {
        killed_enemies.Clear();
        enemy_count = _count;
        enemy_counter_text.text = enemy_count.ToString();
    }


    public void RegisterEnemy(AIController _enemy)
    {
        LifeForce life_force = _enemy.GetComponent<LifeForce>() ??
                               _enemy.GetComponentInChildren<LifeForce>();

        if (life_force == null)
            return;

        life_force.on_death_event.AddListener(EnemyDeath);
    }


    public void EnemyDeath(GameObject _enemy)
    {
        if (killed_enemies.Contains(_enemy))//in case event gets fired twice
            return;

        killed_enemies.Add(_enemy);

        score += GameManager.score_per_kill;
        score = Mathf.Clamp(score, 0, 9999);
        score_text.text = score.ToString("D4");
        final_score_text.text = score.ToString();
     
        enemies_killed_text.text = enemies_killed++.ToString();
        enemy_counter_text.text = Mathf.Clamp(enemy_count--, 0, int.MaxValue).ToString();
    }
}
