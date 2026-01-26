using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("TurnBased Variables")]
    [SerializeField] protected float _speed;
    [SerializeField] protected string _name;
    //[SerializeField] protected Sprite _icon;
    [SerializeField] protected int _teamNumber;

    [Header("Stats")]
    public int MaxHp;
    public int CurrentHP;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StartTurn()
    {
        DoStartTurn();
    }

    protected virtual void DoStartTurn()
    {

    }

    // GETTERS

    public string GetName()
    {
        if (_name != null)
            return _name;
        else
        {
            Debug.Log($"Unit: {gameObject} Has No Valid Name!");
            return null;
        }
    }

    public int GetTeamNumber()
    {
        if (_teamNumber != 0)
            return _teamNumber;
        else
        {
            Debug.Log($"Unit: {gameObject} Has Team Number of Zero!");
            return 0;
        }
    }
    public float GetSpeed()
    {
        if(_speed != 0)
            return _speed;
        else
        {
            Debug.Log($"Unit: {gameObject} Has Speed Value of Zero!");
            return 0;
        }
    }
}
