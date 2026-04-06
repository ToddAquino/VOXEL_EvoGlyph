using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManaGlyphController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GlyphBoard glyphBoard;
    ManaTowerMinigame minigame;
    [Header("Glyph Pattern")]
    [SerializeField] private Pattern InputPattern;
    [SerializeField] private Pattern FeedbackPattern;
    [SerializeField] private float feedbackDuration;
    [SerializeField] List<GlyphNode> ActiveNodes = new List<GlyphNode>();
    [SerializeField] private LayerMask glyphLayer;

    Coroutine FeedbackCoroutine;
    bool showIncorrectFeedbackPattern = true;

    [Header("Glyph Sounds")]
    [SerializeField] float glyphSoundPitch = 0.7f;
    float originalGlyphSoundPitch = 0.7f;
    float addGlyphSoundPitch = 0.1f;

    [Header("Player Inputs")]
    private PlayerInput m_GlyphInput;
    private InputAction m_DrawAction;
    public bool CanInteract;
    bool isDrawing;

    //public bool IsInTutorial;
    private void Awake()
    {
        m_GlyphInput = GetComponent<PlayerInput>();

        m_DrawAction = m_GlyphInput.actions.FindAction("Draw");
    }
    public void CanDrawGlyph(bool state)
    {
        CanInteract = state;
    }
    private void OnEnable()
    {
        m_DrawAction?.Enable();
    }

    private void OnDisable()
    {
        m_DrawAction?.Disable();
    }

    public void Initialize(ManaTowerMinigame Minigame)
    {
        ResetPattern();
        minigame = Minigame;
        InputPattern.gameObject.SetActive(false);
    }
    void Update()
    {
        if (!CanInteract || SettingsMenu.IsPaused)
        {
            return;
        }
        if(minigame.CurrentTryCount == minigame.MaxTries) return;

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
                glyphSoundPitch = originalGlyphSoundPitch;
                bool[] pattern = glyphBoard.GetNodePattern();
                int activeCount = 0;
                foreach (bool patternItem in pattern)
                {
                    if(patternItem == true)
                        activeCount++;
                }
                if (activeCount < 2)
                {
                    ResetPattern();
                    return;
                }

                // Handle minigame result
                bool success = false;

                if (minigame != null)
                {
                    success = minigame.GlyphCreated(pattern);
                    minigame.SetResult(success);
                }

                if (success)
                {
                    return;
                }
                
                Glyph glyph = GameManager.Instance.GlyphDatabase.GetGlyphFromPattern(pattern);
                if (glyph != null && GameManager.Instance.PlayerData.IsUnlocked(glyph))
                {
                    //ResetPattern();
                }
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
        Vector2 currentWorldPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2 startPoint = ActiveNodes.Count > 0 ? (Vector2)ActiveNodes[ActiveNodes.Count - 1].transform.position : currentWorldPos;
        RaycastHit2D[] hits = Physics2D.LinecastAll(startPoint, currentWorldPos, glyphLayer);
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
    
    public void OnEnd()
    {
        //ResetPattern();
        CanDrawGlyph(false);
        glyphSoundPitch = originalGlyphSoundPitch;
        //minigame.ExitMinigame();
    }

}
