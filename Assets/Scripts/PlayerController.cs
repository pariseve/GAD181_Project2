using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float groundDistance = 0.1f;
    public float smoothingFactor = 10f; // Adjust this value for smoother movement
    public LayerMask groundLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Use interpolation for smoother movement
    }

    void Update()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position + Vector3.up; // Slightly above the character
        if (Physics.Raycast(castPos, -Vector3.up, out hit, Mathf.Infinity, groundLayer))
        {
            float targetY = hit.point.y + groundDistance;
            Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);

            // Apply smoothing to character's position
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothingFactor);
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(x, 0, y);

        // Apply movement directly to the Rigidbody for smoother physics
        rb.velocity = moveDirection * speed;

        // Handle character orientation (flipping sprite)
        if (x != 0)
        {
            sr.flipX = (x > 0);
        }
    }
}
