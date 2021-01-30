using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogControls : MonoBehaviour
{
    //Movement
    public float speed;
    public float jump;
    float moveVelocity;

    //Grounded Vars
    bool isGrounded = true;

    private Rigidbody2D body;
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        UpdateJump(); 
        UpdateMovement();
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
        moveVelocity = x * speed;
        body.velocity = new Vector2(moveVelocity, body.velocity.y);
    }

    private void UpdateJump()
    {
        //Jumping
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W)) 
        {
            if(isGrounded)
            {
                body.velocity = new Vector2(body.velocity.x, jump);
                isGrounded = false;
            }
        }
    }
}
