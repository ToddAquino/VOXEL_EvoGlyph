using UnityEngine;
using UnityEngine.InputSystem;

public class MovingPlayerController : MonoBehaviour
{
    InputAction moveAction = new InputAction();
    [SerializeField] SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Vector2 moveInput;

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
    }

    private void FixedUpdate()
    {
        Vector2 targetPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }
}
