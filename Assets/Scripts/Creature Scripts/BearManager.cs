using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class BearManager : MonoBehaviour
{
    public bool isDead = false;
    public float respawnTimer = 0f;
    GameObject player;
    Transform spriteTransform;

    NavMeshAgent bear;

    [SerializeField] LayerMask groundLayer, playerLayer;

    Vector3 destPoint;
    bool walkPointSet;
    [SerializeField] float range;

    // Added variables
    bool playerInSight, playerInAttackRange;
    [SerializeField] float sightRange, attackRange;

    private void Start()
    {
        bear = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        spriteTransform = transform.GetChild(0);

        RespawnBear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has died");

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
                RespawnBear();
            }
        }

        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSight && !playerInAttackRange) Patrol();
        if (playerInSight && !playerInAttackRange) Chase();

        // Make the sprite face the camera
        spriteTransform.LookAt(Camera.main.transform);
        spriteTransform.eulerAngles = new Vector3(0, spriteTransform.eulerAngles.y, 0);
        // flip the sprite based on the direction of movement
        Vector3 moveDirection = bear.destination - transform.position;
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

    private void RespawnBear()
    {
        // Reset position and other attributes
        transform.position = GetInitialSpawnPosition();

        // Additional respawn logic goes here

        isDead = false;
        respawnTimer = 0f;
    }

    private float GetRespawnTime()
    {
        //Return a fixed respawn time
        return 30f;
    }

    private void Chase()
    {
        bear.SetDestination(player.transform.position);
    }

    void Patrol()
    {
        if (!walkPointSet) SearchForDest();
        if (walkPointSet) bear.SetDestination(destPoint);
        if (Vector3.Distance(transform.position, destPoint) < 10) walkPointSet = false;
    }

    void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        destPoint = new Vector3(transform.position.x + x, transform.position.y);

        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkPointSet = true;
        }
    }
}
