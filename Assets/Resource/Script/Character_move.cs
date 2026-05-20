using UnityEngine;

public class Character_move : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 3f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private float moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}