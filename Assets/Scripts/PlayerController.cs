using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject handObject; // Reference to the player's hand object where the menu item will be placed
    public GameObject[] menuItems; // Array of menu items (Small, Medium, Large prefabs)
    public OrderingManager orderingManager; // Reference to the ordering system
    private Rigidbody rb;
    private Animator animator;
    private Vector3 movement;
    private bool isMenuActive = false; // To track if the menu is currently displayed
    private string servingStationTag = "Serving Station"; // Tag or name to identify the serving station
    private string npcTag = "NPC"; // Tag to identify NPCs
    private bool isHoldingFood = false; // Track if the player is holding food
    private bool hasExitedZone = true; // Track if the player has exited the serving zone
    private float menuCooldown = 1f; // Cooldown duration in seconds
    private float lastMenuTime = -1f; // Time the menu was last opened


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
            else
            {
                rb.angularVelocity = Vector3.zero; // Stop unintended rotation
            }
        }
        else
        {
            rb.velocity = Vector3.zero; // Stop player movement when the menu is active
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(servingStationTag) && hasExitedZone)
        {
            // Check cooldown
            if (Time.time > lastMenuTime + menuCooldown)
            {
                isMenuActive = true; // Activate the menu
                lastMenuTime = Time.time; // Update the last menu activation time
            }

            hasExitedZone = false; // Player has entered the zone
        }

        if (collision.gameObject.CompareTag(npcTag) && isHoldingFood)
        {
            DeliverOrder(collision.gameObject); // Deliver the order to the NPC
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(servingStationTag))
        {
            hasExitedZone = true; // Mark that the player has exited the serving station
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
            // Instantiate the selected menu item
            GameObject menuItem = Instantiate(menuItems[index], handObject.transform.position, Quaternion.identity);
    
            // Disable Rigidbody and set Collider to Trigger (if present)
            Rigidbody foodRigidbody = menuItem.GetComponent<Rigidbody>();
            if (foodRigidbody != null)
            {
                Destroy(foodRigidbody); // Remove the Rigidbody component
            }
    
            Collider foodCollider = menuItem.GetComponent<Collider>();
            if (foodCollider != null)
            {
                foodCollider.isTrigger = true; // Ensure the collider is a trigger
            }
    
            // Attach it to the player's hand
            menuItem.transform.SetParent(handObject.transform, true);
            isHoldingFood = true; // Mark that the player is holding food
        }
    
        isMenuActive = false; // Deactivate the menu after selection
    }


    private void DeliverOrder(GameObject npc)
    {
        // Notify the ordering system
        if (orderingManager != null)
        {
            orderingManager.isComplete = true; // Mark the order as complete
            orderingManager.CalculateTip(); // Calculate the tip
        }

        // Clear the player's hand
        foreach (Transform child in handObject.transform)
        {
            Destroy(child.gameObject);
        }

        isHoldingFood = false; // No longer holding food
        Debug.Log("Order delivered to NPC: " + npc.name);
    }
}



