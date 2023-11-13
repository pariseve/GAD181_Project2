using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class WolfManager : MonoBehaviour
{
    public bool isDead = false;
    public float respawnTimer = 0f;
    GameObject player;
    Transform spriteTransform;

    NavMeshAgent agent;
    AudioSource audioSource; // New variable for AudioSource

    [SerializeField] LayerMask groundLayer, playerLayer;
    [SerializeField] AudioClip detectionSound; // New variable for detection sound

    Vector3 destPoint;
    bool walkPointSet;
    [SerializeField] float range;

    [SerializeField] float sightRange, attackRange;
    bool playerInSight, playerInAttackRange;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        spriteTransform = transform.GetChild(0);

        RespawnWolf();

        // Add the AudioSource component to the GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = detectionSound; // Assign the detection sound
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
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            respawnTimer += Time.deltaTime;

            if (respawnTimer >= GetRespawnTime())
            {
                RespawnWolf();
            }
        }

        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSight && !playerInAttackRange) Patrol();
        if (playerInSight && !playerInAttackRange) Chase();

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

    private Vector3 GetInitialSpawnPosition()
    {
        // Example implementation: Randomize the initial spawn position within a specified range
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, 0f, z);
    }

    private void RespawnWolf()
    {
        // Reset position and other attributes
        transform.position = GetInitialSpawnPosition();

        // Additional respawn logic goes here

        isDead = false;
        respawnTimer = 0f;
    }

    private float GetRespawnTime()
    {
        // Example implementation: Return a fixed respawn time (adjust as needed)
        return 30f;
    }

    private void Chase()
    {
        agent.SetDestination(player.transform.position);

        // Play the detection sound if not already playing
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
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
