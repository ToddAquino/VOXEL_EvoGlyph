using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle/Enemy Data")]
public class EnemyUnitData : ScriptableObject
{
    public GameObject EnemyUnitPrefab;
    public int MaxHP = 30;
    public int level = 1;
    public ElementType Element;

    [Header("Mana Drop System")]
    public float ManaDropPercent;
    public int ManaDropAmount = 1;

    //public GameObject EnemyPrefab;

    //public List<SpellData> spells;
    public SpellData spellToCast;
    public bool RollManaChance()
    {
        return UnityEngine.Random.value <= ManaDropPercent / 100f;
    }
}
