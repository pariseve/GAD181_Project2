using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class WolfManager : MonoBehaviour
{
    GameObject player;
    Transform spriteTransform;

    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer, playerLayer, trapLayer; // Add trapLayer

    Vector3 destPoint;
    bool walkPointSet;
    [SerializeField] float range;

    [SerializeField] float sightRange, attackRange;
    bool playerInSight, playerInAttackRange;

    [SerializeField] GameObject deadWolfPrefab; // Reference to the dead wolf prefab

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        spriteTransform = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has died");

            // Clear the player's inventory
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
            if (inventoryManager != null)
            {
                inventoryManager.ResetInventory();
            }

            // Reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Check if the wolf entered the trap zone
        if (other.CompareTag("LargeTrap"))
        {
            Debug.Log("Wolf caught in trap");

            // Destroy the wolf and instantiate the dead wolf in its place
            DestroyWolf();
        }
    }

    private void DestroyWolf()
    {
        // Get the position of the wolf before destruction
        Vector3 wolfPosition = transform.position;

        // Destroy the wolf
        Destroy(gameObject);

        // Instantiate the dead wolf at the wolf's original position
        Instantiate(deadWolfPrefab, wolfPosition, Quaternion.identity);

        // Note: You might want to add additional logic or effects here
    }

    // Update is called once per frame
    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if(!playerInSight && !playerInAttackRange) Patrol();
        if(playerInSight && !playerInAttackRange) Chase();

        // Make the sprite face the camera
        spriteTransform.LookAt(Camera.main.transform);
        spriteTransform.eulerAngles = new Vector3(0, spriteTransform.eulerAngles.y, 0); // Constrain rotation to prevent spinning
                                                                                        // lock the rotation to face the z axis
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // flip the sprite based on the direction of movement
        Vector3 moveDirection = agent.destination - transform.position;
        float x = moveDirection.x;

        // Get the SpriteRenderer component of the child GameObject
        SpriteRenderer spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();

        // Flip the sprite
        spriteRenderer.flipX = (x > 0); // Change to '<' for the correct flip direction

    }

    private void Chase()
    {
        agent.SetDestination(player.transform.position);
    }

    void Patrol()
    {
        if (!walkPointSet) SearchForDest();
        if (walkPointSet) agent.SetDestination(destPoint);
        if (Vector3.Distance(transform.position, destPoint) < 10) walkPointSet = false;
    }

    void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkPointSet = true;
        }
    }
}

