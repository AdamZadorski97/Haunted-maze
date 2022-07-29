using UnityEngine;
using System;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "EnemyProportiesData", menuName = "ScriptableObjects/EnemyProportiesData", order = 1)]

[Serializable]
public class EnemyProporties : ScriptableObject
{
    public string enemyName;
    public float hp;
    public float speed;
}
