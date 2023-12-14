using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 5;
    public float gravity = -9.18f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float sprintDuration = 3f;

    private bool isSprinting = false;

    public float stamina = 100f;
    public float staminaDepletionRate = 10f;
    public float staminaRegenerationRate = 5f;



    void Update()
    {
        HandleMovement();
        HandleSprint();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetKey("left shift") && isGrounded)
        {
            speed = 10;
        }
        else
        {
            speed = 5;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        float moveSpeed = isSprinting ? sprintSpeed : walkSpeed;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveVector = moveDirection * moveSpeed * Time.deltaTime;

        transform.Translate(moveVector);

    if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpHeight;
            }
         moveDirection.y -= gravity * Time.deltaTime;

    }

    private void StartSprint()
    {
        if (!isSprinting)
        {
            isSprinting = true;
            StartCoroutine(SprintTimer());
        }
    }

    private void StopSprint()
    {
        if (isSprinting)
        {
            isSprinting = false;
        }
    }
    private IEnumerator SprintTimer()
    {
        yield return new WaitForSeconds(sprintDuration);
        StopSprint();
    }

     void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement.Normalize();

        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition + movement * (isSprinting ? sprintSpeed : speed) * Time.deltaTime;

        transform.position = newPosition;
    }

      void HandleSprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && stamina > 0)
        {
            isSprinting = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || stamina <= 0)
        {
            isSprinting = false;
        }

        if (isSprinting)
        {
            stamina -= staminaDepletionRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0f, 100f);
        }
        else  
        {
            stamina += staminaRegenerationRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0f, 100f);
        }
    }
}