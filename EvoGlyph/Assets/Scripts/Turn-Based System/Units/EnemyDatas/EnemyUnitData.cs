using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle/Enemy Data")]
public class EnemyUnitData : ScriptableObject
{
    public GameObject EnemyUnitPrefab;
    public int maxHP = 30;
    public int level = 1;

    public ElementType element;

    //public GameObject EnemyPrefab;

    public List<SpellData> spells;
}
