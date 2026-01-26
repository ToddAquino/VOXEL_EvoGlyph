using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pattern : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;

    private void Update()
    {
        //always have the end point follow the mouse position
        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //lineRenderer.SetPosition(lineRenderer.positionCount - 1,worldPos);
        
    }

    void AddVertices()
    {
        lineRenderer.positionCount += 1;
    }

    public void UndoLastVertex()
    {
        lineRenderer.positionCount -= 1;
    }

    public void ResetPattern()
    {
        lineRenderer.positionCount = 0;
    }
    void SetPositionOfVert(int vertIndex, Vector3 pos)
    {
        lineRenderer.SetPosition(vertIndex, pos);
    }

    public void SnapToPosition(Vector3 pos)
    {
        AddVertices();
        SetPositionOfVert(lineRenderer.positionCount - 1, pos);
    }
}
