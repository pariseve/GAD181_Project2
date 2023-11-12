using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public string insideHouseScene = "InsideHouseScene";
    public string woodlandScene = "WoodlandScene";

    private bool playerInRange = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            SaveGameState(); // Save the game state before transitioning to another scene
            SceneManager.LoadScene(insideHouseScene);
        }
    }

    // Save the state of the game (e.g., picked up resources and inventory)
    void SaveGameState()
    {
        // Save inventory
        InventoryManager.Instance.SaveInventory();
        // Save other game state data if needed
    }

    public static void SaveState()
    {
        // Implement the logic to save the scene state here
        // You may want to save information about the state of resources or other relevant data
        // For simplicity, let's just print a message for now
        Debug.Log("Saving scene state...");
    }
}
