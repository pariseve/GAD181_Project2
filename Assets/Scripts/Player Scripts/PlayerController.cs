using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float groundDistance = 0.1f;
    public float smoothingFactor = 10f;
    public LayerMask groundLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;
    public Animator animator;

    public DisplayInventory displayInventory;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        animator = GetComponent<Animator>();

        // Add MeshCollider if not added already
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
            // Configure MeshCollider as needed
        }
    }

    void Update()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position + Vector3.up;
        if (Physics.Raycast(castPos, -Vector3.up, out hit, Mathf.Infinity, groundLayer))
        {
            float targetY = hit.point.y + groundDistance;
            Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothingFactor);
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(x, 0, z);

        rb.velocity = moveDirection * speed;

        // Handle character orientation (flipping sprite)
        if (x != 0 || z != 0) // Check if moving in either x or z direction
        {
            // Set parameters for Animator
            bool isRunning = moveDirection.magnitude > 0; // Check if the player is moving
            animator.SetBool("IsRunning", isRunning);

            // Flip the character in the opposite direction
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x) * -Mathf.Sign(x); 
            localScale.z = Mathf.Abs(localScale.z) * -Mathf.Sign(z); 
            transform.localScale = localScale;
        }
        else
        {
            // If not moving, set IsRunning to false
            animator.SetBool("IsRunning", false);
        }

        //if (Input.GetMouseButtonDown(1)) // Right-click
        //{
        //    // Determine the drop position in the game world.
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit playerControllerHit; // Remove this line

        //    if (Physics.Raycast(ray, out playerControllerHit))
        //    {
        //        Vector3 dropPosition = playerControllerHit.point;

        //        // Call the DropSelectedItem method in the DisplayInventory script.
        //        displayInventory.DropSelectedItem(dropPosition);
        //    }
        //}
    }
}
