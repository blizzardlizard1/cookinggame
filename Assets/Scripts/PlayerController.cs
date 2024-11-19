using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Animator animator;
    private Vector3 movement;

    void Start()
    {
        // Get references to the Rigidbody and Animator components
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input for movement (WASD or Arrow Keys)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Combine input into a movement vector
        movement = new Vector3(moveX, 0f, moveZ).normalized;

        // Set animator parameters for movement
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveZ", moveZ);
        animator.SetBool("IsMoving", movement.magnitude > 0);
    }

    void FixedUpdate()
    {
        // Move the player
        rb.velocity = movement * moveSpeed;

        // Rotate the player to face movement direction
        if (movement.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Log collisions (optional)
        Debug.Log($"Collided with: {collision.gameObject.name}");
    }
}
