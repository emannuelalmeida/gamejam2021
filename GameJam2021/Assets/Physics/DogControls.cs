using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogControls : MonoBehaviour
{
    enum State
    {
        Walking,
        Jumping,
        Sniffing,
        Crouching,
        Grabbing,
        Releasing
    }
    
    //Movement
    public float speed;
    public float currentSpeed;
    public float jump;
    float moveVelocity;
    Animator animator;
    SpriteRenderer render;

    //Grounded Vars
    bool isGrounded = true;
    bool isSniffing = false;
    bool isCrouching = false;

    private Rigidbody2D body;

    private State state = State.Walking;
    
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        render = GetComponentInChildren<SpriteRenderer>();
        currentSpeed = speed;
    }
    
    void FixedUpdate()
    {
        UpdateAction();
        UpdateMovement();
        UpdateAnimation();
    }

    //Check if Grounded
    void OnTriggerEnter2D()
    {
        isGrounded = true;
    }

    void OnCollisionEnter2D()
    {
        isGrounded = true;
    }

    private void UpdateMovement()
    {
        //Left Right Movement
        float x = Input.GetAxis("Horizontal");
        moveVelocity = x * currentSpeed;
        
        if (x > 0)
        {
            render.flipX = true;
        } else if (x < 0)
        {
            render.flipX = false;
        }

        body.velocity = new Vector2(moveVelocity, body.velocity.y);

    }

    private void UpdateAction()
    {
        if(CanJump() && JumpTriggered())
            StartJumping(); 
        else if (CanSniff() && SniffTriggered())
            StartSniffing();
        else if (CanCrouch() && CrouchTriggered())
            StartCrouching();
        else if (CanGrab() && GrabTriggered())
            StartGrabbing();
        else if (CanRelease() && ReleaseTriggered())
            StartReleasing();
        else if (CanIdle())
            StartIdling();
    }

    private bool CanJump()
    {
        return state == State.Walking;
    }

    private bool CanSniff()
    {
        return state == State.Walking;
    }

    private bool CanCrouch()
    {
        return state == State.Walking;
    }

    private bool CanGrab()
    {
        return state == State.Walking;
    }

    private bool CanRelease()
    {
        return state == State.Walking;
    }

    private bool CanIdle()
    {
        return (state == State.Jumping && isGrounded) 
            || (state == State.Sniffing && !SniffTriggered())
            || (state == State.Crouching && !CrouchTriggered());
    }

    private bool JumpTriggered()
    {
        return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z) ||
               Input.GetKey(KeyCode.W);
    }

    private bool SniffTriggered()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    private bool CrouchTriggered()
    {
        return Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
    }

    private bool GrabTriggered()
    {
        return false;
    }

    private bool ReleaseTriggered()
    {
        return false;
    }

    private void StartJumping()
    {
        state = State.Jumping;
        body.velocity = new Vector2(body.velocity.x, jump);
        isGrounded = false;
    }

    private void StartSniffing()
    {
        state = State.Sniffing;
        isSniffing = true;
        currentSpeed = speed /2;
    }

    private void StartCrouching()
    {
        state = State.Crouching;
        isCrouching = true;
        currentSpeed = speed /2;
    }

    private void StartGrabbing()
    {
        
    }

    private void StartReleasing()
    {
        
    }

    private void StartIdling()
    {
        state = State.Walking;
        currentSpeed = speed;
        isSniffing = false;
        isCrouching = false;
    }
    
    private void UpdateAnimation()
    {
        animator.SetFloat("speedY", body.velocity.y);
        animator.SetBool("isWalking", moveVelocity != 0);
        animator.SetBool("isJumping", !isGrounded);
        animator.SetBool("isSniffing", isSniffing);
        animator.SetBool("isCrouching", isCrouching);
    }
}
