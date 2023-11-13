using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RabbitPathing : MonoBehaviour
{
    public bool isDead = false;
    public float respawnTimer = 0f;
    public NavMeshAgent rabbit;
    public float range; // radius of sphere
    private SpriteRenderer sr;

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
            Vector3 randomPoint = RandomNavMeshPoint();
            Debug.DrawRay(randomPoint, Vector3.up, Color.blue, 1.0f); // show gizmos
            rabbit.SetDestination(randomPoint);

            // lock the rotation to face the z axis
            transform.rotation = Quaternion.Euler(0, 0, 0);

            // flip the sprite based on the direction of movement
            Vector3 moveDirection = rabbit.destination - transform.position;
            float x = moveDirection.x;
            sr.flipX = (x > 0);
        }
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
}