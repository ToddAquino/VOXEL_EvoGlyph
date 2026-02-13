using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

public class SpellSpawner : MonoBehaviour
{
    public static SpellSpawner Instance;
    public static List<PooledObjectInfo> SpellPools = new List<PooledObjectInfo>();
    private static GameObject _spellPrefabsEmpty;

    private void Awake()
    {
        Instance = this;
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _spellPrefabsEmpty = new GameObject("Spell Prefab Pool");
        _spellPrefabsEmpty.transform.SetParent(this.transform);
    }

    public Spell CreateSpellPrefab(GameObject spellPrefab, Vector3 pos, Quaternion spawnRotation)
    {
        var spellObj = SpawnObject(spellPrefab, pos, spawnRotation);
        Spell spell = spellObj.GetComponent<Spell>();
        return spell;
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        PooledObjectInfo pool = SpellPools.Find(p => p.LookupString == objectToSpawn.name);

        //Create pool when it doesn't exist
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            SpellPools.Add(pool);
        }

        GameObject spawnableObj = null;
        foreach (GameObject obj in pool.InactiveObjects)
        {
            if (obj != null)
            {
                spawnableObj = obj;
                break;
            }
        }

        // if no Inactive ObjectsAvailable
        if (spawnableObj == null)
        {
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
            spawnableObj.transform.SetParent(_spellPrefabsEmpty.transform);

        }
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }
    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Replace("(Clone)", string.Empty);
        PooledObjectInfo pool = SpellPools.Find(p => p.LookupString == goName);

        if (pool != null)
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }
}
