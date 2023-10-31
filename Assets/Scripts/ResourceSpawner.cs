using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject stickPrefab; // Reference to the stick prefab.
    public GameObject stonePrefab; // Reference to the stone prefab.
    public Transform spawnPlane;   // The plane on which resources will spawn.
    public float spawnInterval = 20f; // Time interval for resource spawning 

    private int maxResourceCount = 3; // Maximum number of resources in the scene.

    private float nextSpawnTime;
    private Vector3 lastSpawnPosition;

    private void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime && CountResources() < maxResourceCount)
        {
            // Decide which resource to spawn (stick or stone).
            GameObject resourcePrefab = Random.Range(0f, 1f) > 0.5f ? stickPrefab : stonePrefab;
            Debug.Log("Choosing Resource to spawn");
            // Calculate a random position within the bounds of the spawn plane.
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnPlane.localScale.x / 2, spawnPlane.localScale.x / 2),
                0f,
                Random.Range(-spawnPlane.localScale.z / 2, spawnPlane.localScale.z / 2)
            );

            Vector3 spawnPosition = spawnPlane.position + randomOffset;

            // Ensure the new position is a safe distance from the last spawn position.
            float minDistance = 5f; // Adjust as needed.
            Debug.Log("Calculating Distance");
            while (Vector3.Distance(spawnPosition, lastSpawnPosition) < minDistance)
            {
                randomOffset = new Vector3(
                    Random.Range(-spawnPlane.localScale.x / 2, spawnPlane.localScale.x / 2),
                    0f,
                    Random.Range(-spawnPlane.localScale.z / 2, spawnPlane.localScale.z / 2)
                );

                spawnPosition = spawnPlane.position + randomOffset;
            }

            lastSpawnPosition = spawnPosition;

            // Adjust the height to avoid clipping into the ground.
            RaycastHit hit;
            if (Physics.Raycast(spawnPosition + Vector3.up * 1.7f, Vector3.down, out hit, 100f))
            {
                spawnPosition = hit.point;
            }

            // Spawn the selected resource at the calculated position.
            Debug.Log("Resource has been generated");
            Instantiate(resourcePrefab, spawnPosition, Quaternion.identity);

            // Set the next spawn time.
            Debug.Log("Setting spawn time");
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private int CountResources()
    {
        // Count the number of resources in the scene (sticks and stones).
        GameObject[] sticks = GameObject.FindGameObjectsWithTag("Stick");
        GameObject[] stones = GameObject.FindGameObjectsWithTag("Stone");

        return sticks.Length + stones.Length;
    }
}
