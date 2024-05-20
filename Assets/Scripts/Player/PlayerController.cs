using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Crouch
    {
        STATE_0, STATE_1, STATE_2
    }

    public enum Movement
    {
        IDLE, WALK, RUN
    }

    private CharacterController character;
    private Vector3 moveDirection;

    [Header("Movement Values")]
    public float speed = 10.0f;
    public float jumpHeight = 100.0f;
    public float crouchOffset = 0f;
    public Crouch crouchState = Crouch.STATE_0;
    public Movement movement;
    public float maxVelocity = 7.0f;
    Vector3 velocity;

    [Header("Ground Check")]
    public float distance = 1.5f;
    public bool isGrounded = false;
    public float gravity = -9.81f;

    [Header("Vehicle")]
    public bool inCar;

    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    void Update()
    {
        //check if player is grounded
        isGrounded = false;
        if(Physics.Raycast(transform.position, -transform.up, character.height / 2 + 0.1f))
        {
            isGrounded = true;
        }

        if (!inCar)
        {
            HandleMovement();
            HandleGravity();
            HandleCrouch();
        }
    }

    //custom methods
    void HandleGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            crouchState = Crouch.STATE_0;
        }

        velocity.y += gravity * Time.deltaTime;
        character.Move(velocity * Time.deltaTime);
    }

    void HandleMovement()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        movement = Movement.IDLE;

        if ((verticalMovement > 0 || verticalMovement < 0 || horizontalMovement > 0 || horizontalMovement < 0) && !Input.GetKey(KeyCode.LeftShift))
        {
            movement = Movement.WALK;
        }else if ((verticalMovement > 0 || verticalMovement < 0 || horizontalMovement > 0 || horizontalMovement < 0) && Input.GetKey(KeyCode.LeftShift))
        {
            movement = Movement.RUN;
        }

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
        moveDirection.Normalize();

        switch(movement)
        {
            case Movement.WALK: speed = 3f; break;
            case Movement.IDLE: speed = 3f; break;
            case Movement.RUN: speed = 5f; break;
        }

        if (movement != Movement.IDLE)
        {
            character.Move(moveDirection * speed * Time.deltaTime);
        }
    }

    void HandleCrouch()
    {
        crouchOffset = 0f;

        //checking crouch state and update crouch offset
        switch (crouchState)
        {
            case Crouch.STATE_0: crouchOffset = 0f; break;
            case Crouch.STATE_1: crouchOffset = 0.5f; break;
            case Crouch.STATE_2: crouchOffset = 0.75f; break;
        }

        if (Input.GetKeyUp(KeyCode.C)) 
        {
            switch (crouchState)
            {
                case Crouch.STATE_0: crouchState = Crouch.STATE_1; break;
                case Crouch.STATE_1: crouchState = Crouch.STATE_2; break;
                case Crouch.STATE_2: crouchState = Crouch.STATE_0; break;
            }
        }
    }

    public bool isInCar()
    {
        return inCar;
    }
}
