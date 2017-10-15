using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Faction")]
public class Faction : ScriptableObject
{
    public List<Faction> rival_factions = new List<Faction>();
    public List<Faction> allie_factions = new List<Faction>();
}
