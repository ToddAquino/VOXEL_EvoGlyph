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
    }
    void Update()
    {
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
        Vector2 targetPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
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
