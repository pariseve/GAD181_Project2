using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject stickPrefab;
    public GameObject stonePrefab;
    public Transform spawnPlane;   // the plane where resources will spawn
    public float spawnInterval = 15f; // time interval

    private int maxResourceCount = 5; // maximum number of resources in the scene

    private float nextSpawnTime;
    private Vector3 lastSpawnPosition;
    private GameObject lastSpawnedResource;

    private void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime && CountResources() < maxResourceCount)
        {
            StartCoroutine(SpawnResourceWithDistanceCheck());
        }
    }

    private IEnumerator SpawnResourceWithDistanceCheck()
    {
        GameObject resourcePrefab;

        // Decide which resource to spawn, ensuring it's different from the last one
        do
        {
            resourcePrefab = Random.Range(0f, 1f) > 0.5f ? stickPrefab : stonePrefab;
        } while (resourcePrefab == lastSpawnedResource); // Repeat until it's different

        lastSpawnedResource = resourcePrefab; // Update the last spawned resource

        Vector3 spawnPosition = CalculateRandomSpawnPosition();
        lastSpawnPosition = spawnPosition;
        spawnPosition = new Vector3(spawnPosition.x, 0.25f, spawnPosition.z);

        Debug.Log("Resource has been generated");
        Instantiate(resourcePrefab, spawnPosition, Quaternion.identity);

        nextSpawnTime = Time.time + spawnInterval;

        yield return null;
    }

    private Vector3 CalculateRandomSpawnPosition()
    {
        // Get the Renderer component of the spawnPlane
        Renderer renderer = spawnPlane.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("The spawnPlane is missing a Renderer component.");
            return Vector3.zero; // Handle the error appropriately
        }

        // Calculate random positions within the bounds of the plane
        float randomX = Random.Range(renderer.bounds.min.x, renderer.bounds.max.x);
        float randomZ = Random.Range(renderer.bounds.min.z, renderer.bounds.max.z);

        return new Vector3(randomX, 0f, randomZ);
    }

    //private Vector3 AdjustHeightToAvoidGround(Vector3 position)
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(position + Vector3.up * 100f, Vector3.down, out hit, 100f))
    //    {
    //        return hit.point;
    //    }
    //    return position;
    //}

    private int CountResources()
    {
        // count the number of resources in the scene
        GameObject[] sticks = GameObject.FindGameObjectsWithTag("Stick");
        GameObject[] stones = GameObject.FindGameObjectsWithTag("Stone");

        return sticks.Length + stones.Length;
    }
}