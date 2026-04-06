using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingPlayerController : MonoBehaviour
{
    InputAction moveAction = new InputAction();
    [SerializeField] SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Vector2 moveInput;
    Coroutine footstepCoroutine;
    [SerializeField] float footstepInterval = 0.5f;
    bool canMove = true;
    bool canInteract = true;
    List<IInteractable> interactablesInRange = new List<IInteractable>();
    IInteractable currentInteractable;
    [SerializeField] float moveSpeed = 5f;
    bool hasTriggeredEncounter = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAction = new InputAction("move");

        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable newInteractable = collision.GetComponent<IInteractable>();
        if (newInteractable != null && !interactablesInRange.Contains(newInteractable))
        {
            interactablesInRange.Add(newInteractable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable existingInteractable = collision.GetComponent<IInteractable>();
        if (existingInteractable != null)
        {
            interactablesInRange.Remove(existingInteractable);

            if (currentInteractable == existingInteractable)
            {
                currentInteractable = null;
            }
        }
    }

    private void OnEnable()
    {
        moveAction.Enable();
        ExplorerUIHandler.OnBookOpen += StopPlayerControls;
        ExplorerUIHandler.OnBookClosed += ResumePlayerControls;

    }

    private void OnDisable()
    {
        moveAction.Disable();
        DialogueManager.Instance.OnDialogueInProgress -= HandlePlayerInConversation;
        ExplorerUIHandler.OnBookOpen -= StopPlayerControls;
        ExplorerUIHandler.OnBookClosed -= ResumePlayerControls;
    }
    private void Start()
    {
        DialogueManager.Instance.OnDialogueInProgress += HandlePlayerInConversation;
    }
    void HandlePlayerInConversation(bool state)
    {
        Debug.Log("Movement state: " + state);
        canMove = !state;
        if (!canMove)
        {
            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null;
            }
        }
    }

    void StopPlayerControls()
    {
        canInteract = false;
        canMove = false;
    }

    void ResumePlayerControls()
    {
        canInteract = true;
        canMove = true;
    }
    public void SetPlayerCanMove(bool state)
    {
        canMove = state;
        if (!canMove)
        {
            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null;
            }
        }
    }
    void Update()
    {
        UpdateCurrentInteractable();

        if (currentInteractable is EnemyEncounter && canInteract && !hasTriggeredEncounter)
        {
            hasTriggeredEncounter = true;
            currentInteractable.Interact(this);
            currentInteractable = null;
            return;
        }

        else if (currentInteractable != null && Keyboard.current.eKey.wasPressedThisFrame && canInteract)
        {
            currentInteractable.Interact(this);
        }

        if (!canMove) return;
        moveInput = moveAction.ReadValue<Vector2>();
        if (moveInput.x < 0)
        {
            spriteRenderer.flipX = true; //look left
        }
        else if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false; //look right
        }
        moveInput = moveInput.normalized;

        HandleFootsteps();
    }
    void UpdateCurrentInteractable()
    {
        float closestDistance = Mathf.Infinity;
        IInteractable closest = null;

        foreach (var interactable in interactablesInRange)
        {
            MonoBehaviour mb = interactable as MonoBehaviour;
            if (mb == null) continue;

            float dist = Vector2.Distance(transform.position, mb.transform.position);

            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = interactable;
            }
        }

        currentInteractable = closest;
    }

    void HandleFootsteps()
    {
        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        if (isMoving && footstepCoroutine == null)
        {
            footstepCoroutine = StartCoroutine(FootstepRoutine());
        }
        else if (!isMoving && footstepCoroutine != null)
        {
            StopCoroutine(footstepCoroutine);
            footstepCoroutine = null;
        }
    }
    private void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero; // stop movement when disabled
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;
    }
    private IEnumerator FootstepRoutine()
    {
        while (true)
        {
            AudioManager.Instance.PlayRandomPlayerFootstep(0.3f);
            yield return new WaitForSeconds(footstepInterval);
        }
    }
}
