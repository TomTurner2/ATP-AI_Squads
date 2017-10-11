using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gun/GunStats")]
public class GunStats : ScriptableObject
{
    public int damage = 10;
    public float effective_range = 20;
    public float accuracy = 0.2f;
    public float shot_delay = 0.05f;
}
