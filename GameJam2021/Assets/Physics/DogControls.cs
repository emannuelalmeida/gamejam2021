using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogControls : MonoBehaviour
{
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
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        render = GetComponentInChildren<SpriteRenderer>();
        currentSpeed = speed;
    }
    
    void FixedUpdate()
    {
        UpdateJump(); 
        UpdateMovement();
        UpdateAction();
    }

    //Check if Grounded
    void OnTriggerEnter2D()
    {
        isGrounded = true;
    }

    void OnCollisionEnter2D()
    {
        isGrounded = true;
        animator.SetBool("isJumping", false);
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

        animator.SetFloat("speedY", body.velocity.y);

        if (moveVelocity != 0) animator.SetBool("isWalking", true);
        else animator.SetBool("isWalking", false);
    }

    private void UpdateJump()
    {
        //Jumping
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W)) 
        {
            if(isGrounded)
            {
                animator.SetBool("isJumping", true);
                body.velocity = new Vector2(body.velocity.x, jump);
                isGrounded = false;
                
            }
        }
    }

    private void UpdateAction()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSniffing = true;
            currentSpeed = speed /2;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSniffing = false;
            currentSpeed = speed;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            isCrouching = true;
            currentSpeed = speed /2;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            isCrouching = false;
            currentSpeed = speed;
        }

        animator.SetBool("isSniffing", isSniffing);
        animator.SetBool("isCrouching", isCrouching);
    }
}
