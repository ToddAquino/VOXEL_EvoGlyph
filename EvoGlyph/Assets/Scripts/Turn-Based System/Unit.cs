using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("TurnBased Variables")]
    [SerializeField] protected float _speed;
    [SerializeField] protected string _name;
    [SerializeField] protected Sprite _icon;
    [SerializeField] protected int _teamNumber;
    [SerializeField] private TurnDeck turnDeck;
    void Start()
    {
        
    }

    void Update()
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

    public Sprite GetIcon()
    {
        if (_icon != null)
            return _icon;
        else
        {
            Debug.Log($"Unit: {gameObject} Has No Valid Icon!");
            return null; //Could be set with placeholder "?" Icon
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

    //SETTERS

    public void SetTurnDeck(TurnDeck deck)
    {
        turnDeck = deck;
    }
}
