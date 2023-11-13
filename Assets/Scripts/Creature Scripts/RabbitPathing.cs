using UnityEngine;
using UnityEngine.AI;

public class RabbitPathing : MonoBehaviour
{
    public bool isDead = false;
    public float respawnTimer = 0f;
    public NavMeshAgent rabbit;
    private SpriteRenderer sr;

    // Reference to the currently placed trap
    private GameObject placedTrap;

    // Rabbit carcass prefab
    public GameObject rabbitCarcassPrefab;

    void Start()
    {
        rabbit = GetComponent<NavMeshAgent>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rabbit.updateRotation = false; // disable NavMeshAgent's rotation

        RespawnRabbit();
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            respawnTimer += Time.deltaTime;

            if (respawnTimer >= GetRespawnTime())
            {
                RespawnRabbit();
            }
        }

        if (rabbit.remainingDistance <= rabbit.stoppingDistance)
        {
            // Check if a trap is placed
            if (placedTrap != null)
            {
                // Get the position of the rabbit before destruction
                Vector3 rabbitPosition = transform.position;

                // If a trap is placed, destroy the trap, rabbit, and instantiate rabbit carcass
                Destroy(placedTrap);
                Destroy(gameObject); // Destroy the rabbit

                // Instantiate rabbit carcass at the rabbit's original position with a specific Y-coordinate
                Instantiate(rabbitCarcassPrefab, new Vector3(rabbitPosition.x, 0f, rabbitPosition.z), Quaternion.identity);

                // Note: You might want to add additional logic or effects here

                // Reset placedTrap to null to avoid repeated destruction
                placedTrap = null;
            }
            else
            {
                // If no trap is placed, check if a trap prefab is in the scene with "SmallTrap" tag
                GameObject[] traps = GameObject.FindGameObjectsWithTag("SmallTrap");

                // If any trap prefab is found, move towards it
                if (traps.Length > 0)
                {
                    placedTrap = traps[Random.Range(0, traps.Length)]; // Assuming you want to choose a random trap if multiple are present
                    rabbit.SetDestination(placedTrap.transform.position);

                    // lock the rotation to face the z axis
                    transform.rotation = Quaternion.Euler(0, 0, 0);

                    // flip the sprite based on the direction of movement
                    Vector3 moveDirection = rabbit.destination - transform.position;
                    float x = moveDirection.x;
                    sr.flipX = (x > 0);
                }
                else
                {
                    // If no trap is placed and no trap prefab is found, move to a random point
                    MoveToRandomPoint();
                }
            }
        }
    }



    void MoveToRandomPoint()
    {
        Vector3 randomPoint = RandomNavMeshPoint();
        rabbit.SetDestination(randomPoint);

        // lock the rotation to face the z-axis
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // flip the sprite based on the direction of movement
        Vector3 moveDirection = rabbit.destination - transform.position;
        float x = moveDirection.x;
        sr.flipX = (x > 0);
    }

    Vector3 RandomNavMeshPoint()
    {
        Vector3 randomPoint = Vector3.zero;
        NavMeshHit hit;
        for (int i = 0; i < 30; i++) // find a valid point up to 30 times
        {
            randomPoint = transform.position + Random.insideUnitSphere * range;
            if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return randomPoint; // return the last generated point if no valid point was found
    }

    private Vector3 GetInitialSpawnPosition()
    {
        // Example implementation: Randomize the initial spawn position within a specified range
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, 0f, z);
    }

    private void RespawnRabbit()
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
    
    // Adjust the range variable according to your needs
    public float range = 5f;
}


