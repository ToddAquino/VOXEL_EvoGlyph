using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class GlyphController : MonoBehaviour
{
    public static event Action<bool[]> OnCreateGlyph;

    [SerializeField] GameObject m_GlyphBoardObj;
    [SerializeField] Camera _cam;

    [Header("Player Inputs")]
    private PlayerInput m_GlyphInput;
    private InputAction m_DrawAction;
    public bool CanInteract;
    bool isDrawing;

    [Header("Glyph Pattern")]
    [SerializeField] private Pattern InputPattern;
    [SerializeField] private Pattern FeedbackPattern;
    [SerializeField] private float feedbackDuration;
    [SerializeField]  List<GlyphNode> ActiveNodes = new List<GlyphNode>();
    //List<int> Sequence = new List<int>();
    Coroutine FeedbackCoroutine;
    bool showIncorrectFeedbackPattern = true;
    [Header("Glyph Sounds")]
    [SerializeField] float glyphSoundPitch = 0.7f;
    float originalGlyphSoundPitch = 0.7f;
    float addGlyphSoundPitch = 0.1f;

    private void Awake()
    {
        m_GlyphInput = GetComponent<PlayerInput>();

        m_DrawAction = m_GlyphInput.actions.FindAction("Draw");
    }

    public void CanDrawGlyph(bool state)
    {
        CanInteract = state;
    }
    public void ShowIncorrectPattern(bool state)
    {
        showIncorrectFeedbackPattern = state;
    }
    private void Start()
    {
        //results in broken 1st turn if player goes first, fix later
        //CanInteract = false;
    }
    public void Initialize()
    {
        InputPattern.gameObject.SetActive(false);
    }
    public void GlyphControllerOnEndTurn()
    {
        ResetPattern();
        CanDrawGlyph(false);
        glyphSoundPitch = originalGlyphSoundPitch;
    }

    private void OnEnable()
    {
        m_DrawAction?.Enable();
    }

    private void OnDisable()
    {
        m_DrawAction?.Disable();
    }
    private void Update()
    {
        if (!CanInteract || SettingsMenu.IsPaused)
        {
            return;
        }


        if (m_DrawAction.WasPressedThisFrame())
        {
            ResetFeedback();
            InputPattern.gameObject.SetActive(true);
        }

        if (m_DrawAction.IsPressed())
        {
            if (!isDrawing)
            {
                isDrawing = true;
            }
            HandlePatternDraw();
        }
        else
        {
            if (isDrawing)
            {
                isDrawing = false;
                //OnCreateGlyph?.Invoke(Sequence);
                //ResetPattern();
                glyphSoundPitch = originalGlyphSoundPitch;
                var pattern = GlyphBoard.Instance.GetNodePattern();
                OnCreateGlyph?.Invoke(pattern);
                Glyph glyph = GameManager.Instance.GlyphDatabase.GetGlyphFromPattern(pattern);

                // Check if valid pattern is unlocked
                if (glyph != null && GameManager.Instance.PlayerData.IsUnlocked(glyph))
                {
                    ActiveNodes.Clear();
                    ResetPattern();
                }
                // Chcck if pattern is valid = success
                //if (GameManager.Instance.GlyphDatabase.TryGetValidGlyphFromPattern(pattern))
                //{
                //    ResetPattern();
                //}
                else
                {
                    if (showIncorrectFeedbackPattern)
                        ShowIncorrectPatternFeedback();

                    if (ActiveNodes.Count > 0)
                    {
                        AudioManager.Instance.PlaySFX("spellFailed");
                    }
                    ResetPattern();
                }
            }
        }
    }

    void HandlePatternDraw()
    {
        if (!isDrawing) return;
        Vector2 currentWorldPos = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        
        Vector2 startPoint = ActiveNodes.Count > 0 ? (Vector2)ActiveNodes[ActiveNodes.Count - 1].transform.position: currentWorldPos;
        RaycastHit2D[] hits = Physics2D.LinecastAll(startPoint, currentWorldPos);
        foreach (var hit in hits)
        {
            GlyphNode node = hit.collider.GetComponent<GlyphNode>();
            if (node != null)
            {
                ProcessNodeSelection(node);
            }
        }
    }

    void ProcessNodeSelection(GlyphNode nodeSelected)
    {
        if (!nodeSelected.IsActivated && !ActiveNodes.Contains(nodeSelected))
        {
            ActivateNode(nodeSelected);
        }
    }

    void ActivateNode(GlyphNode nodeSelected)
    {
        nodeSelected.SetNodeActive();
        ActiveNodes.Add(nodeSelected);
        InputPattern.SnapToPosition(nodeSelected.transform.position);
        AudioManager.Instance.PlaySFXWithPitch("glyphActivate", glyphSoundPitch);
        glyphSoundPitch += addGlyphSoundPitch;
        Debug.Log($"Sequence: {string.Join(",", ActiveNodes)}");
    }
    void ResetPattern()
    {
        GlyphBoard.Instance.ResetBoard();
        ActiveNodes.Clear();
        InputPattern.ResetVertexCount();
        InputPattern.gameObject.SetActive(false);
        Debug.Log($"Reset Sequence: {string.Join(",", ActiveNodes)}");
    }

    public void ShowPatternHint(Glyph glyph)
    {
        //if (FeedbackCoroutine != null) StopCoroutine(FeedbackCoroutine);
        //FeedbackCoroutine = StartCoroutine(DoShowPatternHint(glyph));
        InputPattern.ResetVertexCount();
        InputPattern.gameObject.SetActive(false);

        FeedbackPattern.gameObject.SetActive(true);
        FeedbackPattern.ResetVertexCount();
        FeedbackPattern.SetColor(Color.green);
        GlyphBoard.Instance.ResetBoard();

        int[] sequence = glyph.pattern.glyphSequence;
        List<(int index, int seq)> activeNodes = new List<(int, int)>();
        for (int i = 0; i < sequence.Length; i++)
        {
            if (sequence[i] > 0)
            {
                activeNodes.Add((i, sequence[i]));
            }
        }
        //sort sequence by ascending    
        activeNodes.Sort((a, b) => a.seq.CompareTo(b.seq));

        foreach (var (index, seq) in activeNodes)
        {
            var node = GlyphBoard.Instance.Nodes[index];
            node.SetNodeActive();
            FeedbackPattern.SnapToPosition(node.transform.position);
        }
    }

    public void HidePatternHint()
    {

        GlyphBoard.Instance.ResetBoard();
        FeedbackPattern.ResetVertexCount();
        FeedbackPattern.gameObject.SetActive(false);
    }

    public void ShowIncorrectPatternFeedback()
    {
        if (FeedbackCoroutine != null) StopCoroutine(FeedbackCoroutine);
        FeedbackCoroutine = StartCoroutine(DoShowIncorrectPatternFeedback());
    }
    IEnumerator DoShowIncorrectPatternFeedback()
    {
        InputPattern.gameObject.SetActive(false);
        FeedbackPattern.gameObject.SetActive(true);
        FeedbackPattern.ResetVertexCount();
        FeedbackPattern.SetColor(Color.red);
        var inputLR = InputPattern.GetComponent<LineRenderer>();
        var feedbackLR = FeedbackPattern.GetComponent<LineRenderer>();

        feedbackLR.positionCount = inputLR.positionCount;
        for (int i = 0; i < inputLR.positionCount; i++)
        {
            feedbackLR.SetPosition(i, inputLR.GetPosition(i));
        }

        yield return new WaitForSeconds(feedbackDuration);
        ResetFeedback();
        FeedbackCoroutine = null;
    }
    
    void ResetFeedback()
    {
        if (FeedbackCoroutine != null)
        {
            StopCoroutine(FeedbackCoroutine);
            FeedbackCoroutine = null;
        }
        FeedbackPattern.ResetVertexCount();
        FeedbackPattern.gameObject.SetActive(false);
    }
}
