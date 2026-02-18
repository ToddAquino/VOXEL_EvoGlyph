using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UIPopUpGenerator : MonoBehaviour
{
    public static UIPopUpGenerator Instance;
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();
    private static GameObject _daamageNumbersEmpty;

    [SerializeField] private GameObject DamagePopupPrefab;

    private void Awake()
    {
        Instance = this;
        SetupEmpties();
    }
    private void SetupEmpties()
    {
        _daamageNumbersEmpty = new GameObject("Damage Number PopUps");
        _daamageNumbersEmpty.transform.SetParent(this.transform);
    }
    public DamagePopUP CreateDamagePopUP(Vector3 pos, Quaternion spawnRotation, int damage)
    {
        var popup = SpawnObject(DamagePopupPrefab, pos, spawnRotation);
        DamagePopUP dmgUI = popup.GetComponent<DamagePopUP>();
        dmgUI.SetupText(damage.ToString());

        return dmgUI;
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

        //Create pool when it doesn't exist
        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
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
            if(_daamageNumbersEmpty != null)
                spawnableObj.transform.SetParent(_daamageNumbersEmpty.transform);
                    
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
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);

        if (pool != null)
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }

}

public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}