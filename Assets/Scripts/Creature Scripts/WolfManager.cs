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
    AudioSource audioSource;

    [SerializeField] LayerMask groundLayer, playerLayer;
    [SerializeField] AudioClip detectionSound;

    Vector3 destPoint;
    bool walkPointSet;
    [SerializeField] float range;

    [SerializeField] float sightRange, attackRange;
    bool playerInSight, playerInAttackRange;

    [SerializeField] GameObject deadWolfPrefab;

    // Reference to the currently placed trap
    private GameObject placedTrap;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        spriteTransform = transform.GetChild(0);

        RespawnWolf();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = detectionSound;
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

            // Set the placedTrap reference to the current trap
            placedTrap = other.gameObject;

            // Destroy the wolf and trap
            DestroyWolf();
        }
    }

    private void DestroyWolf()
    {
        // Check if the wolf has already been destroyed
        if (isDead)
        {
            return;
        }

        // Get the position of the wolf before destruction
        Vector3 wolfPosition = transform.position;

        // Set isDead to true to prevent repeated destruction
        isDead = true;

        // Destroy the wolf
        Destroy(gameObject);

        // Instantiate the dead wolf at the wolf's original position
        Instantiate(deadWolfPrefab, new Vector3(wolfPosition.x, 0.1f, wolfPosition.z), Quaternion.identity);

        // Destroy the large trap
        if (placedTrap != null)
        {
            Destroy(placedTrap);
        }

        // Note: You might want to add additional logic or effects here
    }
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

        spriteTransform.LookAt(Camera.main.transform);
        spriteTransform.eulerAngles = new Vector3(0, spriteTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);

        Vector3 moveDirection = agent.destination - transform.position;
        float x = moveDirection.x;

        SpriteRenderer spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = (x > 0);
    }

    private Vector3 GetInitialSpawnPosition()
    {
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, 0f, z);
    }

    private void RespawnWolf()
    {
        transform.position = GetInitialSpawnPosition();
        isDead = false;
        respawnTimer = 0f;
    }

    private float GetRespawnTime()
    {
        return 30f;
    }

    private void Chase()
    {
        agent.SetDestination(player.transform.position);

        // Play the detection sound if not already playing
        AudioSource audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(detectionSound);
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
