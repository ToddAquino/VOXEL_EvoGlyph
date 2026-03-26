using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TomeBoard : MonoBehaviour
{
    public event Action OnFinished;
    public static TomeBoard Instance;
    [Header("Grid")]
    int gridSize = 4;
    public float spacing;

    [SerializeField] GameObject m_TomeNodesObj;
    [SerializeField] Transform m_GridTransform;
    [SerializeField] TomePointer pointer;

    Glyph requiredGlyph;
    [SerializeField] SpriteRenderer glyphSprite;

    [Header("Pattern")]
    [SerializeField] LineRenderer pathLineRenderer;
    List<TomeNode> nodes = new List<TomeNode>();
    List<TomeNode> path = new List<TomeNode>();
    int currentIndex = 0;
    bool isRunning = false;

    private void Awake()
    {
        Instance = this;
        m_TomeNodesObj.SetActive(false);
    }

    public void StartMinigame(Glyph glyph)
    {
        requiredGlyph = glyph;
        glyphSprite.sprite = requiredGlyph.GlyphIcon;
        GenerateField();
        BuildPath();
        SetupNodes();

        pointer.transform.position = path[0].transform.position;
        pointer.gameObject.SetActive(true);
        pointer.Initialize();
        currentIndex = 0;
        isRunning = true;
    }

    void Update()
    {
        if (!isRunning || currentIndex >= path.Count)
            return;
        TomeNode targetNode = path[currentIndex];
        pointer.MoveToTarget(targetNode.transform);

        // Check if pointer reached node
        if (Vector2.Distance(pointer.transform.position, targetNode.transform.position) < 0.5f)
        {
            if (targetNode.TryActivate())
            {
                currentIndex++;
                pointer.Boost();

                if (currentIndex >= path.Count)
                {
                    Debug.Log("Minigame Complete!");
                    isRunning = false;
                    OnFinished?.Invoke();
                }
            }
        }
    }
    public void GenerateField()
    {
        foreach (var n in nodes)
            Destroy(n.gameObject);

        nodes.Clear();

        int index = 0;

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                GameObject obj = Instantiate(m_TomeNodesObj, m_GridTransform);
                obj.name = $"TomeNode {index}";
                obj.SetActive(true);
                int flippedY = (gridSize - 1) - y;
                obj.transform.localPosition = new Vector2(x * spacing, flippedY * spacing);

                TomeNode node = obj.GetComponent<TomeNode>();
                node.index = index;

                nodes.Add(node);
                index++;
            }
        }
    }

    void BuildPath()
    {
        path.Clear();
        GlyphPattern pattern = requiredGlyph.pattern;
        for (int seq = 1; seq <= pattern.glyphSequence.Length; seq++)
        {
            for (int i = 0; i < pattern.glyphSequence.Length; i++)
            {
                if (pattern.glyphSequence[i] == seq)
                {
                    path.Add(nodes[i]);
                    break;
                }
            }
        }
        pathLineRenderer.positionCount = path.Count;

        for (int i = 0; i < path.Count; i++)
        {
            pathLineRenderer.SetPosition(i, path[i].transform.position);
        }
    }

    void SetupNodes()
    {
        GlyphPattern pattern = requiredGlyph.pattern;
        for (int i = 0; i < nodes.Count; i++)
        {
            int seq = pattern.glyphSequence[i];

            if (seq == 0)
            {
                nodes[i].SetState(TomeNodeState.Disabled);
            }
            else
            {
                nodes[i].SetState(TomeNodeState.Pending);
                // Assign random input (replace with your logic later)
                nodes[i].Initialize(GetRandomKey());
            }
        }
    }

    Key GetRandomKey()
    {
        Key[] keys = new Key[]
        {
            Key.W, Key.A, Key.S, Key.D
        };

        return keys[UnityEngine.Random.Range(0, keys.Length)];
    }
}
