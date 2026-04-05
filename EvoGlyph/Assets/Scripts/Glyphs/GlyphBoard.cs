using EditorAttributes;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GlyphBoard : MonoBehaviour
{
    public static GlyphBoard Instance;

    int gridSize = 4;
    public float _spacing = 1.25f;
    GlyphNode[,] grid;
    public List<GlyphNode> Nodes = new List<GlyphNode>();
    [SerializeField] private GameObject m_GlyphNodesObj;
    [SerializeField] private Transform m_GridTransform;
    [Header("NodeSprites")]
    [SerializeField] Sprite[] unlitNodeSprites;
    [SerializeField] Sprite[] litNodeSprites;

    private void Awake()
    {
        Instance = this;
        m_GlyphNodesObj.SetActive(false);
    }

    [Button]
    public void DebugRefreshField()
    {
        int nodeIndex = 0;
        grid = new GlyphNode[gridSize, gridSize];

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                GameObject _n = Nodes[nodeIndex].gameObject; 
                int flippedY = (gridSize - 1) - y;
                _n.transform.localPosition = new Vector2(_spacing * x, _spacing * flippedY);
                nodeIndex++;
            }
        }
    }
    public void GenerateField()
    {     
        CreateNodes();
        //AssignNeighbors();
    }
    public void ClearField()
    {
        DeleteNodes();
    }
    void DeleteNodes()
    {
        for (int i = Nodes.Count - 1; i >= 0; i--)
        {
            Destroy(Nodes[i].gameObject);
        }

        Nodes.Clear();
    }
    void CreateNodes()
    {
        int nodeIndex = 1;
        grid = new GlyphNode[gridSize, gridSize];

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                GameObject _n = Instantiate(m_GlyphNodesObj, m_GridTransform, true);
                _n.SetActive(true);
                _n.name = $"Node: {nodeIndex}";
                int flippedY = (gridSize - 1) - y;
                _n.transform.localPosition = new Vector2(_spacing * x, _spacing * flippedY);    

                GlyphNode node = _n.GetComponent<GlyphNode>();
                int spriteIndex = nodeIndex % unlitNodeSprites.Length;
                node.InitializeNodeSprites(unlitNodeSprites[spriteIndex], litNodeSprites[spriteIndex]);
                Nodes.Add(node);
                node.index = nodeIndex;
                node.X = x;
                node.Y = y;
                grid[x, y] = node;
                nodeIndex++;
            }
        }
    }

    public void ResetBoard()
    {
        foreach (var node in Nodes)
        {
            node.ResetNode();
        }
    }


    //void AssignNeighbors()
    //{
    //    for (int x = 0; x < gridSize; x++)
    //    {
    //        for (int y = 0; y < gridSize; y++)
    //        {
    //            GlyphNode node = grid[x, y];

    //                // Up
    //                if (y + 1 < gridSize) node.neighbors.Add(grid[x, y + 1]);
    //                // Down
    //                if (y - 1 >= 0) node.neighbors.Add(grid[x, y - 1]);
    //                // Right
    //                if (x + 1 < gridSize) node.neighbors.Add(grid[x + 1, y]);
    //                // Left
    //                if (x - 1 >= 0) node.neighbors.Add(grid[x - 1, y]);
    //        }
    //    }
    //    Debug.Log("Neighbor nodes set");
    //}

    public bool[] GetNodePattern()
    {
        bool[] pattern = new bool[Nodes.Count];
        for (int i = 0; i < Nodes.Count; i++)
            pattern[i] = Nodes[i].IsActivated;

        return pattern;
    }
}
