using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Movement
    public float moveSpeed;
    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector]
    public Vector2 lastMovedVector;

    public enum ControlType { oldInputManager, newInputSystem };
    public ControlType controlType = ControlType.oldInputManager;

    // Right-click movement (League of Legends style)
    private Vector3 targetPosition;
    private bool isMovingToTarget = false;
    private float stoppingDistance = 0.1f;

    // References
    Rigidbody2D rb;
    PlayerStats player;
    Camera mainCamera;

    void Start()
    {
        player = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        lastMovedVector = new Vector2(1, 0f);
        targetPosition = transform.position;
    }

    void Update()
    {
        if(controlType == ControlType.oldInputManager)
        {
            InputManagement();
        }

        // Handle right-click movement
        HandleRightClickMovement();
    }

    void FixedUpdate()
    {
        Move();
    }

    void InputManagement()
    {
        if(GameManager.instance.isGameOver)
        {
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;
        UpdateLastMovedVector();

        // Right-click to set target
        if (Input.GetMouseButtonDown(1)) // 1 = Right Mouse Button
        {
            SetTargetFromMouse();
        }

        // If moving to target via click, stop keyboard movement
        if (isMovingToTarget)
        {
            moveDir = Vector2.zero;
        }
    }

    void HandleRightClickMovement()
    {
        if (!isMovingToTarget)
            return;

        // Move towards target
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        moveDir = directionToTarget;

        // Check if reached target
        if (Vector3.Distance(transform.position, targetPosition) <= stoppingDistance)
        {
            isMovingToTarget = false;
            moveDir = Vector2.zero;
        }
    }

    void SetTargetFromMouse()
    {
        if (mainCamera == null)
            return;

        // Convert mouse position to world position
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; // Keep Z at 0 for 2D

        targetPosition = mouseWorldPos;
        isMovingToTarget = true;
    }

    void Move()
    {
        if (GameManager.instance.isGameOver)
        {
            return;
        }

        rb.linearVelocity = new Vector2(moveDir.x * player.CurrentMoveSpeed, moveDir.y * player.CurrentMoveSpeed);
    }

    public void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>().normalized;
        isMovingToTarget = false; // Cancel click movement if keyboard is used
        UpdateLastMovedVector();
    }

    void UpdateLastMovedVector()
    {
        lastMovedVector = moveDir != Vector2.zero ? moveDir : lastMovedVector;
    }
}
