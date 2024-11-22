using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject handObject; // Reference to the player's hand object where the menu item will be placed
    public GameObject[] menuItems; // Array of menu items (Small, Medium, Large prefabs)

    private Rigidbody rb;
    private Animator animator;
    private Vector3 movement;
    private bool isMenuActive = false; // To track if the menu is currently displayed
    private string servingStationTag = "Serving Station"; // Tag or name to identify the serving station

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
        if (!isMenuActive)
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
        else
        {
            rb.velocity = Vector3.zero; // Stop player movement when the menu is active
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if collided with "Serving Station"
        if (collision.gameObject.name == servingStationTag || collision.gameObject.CompareTag(servingStationTag))
        {
            isMenuActive = true; // Activate the menu
        }
    }

    private void OnGUI()
    {
        if (isMenuActive)
        {
            // Display a simple GUI for Small, Medium, Large selection
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 75, 200, 150), "Select Size");

            if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 50, 150, 30), "Small"))
            {
                PlaceMenuItem(0); // Small
            }

            if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2, 150, 30), "Medium"))
            {
                PlaceMenuItem(1); // Medium
            }

            if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height / 2 + 50, 150, 30), "Large"))
            {
                PlaceMenuItem(2); // Large
            }
        }
    }

    private void PlaceMenuItem(int index)
    {
        if (index >= 0 && index < menuItems.Length)
        {
            // Instantiate the selected menu item and place it in the player's hand
            GameObject menuItem = Instantiate(menuItems[index], handObject.transform.position, Quaternion.identity);
            menuItem.transform.SetParent(handObject.transform); // Attach it to the player's hand
        }

        isMenuActive = false; // Deactivate the menu after selection
    }
}



