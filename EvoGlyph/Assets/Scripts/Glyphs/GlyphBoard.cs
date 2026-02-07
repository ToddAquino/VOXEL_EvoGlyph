using System.Collections.Generic;
using UnityEngine;

public class GlyphBoard : MonoBehaviour
{
    public static GlyphBoard Instance;

    int gridSize = 4;
    const float _spacing = 1.25f;
    GlyphNode[,] grid;
    public List<GlyphNode> Nodes = new List<GlyphNode>();
    [SerializeField] private GameObject m_GlyphNodesObj;
    [SerializeField] private Transform m_GridTransform;

    private void Awake()
    {
        Instance = this;
        m_GlyphNodesObj.SetActive(false);
    }

    public void GenerateField()
    {     
        CreateNodes();
        AssignNeighbors();
    }

    void CreateNodes()
    {
        int nodeIndex = 1;
        grid = new GlyphNode[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                GameObject _n = Instantiate(m_GlyphNodesObj, m_GridTransform, true);
                _n.SetActive(true);
                _n.name = $"Node: {nodeIndex}";
                _n.transform.localPosition = new Vector2(_spacing * x, _spacing * y);

                GlyphNode node = _n.GetComponent<GlyphNode>();
                Nodes.Add(node);
                node.index = nodeIndex;
                node.X = x;
                node.Y = y;
                grid[x, y] = node;
                nodeIndex++;
            }
        }
    }

    void AssignNeighbors()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                GlyphNode node = grid[x, y];

                    // Up
                    if (y + 1 < gridSize) node.neighbors.Add(grid[x, y + 1]);
                    // Down
                    if (y - 1 >= 0) node.neighbors.Add(grid[x, y - 1]);
                    // Right
                    if (x + 1 < gridSize) node.neighbors.Add(grid[x + 1, y]);
                    // Left
                    if (x - 1 >= 0) node.neighbors.Add(grid[x - 1, y]);
            }
        }
        Debug.Log("Neighbor nodes set");
    }
}
