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
        Eating
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

    private bool isHoldingObject = false;
    private GameObject currentObject = null;
    private bool isShoeInRange = false;
    private GameObject shoeInRange = null;
    private int actionCooldown = 0;

    private Rigidbody2D body;
    private BoxCollider2D box;

    private State state = State.Walking;
    private PowerUp powerUp = PowerUp.None;
    private int powerUpTime = 0;
    private int jumpsAllowed = 1;
    private int jumps = 0;
    private bool jumpTriggerReleased = true;
    private int yMovementCheckCount = 0;
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        render = GetComponentInChildren<SpriteRenderer>();
        currentSpeed = speed;
    }
    
    void FixedUpdate()
    {
        UpdateAction();
        UpdateMovement();
        UpdateState();
        UpdateAnimation();
    }

    //Check if Grounded
    void OnTriggerEnter2D(Collider2D col)
    {
        shoeInRange = col.gameObject;
        isShoeInRange = true;
    }

    void OnTriggerExit2D()
    {
        shoeInRange = null;
        isShoeInRange = false;
    }

    void OnCollisionEnter2D()
    {
        yMovementCheckCount = 5;
    }

    private void UpdateMovement()
    {
        if (CanMove())
        {
            //Left Right Movement
            float x = Input.GetAxis("Horizontal");
            moveVelocity = x * currentSpeed;
            if(x != 0)
                render.flipX = x > 0;
        }
        else
        {
            moveVelocity = 0;
        }

        body.velocity = new Vector2(moveVelocity, body.velocity.y);
    }

    private void UpdateAction()
    {
        if (CanGround())
            StartGrounding();
        if (CanFall())
            StartFalling();
        if(CanJump() && JumpTriggered())
            StartJumping();
        else if (CanJumpInAir() && JumpTriggered())
            StartJumping();
        else if (CanSniff() && SniffTriggered())
            StartSniffing();
        else if (CanCrouch() && CrouchTriggered())
            StartCrouching();
        else if (CanGrab() && GrabTriggered())
            StartGrabbing();
        else if (CanRelease() && ReleaseTriggered())
            StartReleasing();
        else if (CanEat() && EatTriggered())
            StartEating();
        else if (CanIdle())
            StartIdling();
    }

    private void UpdateState()
    {
        if(actionCooldown > 0)
            actionCooldown--;
        switch (state)
        {
            case State.Jumping:
                jumpTriggerReleased = !JumpTriggered();
                break;
            case State.Eating:
                if (actionCooldown == 0)
                {
                    RevokeCurrentPowerUp();
                    ApplyPowerUp(ExtractPowerUp(currentObject));
                    isHoldingObject = false;
                    Object.Destroy(currentObject);
                    currentObject = null;
                }
                break;
        }

        if (powerUpTime > 0)
            powerUpTime--;
        if (powerUpTime == 0)
        {
            RevokeCurrentPowerUp();
        }
    }

    private bool CanGround()
    {
        return yMovementCheckCount > 0;
    }

    private bool CanFall()
    {
        return state != State.Jumping && body.velocity.y < -2f;
    }

    private bool CanJump()
    {
        return state == State.Walking;
    }

    private bool CanJumpInAir()
    {
        return state == State.Jumping && jumps < jumpsAllowed && jumpTriggerReleased;
    }

    private bool CanSniff()
    {
        return !isHoldingObject && state == State.Walking;
    }

    private bool CanCrouch()
    {
        return state == State.Walking;
    }

    private bool CanGrab()
    {
        return !isHoldingObject
               && isShoeInRange
               && actionCooldown == 0 
               && (state == State.Walking || state == State.Jumping || state == State.Crouching);
    }

    private bool CanRelease()
    {
        return isHoldingObject
               && actionCooldown == 0 
               && state == State.Walking;
    }

    private bool CanEat()
    {
        return isHoldingObject
               && actionCooldown == 0
               && state == State.Walking
               && IsEdible(currentObject);
    }

    private bool CanIdle()
    {
        return (state == State.Jumping && isGrounded) 
            || (state == State.Sniffing && !SniffTriggered())
            || (state == State.Crouching && !CrouchTriggered())
            || (state == State.Eating && actionCooldown == 0);
    }

    private bool CanMove()
    {
        return state != State.Eating;
    }

    private bool IsEdible(GameObject obj)
    {
        return obj.GetComponent<PowerUpComponent>() != null;
    }

    private void ApplyPowerUp(PowerUp newPowerUp)
    {
        powerUp = newPowerUp;
        switch (powerUp)
        {
            case PowerUp.DoubleJump:
                jumpsAllowed = 2;
                powerUpTime = -1;
                break;
            case PowerUp.DoubleSpeed:
                speed = speed * 2;
                currentSpeed = speed;
                powerUpTime = -1;
                break;
        }
    }

    private void RevokeCurrentPowerUp()
    {
        switch (powerUp)
        {
            case PowerUp.DoubleJump:
                jumpsAllowed = 1;
                break;
            case PowerUp.DoubleSpeed:
                speed = speed / 2;
                currentSpeed = speed;
                break;
        }
        powerUp = PowerUp.None;
    }

    private PowerUp ExtractPowerUp(GameObject obj)
    {
        if (obj == null)
            return PowerUp.None;
        var component = obj.GetComponent<PowerUpComponent>();
        if (component == null)
            return PowerUp.None;
        return component.powerUp;
    }

    public static bool JumpTriggered()
    {
        return Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z) ||
               Input.GetKey(KeyCode.W);
    }

    public static bool SniffTriggered()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    public static bool CrouchTriggered()
    {
        return Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
    }

    public static bool GrabTriggered()
    {
        return Input.GetKey(KeyCode.G);
    }

    public static bool ReleaseTriggered()
    {
        return Input.GetKey(KeyCode.G);
    }

    public static bool EatTriggered()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    private void StartGrounding()
    {
        yMovementCheckCount--;
        isGrounded = Mathf.Abs(body.velocity.y) <= 0.001f;
        if (isGrounded)
            yMovementCheckCount = 0;
    }
    
    private void StartFalling()
    {
        state = State.Jumping;
        jumps++;
        isGrounded = false;
    }

    private void StartJumping()
    {
        state = State.Jumping;
        jumps++;
        jumpTriggerReleased = false;
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
        isHoldingObject = true;
        shoeInRange.transform.SetParent(gameObject.transform);
        currentObject = shoeInRange;
        actionCooldown = 10;
    }

    private void StartReleasing()
    {
        isHoldingObject = false;
        currentObject.transform.SetParent(null);
        currentObject = null;
        actionCooldown = 10;
    }

    private void StartEating()
    {
        state = State.Eating;
        actionCooldown = 20;
    }

    private void StartIdling()
    {
        state = State.Walking;
        currentSpeed = speed;
        jumps = 0;
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
        // animator.SetBool("isEating", state == State.Eating);
    }
}
