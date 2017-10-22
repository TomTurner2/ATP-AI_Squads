using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactionManager : MonoBehaviour
{
    private List<Character> characters = new List<Character>();


    void Start()
    {
        characters = GameObject.FindObjectsOfType<Character>().ToList();
    }


    public Character FindClosestEnemy(Vector3 _position, Faction _requesters_faction, float _enemy_max_check_distance = Mathf.Infinity)
    {
        Character closest_enemy = null;
        float closest_distance = float.PositiveInfinity;

        foreach (var character in characters)
        {
            if (character == null)
                continue;

            if (character.faction == null)
                continue;

            if (character.dead)
                continue;

            if (!CheckFactionRelation(_requesters_faction, character.faction))
                continue;

            float dist = (character.transform.position - _position).sqrMagnitude;
            if (dist >= closest_distance || dist > _enemy_max_check_distance * _enemy_max_check_distance)
                continue;

            closest_distance = dist;
            closest_enemy = character;
        }

        return closest_enemy;
    }


    public bool CheckFactionRelation(Faction _faction_a, Faction _faction_b)
    {
        return _faction_a.rival_factions.Any(rival_faction => rival_faction == _faction_b);
    }


    public void RefreshCharacterList()
    {
        characters = GameObject.FindObjectsOfType<Character>().ToList();
        characters.RemoveAll(c => c == null);
    }
}
