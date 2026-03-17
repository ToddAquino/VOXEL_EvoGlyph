using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public event Action<Checkpoint> CheckpointSet;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Checkpoint Set");
            CheckpointSet?.Invoke(this);
        }
    }
}
