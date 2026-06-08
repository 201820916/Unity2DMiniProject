using UnityEngine;

public class Character_move : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 3f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private float moveInput;
    private bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!canMove)
        {
            moveInput = 0f;
            anim.SetBool("isMoving", false);
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0) // 오른쪽
        {
            sprite.flipX = false;
        }
        else if (moveInput < 0) // 왼쪽
        {
            sprite.flipX = true;
        }

        anim.SetBool("isMoving", moveInput != 0);

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
        }
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
        moveInput = 0f;

        if (anim != null)
        {
            anim.SetBool("isMoving", false);
        }

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }
}
