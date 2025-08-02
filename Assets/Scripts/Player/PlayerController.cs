using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public CharacterSO characterData;
    public Room currentRoom;

    [Header("Player References")]
    [HideInInspector] public bool isInteracting = false;
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public Animator anim;
    public Vector2 moveDirection;
    [HideInInspector] public Vector2 lastMoveDirection;

    [Header("Player Dash")]
    bool isDashing = false;
    public float dashSpeed;
    public float startDashDuration = 0.2f;
    float dashDuration;
    public float startDashCooldown;
    float dashCooldown;

    // Player Items

    [Header("Player Items")]
    public Torch playerTorch;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isInteracting)
            {
                Dash();
            }
        }
        CalculateDash();
        CalculateDashCooldown();
    }
    void FixedUpdate()
    {
        HandleMovement();
    }
    void HandleMovement()
    {
        if (!isDashing && !isInteracting)
        {
            moveDirection.x = Input.GetAxisRaw("Horizontal");
            moveDirection.y = Input.GetAxisRaw("Vertical");

            if (moveDirection != Vector2.zero)
            {
                lastMoveDirection = moveDirection;
            }
            moveDirection.Normalize();

            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

            anim.SetFloat("moveX", moveDirection.x);
            anim.SetFloat("moveY", moveDirection.y);
            anim.SetFloat("lastMoveX", lastMoveDirection.x);
            anim.SetFloat("lastMoveY", lastMoveDirection.y);
            anim.SetBool("isWalking", moveDirection != Vector2.zero);
        }
    }
    void Dash()
    {
        if (dashCooldown <= 0f)
        {
            anim.SetTrigger("dash");
            dashDuration = startDashDuration;

            dashCooldown = startDashCooldown;
            isDashing = true;
        }
    }
    void CalculateDash()
    {
        if (dashDuration > 0)
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed) * dashSpeed;
            dashDuration -= Time.deltaTime;
            if (dashDuration <= 0)
            {
                isDashing = false;
                rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
            }
        }
    }
    void CalculateDashCooldown()
    {
        if (dashCooldown > 0f)
        {
            dashCooldown -= Time.deltaTime;
            if (dashCooldown < 0f)
            {
                dashCooldown = 0f;
            }
        }
    }

    public void ResetVelocity()
    {
        rb.velocity = Vector2.zero;
        anim.SetBool("isWalking", false);
    }
}
