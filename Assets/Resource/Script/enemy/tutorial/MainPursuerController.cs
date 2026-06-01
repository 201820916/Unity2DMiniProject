using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MainPursuerController : MonoBehaviour
{
    public static MainPursuerController Instance { get; private set; }

    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private string targetTag = "Player";

    [Header("Movement")]
    [SerializeField] private float baseSpeed = 2.5f;
    [SerializeField] private float pressureSpeedBonus = 0.35f;
    [SerializeField] private float stopDistance = 1.2f;
    [SerializeField] private bool followVertical = false;
    [SerializeField] private float verticalSpeed = 2f;

    [Header("Region Follow")]
    [SerializeField] private bool followPlayerTeleports = true;
    [SerializeField] private Vector2 teleportOffsetFromPlayer = new Vector2(-6f, 0f);
    [SerializeField] private float snapDistance = 35f;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private int pressureLevel;
    private bool isPaused;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("MainPursuerController already exists. Disabling duplicate pursuer.");
            enabled = false;
            return;
        }

        Instance = this;
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void Start()
    {
        FindTargetIfNeeded();
    }

    private void FixedUpdate()
    {
        if (isPaused)
        {
            StopMovement();
            return;
        }

        FindTargetIfNeeded();

        if (target == null)
        {
            StopMovement();
            return;
        }

        if (Vector2.Distance(transform.position, target.position) > snapDistance)
        {
            SnapNearTarget(target.position);
            return;
        }

        ChaseTarget();
    }

    private void FindTargetIfNeeded()
    {
        if (target != null) return;

        GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);

        if (targetObject != null)
        {
            target = targetObject.transform;
        }
    }

    private void ChaseTarget()
    {
        Vector2 offset = target.position - transform.position;
        float currentSpeed = baseSpeed + pressureLevel * pressureSpeedBonus;
        float xVelocity = Mathf.Abs(offset.x) > stopDistance ? Mathf.Sign(offset.x) * currentSpeed : 0f;
        float yVelocity = rb.linearVelocity.y;

        if (followVertical && Mathf.Abs(offset.y) > stopDistance)
        {
            yVelocity = Mathf.Sign(offset.y) * verticalSpeed;
        }

        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        UpdateVisual(xVelocity);
    }

    private void UpdateVisual(float xVelocity)
    {
        if (spriteRenderer != null && Mathf.Abs(xVelocity) > 0.01f)
        {
            spriteRenderer.flipX = xVelocity < 0f;
        }

        if (animator != null)
        {
            animator.SetBool("isMoving", Mathf.Abs(xVelocity) > 0.01f);
        }
    }

    private void StopMovement()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        if (animator != null)
        {
            animator.SetBool("isMoving", false);
        }
    }

    public void OnPlayerTeleported(Vector3 playerDestination)
    {
        if (!followPlayerTeleports) return;

        SnapNearTarget(playerDestination);
    }

    public void SetPressureLevel(int level)
    {
        pressureLevel = Mathf.Max(0, level);
    }

    public void AddPressure(int amount = 1)
    {
        SetPressureLevel(pressureLevel + amount);
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }

    private void SnapNearTarget(Vector3 playerPosition)
    {
        Vector3 snapPosition = playerPosition + (Vector3)teleportOffsetFromPlayer;
        transform.position = snapPosition;
        StopMovement();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag)) return;

        Debug.Log("Main pursuer caught the player.");
    }
}
