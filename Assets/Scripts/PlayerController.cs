using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float horizontal;
    [SerializeField] float speed = 12f;
    [SerializeField] float jumpingPower = 16f;
    private bool isFacingRight = true;
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private bool doubleJump;

    //dashing
    bool isDashing;
    bool canDash = true;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingDuration = 0.2f;
    private float dashingCooldown = 2f;
    [SerializeField] private TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        Flip();
        Jump();

        if (canDash && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) 
        {
            return;
        }
        playerRB.velocity = new Vector2(horizontal * speed, playerRB.velocity.y);
    }

    void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void Jump()
    {
        if (isGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded() || doubleJump)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpingPower);

                doubleJump = !doubleJump;
            }

        }

        if (Input.GetButtonUp("Jump") && playerRB.velocity.y > 0f)

        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpingPower * 0.5f);
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        float gravityScale = playerRB.gravityScale;
        playerRB.gravityScale = 0f;

        tr.emitting = true;
        playerRB.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);

        yield return new WaitForSeconds(dashingDuration);
        tr.emitting = false;
        playerRB.gravityScale = gravityScale;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;



    }
}
