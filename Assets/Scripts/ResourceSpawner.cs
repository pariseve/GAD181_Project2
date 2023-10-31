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
        // Decide which resource to spawn
        GameObject resourcePrefab = Random.Range(0f, 1f) > 0.5f ? stickPrefab : stonePrefab;
        Debug.Log("Choosing Resource to spawn");

        // Calculate a random position within the bounds of the spawn plane
        Vector3 spawnPosition = CalculateRandomSpawnPosition();
        Debug.Log("Calculating Distance");

        // Ensure the new position is a fair distance from the last spawn position
        //while (Vector3.Distance(spawnPosition, lastSpawnPosition) < 8f)
        //{
        //    spawnPosition = CalculateRandomSpawnPosition();
        //}

        lastSpawnPosition = spawnPosition;

        // Manually set the height to avoid clipping into the ground
        spawnPosition = new Vector3(spawnPosition.x, 0.25f, spawnPosition.z); // Adjust the Y-coordinate (2.0f is just an example height)

        // Spawn the selected resource at the calculated position
        Debug.Log("Resource has been generated");
        Instantiate(resourcePrefab, spawnPosition, Quaternion.identity);

        // Set the next spawn time
        Debug.Log("Setting spawn time");
        nextSpawnTime = Time.time + spawnInterval;

        yield return null;
    }

    private Vector3 CalculateRandomSpawnPosition()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnPlane.localScale.x / 2, spawnPlane.localScale.x / 2),
            0f,
            Random.Range(-spawnPlane.localScale.z / 2, spawnPlane.localScale.z / 2)
        );

        return spawnPlane.position + randomOffset;
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