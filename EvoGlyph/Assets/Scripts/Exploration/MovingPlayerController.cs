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
	bool canMove = true;

    [SerializeField] float moveSpeed = 5f;

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

    private void OnEnable()
    {
        moveAction.Enable();

    }

    private void OnDisable()
    {
        moveAction.Disable();
        DialogueManager.Instance.OnDialogueInProgress -= HandlePlayerInConversation;
    }
    private void Start()
    {
        DialogueManager.Instance.OnDialogueInProgress += HandlePlayerInConversation;
    }
    void HandlePlayerInConversation(bool state)
    {
        Debug.Log("Movement state: " + state);
        canMove = !state;
    }
    public void SetPlayerCanMove(bool state)
    {
        canMove = state;
    }

    void Update()
    {
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
