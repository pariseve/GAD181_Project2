using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public float respawnTime = 30f; // Adjust this as needed

    void Update()
    {
        // Find all game objects with the "animal" tag
        GameObject[] animals = GameObject.FindGameObjectsWithTag("animal");

        foreach (GameObject animal in animals)
        {
            AnimalManager animalManager = animal.GetComponent<AnimalManager>();

            // Check if the animal is dead
            if (animalManager != null && animalManager.isDead)
            {
                // Start the respawn timer
                animalManager.respawnTimer += Time.deltaTime;

                // Check if the respawn timer has reached the specified time
                if (animalManager.respawnTimer >= respawnTime)
                {
                    // Respawn the animal
                    animalManager.RespawnAnimal();
                }
            }
        }
    }
}
