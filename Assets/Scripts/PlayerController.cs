using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed
    public LayerMask pathLayer; // Layer for paths the player can move through
    public float rotationSpeed = 10f; // Speed of rotation

    private Vector3 moveDirection;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; // Ensure Rigidbody works with physics
        rb.useGravity = false;  // Disable gravity for free movement
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent unwanted rotations
    }

    void Update()
    {
        HandleMovementInput();
    }

    void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    void HandleMovementInput()
    {
        // Capture 8-directional movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, 0, vertical).normalized;
    }

    void MovePlayer()
    {
        // Check if the player is moving along a valid path
        RaycastHit hit;
        Vector3 targetPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

        // Perform a raycast to ensure the target position is on a valid path
        if (Physics.Raycast(targetPosition + Vector3.up * 0.5f, Vector3.down, out hit, 1f, pathLayer))
        {
            rb.MovePosition(targetPosition);
        }
    }

    void RotatePlayer()
    {
        // Rotate the player to face the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
        }
    }
}