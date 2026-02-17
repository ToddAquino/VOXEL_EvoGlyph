using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class GlyphController : MonoBehaviour
{
    //public UnityEvent OnTimerRanOut;
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

    //[Header("Glyph Drawing Timer")]
    //public bool isTimerEnabled;
    //[SerializeField] float timeLimit;
    //float timeRemaining;
    //public bool isTimerActive = false;
    //[SerializeField] SpriteRenderer timerProgress;

    [Header("Glyph Sounds")]
    [SerializeField] float glyphSoundPitch = 0.7f;
    float originalGlyphSoundPitch = 0.7f;
    float addGlyphSoundPitch = 0.1f;

    private void Awake()
    {
        m_GlyphInput = GetComponent<PlayerInput>();

        m_DrawAction = m_GlyphInput.actions.FindAction("Draw");
    }

    //public void UnlockGlyph(Glyph glyph)
    //{
    //    GameManager.Instance.PlayerGlyphs.UnlockGlyph(glyph);
    //}
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
        //timerProgress.material.SetFloat("_FillAmount", 1);
        //isTimerActive = false;
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
                if (GameManager.Instance.GlyphDatabase.TryGetValidGlyphFromPattern(pattern))
                {
                    ResetPattern();
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

        //if (isTimerEnabled && isTimerActive)
        //{
        //    timeRemaining -= Time.deltaTime;
        //    timeRemaining = Mathf.Max(timeRemaining, 0f);

        //    float fill = Mathf.Clamp01(timeRemaining / timeLimit);
        //    timerProgress.material.SetFloat("_FillAmount", fill);

        //    if (timeRemaining <= 0 && isTimerActive)
        //    {
        //        isTimerActive = false;
        //        ResetPattern();
        //        CanDrawGlyph(false);
        //        OnTimerRanOut?.Invoke();
        //        glyphSoundPitch = originalGlyphSoundPitch;
        //    }
        //}      
    }
    //public void SetTimerEnabled(bool state)
    //{
    //    isTimerEnabled = state;
    //}
    //void StartTimer()
    //{
    //    timeRemaining = timeLimit;
    //    float fill = Mathf.Clamp01(timeRemaining / timeLimit);
    //    timerProgress.material.SetFloat("_FillAmount", fill);
    //    isTimerActive = true;
    //}
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
        ////First Node
        //if (ActiveNodes.Count == 0)
        //{
        //    if (!nodeSelected.IsActivated)
        //    {
        //        //if (!isTimerActive)
        //        //{
        //        //    StartTimer();
        //        //}
        //        ActivateNode(nodeSelected);
        //    }
        //    return;
        //}

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
        //else if (ActiveNodes.Count > 1 && ActiveNodes[ActiveNodes.Count - 2] == nodeSelected)
        //{
        //    GlyphNode LastNode = ActiveNodes[ActiveNodes.Count - 1];
        //    LastNode.SetNodeInactive(); //Undo Last Node
        //    ActiveNodes.Remove(LastNode);
        //    //Sequence.Remove(Sequence[Sequence.Count - 1]);
        //    InputPattern.UndoLastVertex();
        //}
    }

    void ActivateNode(GlyphNode nodeSelected)
    {
        nodeSelected.SetNodeActive();
        ActiveNodes.Add(nodeSelected);
        //Sequence.Add(nodeSelected.index);
        InputPattern.SnapToPosition(nodeSelected.transform.position);
        AudioManager.Instance.PlaySFXWithPitch("glyphActivate", glyphSoundPitch);
        glyphSoundPitch += addGlyphSoundPitch;
        Debug.Log($"Sequence: {string.Join(",", ActiveNodes)}");
    }
    void ResetPattern()
    {
        //foreach (var node in ActiveNodes)
        //{
        //    node.SetNodeInactive();
        //}
        GlyphBoard.Instance.ResetBoard();
        ActiveNodes.Clear();
        //Sequence.Clear();
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

        int repeatCount = 2;
        for (int r = 0; r < repeatCount; r++)
        { 
            FeedbackPattern.gameObject.SetActive(true);
            FeedbackPattern.ResetVertexCount();
            FeedbackPattern.SetColor(Color.green);
            GlyphBoard.Instance.ResetBoard();

            int[] sequence = glyph.GlyphData.glyphSequence;
            List<(int index, int seq)> activeNodes = new List<(int, int)>();
            for (int i = 0; i < sequence.Length; i++)
            {
                if (sequence[i] > 0)
                {
                    activeNodes.Add((i,sequence[i]));
                }
            }
            //sort sequence by ascending    
            activeNodes.Sort((a,b) => a.seq.CompareTo(b.seq));

            foreach (var (index, seq) in activeNodes)
            {
                var node = GlyphBoard.Instance.Nodes[index];
                node.SetNodeActive();
                FeedbackPattern.SnapToPosition(node.transform.position);
            }


            yield return new WaitForSeconds(feedbackDuration);

            GlyphBoard.Instance.ResetBoard();
            FeedbackPattern.gameObject.SetActive(false);
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
