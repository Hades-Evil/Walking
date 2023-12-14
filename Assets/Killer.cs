using System.Collections;
using UnityEngine;

public class Killer : MonoBehaviour
{
    public CharacterController controller;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float sprintDuration = 3f;

    public float gravity = -9.18f;
    public float jumpHeight = 3f;

    Vector3 velocity;
    bool isGrounded;

    private bool isSprinting = false;

    public KeyCode killButton = KeyCode.F; 
    public GameObject playerObject;
    private bool isPlayerKill = false;

    public float stamina = 100f;
    public float staminaDepletionRate = 10f;
    public float staminaRegenerationRate = 5f;


    private void Update()
    {
        HandleSprint();
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartSprint();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopSprint();
        }

      float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * walkSpeed * Time.deltaTime);

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

         if (Input.GetKeyDown(killButton) && !isPlayerKill)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);

            if (distanceToPlayer < 2f) 
            {
                KillPlayer();
            }
        }
        
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

    private void KillPlayer()
    {
        Debug.Log("Player Kill!");
        
        Destroy(playerObject);

        isPlayerKill = true;
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        movement.Normalize();

        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition + movement * (isSprinting ? sprintSpeed : walkSpeed) * Time.deltaTime;

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

        // Deplete stamina when sprinting
        if (isSprinting)
        {
            stamina -= staminaDepletionRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0f, 100f);
        }
        else  // Regenerate stamina when not sprinting
        {
            stamina += staminaRegenerationRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0f, 100f);
        }
    }



}
