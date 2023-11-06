using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingBench : MonoBehaviour
{
    public GameObject craftingUI; // Reference to the crafting UI object
    private bool isPlayerNearBench = false;

    private void Start()
    {
        craftingUI.SetActive(false); // Initially, hide the crafting UI
    }

    private void Update()
    {
        if (isPlayerNearBench && Input.GetKeyDown(KeyCode.E))
        {
            ToggleCraftingUI();
        }
    }

    private void ToggleCraftingUI()
    {
        bool isOpen = craftingUI.activeSelf;
        craftingUI.SetActive(!isOpen);

        if (!isOpen)
        {
            // Pause the game when the crafting UI is opened
            Time.timeScale = 0f;
        }
        else
        {
            // Unpause the game when the crafting UI is closed
            Time.timeScale = 1f;
        }
    }

    public void CloseCraftingUI()
    {
        craftingUI.SetActive(false);
        Time.timeScale = 1f; // Unpause the game when the UI is closed
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearBench = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearBench = false;
            CloseCraftingUI(); // Close the UI when the player moves away
        }
    }
}
