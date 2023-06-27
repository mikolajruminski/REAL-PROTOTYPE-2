using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float horizontal;
    [SerializeField] float speed = 12f;
    float originalSpeed = 12f;
    float crouchSpeedModifier = 6f;
    [SerializeField] float jumpingPower = 16f;
    private bool isFacingRight = true;
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] private Transform groundCheck;
    [SerializeField] Transform overheadCheckColldier;
    [SerializeField] private LayerMask groundLayer;
    Animator animator;

    //Crouching

    [SerializeField] Collider2D standingCollider;
    public bool isCrouching = false;

    //double jump coyote time
    private bool doubleJump;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    //dashing
    bool isDashing;
    bool longJump = false;
    bool canDash = true;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingDuration = 0.2f;
    private float dashingCooldown = 2f;
    [SerializeField] private TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        animator.SetFloat("JumpingVelocity", playerRB.velocity.y);
        horizontal = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Horizontal input", horizontal);
        animator.SetBool("isCrawling", isCrouching);
        isMoving();
        Flip();
        Jump();
        Crouch();

        if (canDash && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }

        if (isCrouching)
        {
            standingCollider.enabled = false;
            speed = crouchSpeedModifier;
        }
        else
        {
            standingCollider.enabled = true;
            speed = originalSpeed;
        }

        if (isCrouching && horizontal != 0) 
        {
            animator.SetBool("isMovingCrouching", true);
            animator.SetBool("isCrawling", !isCrouching);
        }
        else 
        {
            animator.SetBool("isMovingCrouching", false);
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
        if (isGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            doubleJump = true;
            animator.SetBool("isJumping", false);
            longJump = false;

        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            animator.SetBool("isJumping", true);
            animator.SetBool("isCrawling", false);
        }

        if (Input.GetKey(KeyCode.Space) && longJump == false)
        {
            animator.SetBool("isJumping", true);
            longJump = true;
        }

        if (Input.GetKey(KeyCode.Space) && longJump == true && isGrounded())
        {
            animator.SetBool("isJumping", false);
            longJump = false;
        }


        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0f)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpingPower);
            animator.SetBool("isJumping", true);
        }

        if (Input.GetButtonDown("Jump") && !isGrounded() && doubleJump)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpingPower);
            doubleJump = false;
            animator.SetBool("isJumping", true);
        }


        if (Input.GetButtonUp("Jump") && playerRB.velocity.y > 0f)

        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpingPower * 0.5f);
            coyoteTimeCounter = 0f;
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

    void isMoving()
    {
        bool isMoving = Input.GetButton("Horizontal");
        if (animator.GetBool("IsMoving") != isMoving)
        {
            animator.SetBool("IsMoving", isMoving);
        }
    }

    private void Crouch()
    {
        if (isGrounded())
        {
            if (Input.GetKey(KeyCode.S) || Physics2D.OverlapCircle(overheadCheckColldier.position, 0.2f, groundLayer))
            {
                isCrouching = true;
            }
            else if (!Input.GetKeyUp(KeyCode.S) && !Physics2D.OverlapCircle(overheadCheckColldier.position, 0.2f, groundLayer))

                isCrouching = false;
        }

    }

}

