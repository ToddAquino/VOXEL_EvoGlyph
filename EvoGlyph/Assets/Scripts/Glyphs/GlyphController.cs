using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore;
public class GlyphController : MonoBehaviour
{

    [Header("Glyph Library")]
    [SerializeField] Glyph[] existingGlyphs;

    [SerializeField] GameObject m_GlyphBoardObj;
    [SerializeField] Camera _cam;

    [Header("Player Inputs")]
    [SerializeField] private PlayerUnit player;
    private PlayerInput m_GlyphInput;
    private InputAction m_DrawAction;

    [SerializeField] private Pattern pattern;
    List<GlyphNode> ActiveNodes = new List<GlyphNode>();
    List<int> Sequence = new List<int>();
    void OnStartGlyphMechanic()
    {
        //m_GlyphBoardObj.SetActive(true);
        GlyphBoard.Instance.GenerateField();
    }

    private void Awake()
    {
        m_GlyphInput = GetComponent<PlayerInput>();

        m_DrawAction = m_GlyphInput.actions.FindAction("Draw");
    }

    private void OnEnable()
    {
        m_DrawAction?.Enable();
    }

    private void OnDisable()
    {
        m_DrawAction?.Disable();
    }
    private void Start()
    {
        OnStartGlyphMechanic();
        pattern.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (!GlyphBoard.Instance.CanInteract)
            return;

        if (m_DrawAction.WasPressedThisFrame())
        {
            pattern.gameObject.SetActive(true);
        }

        if (m_DrawAction.IsPressed())
        {
            HandlePatternDraw();
        }

        if (m_DrawAction.WasReleasedThisFrame())
        {
            ComparePattern();
            ResetPattern();
            player.OnEndTurn();
        }      
    }

    void HandlePatternDraw()
    {
        RaycastHit2D hit = Physics2D.Raycast(_cam.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
        if (hit.collider != null)
        {
            GlyphNode nodeSelected = hit.collider.GetComponent<GlyphNode>();
            if (!nodeSelected)
                return;

            //First Node
            if (ActiveNodes.Count == 0)
            {
                if (!nodeSelected.IsActivated)
                {
                    nodeSelected.SetNodeActive();
                    ActiveNodes.Add(nodeSelected);
                    Sequence.Add(nodeSelected.index);
                    pattern.SnapToPosition(nodeSelected.transform.position);
                }
                return;
            }

            //Add new Node
            if (!nodeSelected.IsActivated && !ActiveNodes.Contains(nodeSelected))
            {
                //Check if it binds from the current end point node
                if (ActiveNodes[ActiveNodes.Count - 1].neighbors.Contains(nodeSelected))
                {
                    nodeSelected.SetNodeActive();
                    ActiveNodes.Add(nodeSelected);
                    Sequence.Add(nodeSelected.index);
                    pattern.SnapToPosition(nodeSelected.transform.position);

                    Debug.Log($"Sequence: {string.Join(",", ActiveNodes)}");
                }

                //Can do any pattern as long as not repeating same nodes
                //nodeSelected.SetNodeActive();
                //Sequence.Add(nodeSelected);
                //pattern.SnapToPosition(nodeSelected.transform.position);

                //Debug.Log($"Sequence: {string.Join(",", Sequence)}");
            }

            //Undo last Node
            else if (ActiveNodes.Count > 1 && ActiveNodes[ActiveNodes.Count - 2] == nodeSelected)
            {
                ActiveNodes[ActiveNodes.Count - 1].SetNodeInactive(); //Undo Last Node
                ActiveNodes.Remove(ActiveNodes[ActiveNodes.Count - 1]);
                Sequence.Remove(Sequence[Sequence.Count - 1]);
                pattern.UndoLastVertex();
            }
        }
            
    }

    void ResetPattern()
    {
        foreach (var node in ActiveNodes)
        {
            node.SetNodeInactive();
        }
        ActiveNodes.Clear();
        Sequence.Clear();
        pattern.ResetPattern();
        pattern.gameObject.SetActive(false);
        Debug.Log($"Reset Sequence: {string.Join(",", ActiveNodes)}");
    }

    void ComparePattern()
    {
        foreach (var glyphs in existingGlyphs)
        {
            foreach (var sequences in glyphs.GlyphData.PatternPossibilities)
            {
                if (Sequence.SequenceEqual(sequences.GlyphPattern))
                {
                    Debug.Log($"<color=yellow>Match Found: {glyphs} was formed</color>");
                    return;
                }
            }
        }       
    }
}
