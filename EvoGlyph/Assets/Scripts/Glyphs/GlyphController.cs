using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class GlyphController : MonoBehaviour
{
    public UnityEvent OnTimerRanOut;
    public static event Action<List<int>> OnCreateGlyph;

    [SerializeField] GameObject m_GlyphBoardObj;
    [SerializeField] Camera _cam;

    [Header("Player Inputs")]
    private PlayerInput m_GlyphInput;
    private InputAction m_DrawAction;
    public bool CanInteract;

    [Header("Glyph Pattern")]
    [SerializeField] private Pattern InputPattern;
    [SerializeField] private Pattern FeedbackPattern;
    [SerializeField] private float feedbackDuration;
    List<GlyphNode> ActiveNodes = new List<GlyphNode>();
    List<int> Sequence = new List<int>();
    Coroutine FeedbackCoroutine;
    bool showIncorrectFeedbackPattern = true;

    [Header("Glyph Drawing Timer")]
    public bool isTimerEnabled;
    [SerializeField] float timeLimit;
    float timeRemaining;
    public bool isTimerActive = false;
    [SerializeField] Image timerProgress;

    private void Awake()
    {
        m_GlyphInput = GetComponent<PlayerInput>();

        m_DrawAction = m_GlyphInput.actions.FindAction("Draw");
    }

    public void UnlockGlyph(Glyph glyph)
    {
        GameManager.Instance.PlayerGlyphs.UnlockGlyph(glyph);
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
        CanInteract = false;
    }
    public void Initialize()
    {
        timerProgress.fillAmount = 1;
        isTimerActive = false;
        InputPattern.gameObject.SetActive(false);
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
        if (!CanInteract)
            return;
    

        if (m_DrawAction.WasPressedThisFrame())
        {
            ResetFeedback();
            InputPattern.gameObject.SetActive(true);
        }

        if (m_DrawAction.IsPressed())
        {
            HandlePatternDraw();
        }

        if (m_DrawAction.WasReleasedThisFrame())
        {
            //OnCreateGlyph?.Invoke(Sequence);
            //ResetPattern();
            OnCreateGlyph?.Invoke(Sequence);
            if (GameManager.Instance.GlyphDatabase.TryGetValidGlyphFromPattern(Sequence))
            {
                ResetPattern();
            }
            else
            {
                if(showIncorrectFeedbackPattern)
                    ShowIncorrectPatternFeedback();

                ResetPattern();
            }
        }
        if (isTimerEnabled && isTimerActive && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerProgress.fillAmount = timeRemaining/ timeLimit;
        }
        //Debug.Log($"<color=orange>Time Remaining: {timeRemaining} </color>");
        if (timeRemaining <= 0 && isTimerActive)
        {
            isTimerActive = false;
            ResetPattern();
            CanDrawGlyph(false);
            OnTimerRanOut?.Invoke();
        }
    }
    public void SetTimerEnabled(bool state)
    {
        isTimerEnabled = state;
    }
    void StartTimer()
    {
        timeRemaining = timeLimit;
        timerProgress.fillAmount = timeRemaining / timeLimit;
        isTimerActive = true;
    }
    void HandlePatternDraw()
    {
        Vector2 worldPos = _cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Collider2D hitCollider = Physics2D.OverlapPoint(worldPos);

        if (hitCollider == null) return;

            Debug.Log(hitCollider.name);
            GlyphNode nodeSelected = hitCollider.GetComponent<GlyphNode>();
        if (nodeSelected == null) return;
        
        ProcessNodeSelection(nodeSelected);
    }

    void ProcessNodeSelection(GlyphNode nodeSelected)
    {
        //First Node
        if (ActiveNodes.Count == 0)
        {
            if (!nodeSelected.IsActivated)
            {
                if (!isTimerActive)
                {
                    StartTimer();
                }
                ActivateNode(nodeSelected);
            }
            return;
        }
        GlyphNode LastNode = ActiveNodes[ActiveNodes.Count - 1];
        //Add new Node
        if (!nodeSelected.IsActivated && !ActiveNodes.Contains(nodeSelected))
        {
            //Strictly Vertical and Horizontal
            //Check if it binds from the current end point node
            //if (LastNode.neighbors.Contains(nodeSelected))
            //{
            //    ActivateNode(nodeSelected);
            //}
            //else
            //{
            //    foreach (var node in LastNode.neighbors)
            //    {
            //        if (nodeSelected.neighbors.Contains(node) && !ActiveNodes.Contains(node))
            //        {
            //            ActivateNode(node);
            //            ActivateNode(nodeSelected);
            //            break;
            //        }
            //    }
            //}

            //Free Pattern
            ActivateNode(nodeSelected);
        }

        //Undo last Node
        else if (ActiveNodes.Count > 1 && ActiveNodes[ActiveNodes.Count - 2] == nodeSelected)
        {
            LastNode.SetNodeInactive(); //Undo Last Node
            ActiveNodes.Remove(LastNode);
            Sequence.Remove(Sequence[Sequence.Count - 1]);
            InputPattern.UndoLastVertex();
        }
    }

    void ActivateNode(GlyphNode nodeSelected)
    {
        nodeSelected.SetNodeActive();
        ActiveNodes.Add(nodeSelected);
        Sequence.Add(nodeSelected.index);
        InputPattern.SnapToPosition(nodeSelected.transform.position);

        Debug.Log($"Sequence: {string.Join(",", ActiveNodes)}");
    }
    void ResetPattern()
    {
        foreach (var node in ActiveNodes)
        {
            node.SetNodeInactive();
        }
        ActiveNodes.Clear();
        Sequence.Clear();
        InputPattern.ResetVertexCount();
        InputPattern.gameObject.SetActive(false);
        Debug.Log($"Reset Sequence: {string.Join(",", ActiveNodes)}");
    }

    public void ShowPatternHint(Glyph glyph)
    {
        if (FeedbackCoroutine != null) StopCoroutine(FeedbackCoroutine);
        FeedbackCoroutine = StartCoroutine(DoShowPatternHint(glyph));
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

    IEnumerator DoShowPatternHint(Glyph glyph)
    {    
        InputPattern.ResetVertexCount();
        InputPattern.gameObject.SetActive(false);

        FeedbackPattern.gameObject.SetActive(true);
        FeedbackPattern.ResetVertexCount();
        FeedbackPattern.SetColor(Color.green);

        foreach (var index in glyph.GlyphData.PatternPossibilities[0].GlyphPattern)
        {
            GlyphBoard.Instance.Nodes[index - 1].SetNodeActive();
            FeedbackPattern.SnapToPosition(GlyphBoard.Instance.Nodes[index - 1].transform.position);
        }

        yield return new WaitForSeconds(feedbackDuration);
        foreach (var index in glyph.GlyphData.PatternPossibilities[0].GlyphPattern)
        {
            GlyphBoard.Instance.Nodes[index - 1].SetNodeInactive();
        }
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
