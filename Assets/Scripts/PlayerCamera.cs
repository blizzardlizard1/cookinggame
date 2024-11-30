using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset = new Vector3(0f, 10f, 0f); // Offset position from the player
    public Vector3 lookDirection = Vector3.down; // Direction the camera should face (downward)

    private void LateUpdate()
    {
        if (player != null)
        {
            // Set the camera's position relative to the player's position plus the offset
            transform.position = player.position + offset;

            // Fix the camera's rotation to always look in the defined direction
            transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }
    }
}
