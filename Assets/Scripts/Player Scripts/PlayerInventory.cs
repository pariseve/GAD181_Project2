using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public DisplayInventory displayInventory;
    public GameObject bushPrefab; // Reference to the bush prefab
    public GameObject berriesPrefab; // Reference to the berries prefab

    private GroundItem currentGroundItem; // Track the currently collided ground item


    private void OnTriggerEnter(Collider other)
    {
        var groundItem = other.GetComponent<GroundItem>();
        if (groundItem)
        {
            currentGroundItem = groundItem;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var groundItem = other.GetComponent<GroundItem>();
        if (groundItem == currentGroundItem)
        {
            currentGroundItem = null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentGroundItem != null)
        {
            Debug.Log("Player picked up item");
            ItemObject itemObject = currentGroundItem.item;
            inventory.AddItem(itemObject, 1);
            // Call the UpdateDisplay method from the DisplayInventory component
            displayInventory.UpdateDisplay();
            Destroy(currentGroundItem.gameObject);

            // Replace the berry bush with the bush prefab
            if (currentGroundItem.CompareTag("Berries"))
            {
                Vector3 position = currentGroundItem.transform.position;

                // Instantiate the bush prefab in place of the berry bush
                Instantiate(bushPrefab, position, Quaternion.identity);

                // Start the coroutine to respawn the berry bush after 5 seconds
                StartCoroutine(RespawnBerryBush(position));
            }

            // Save inventory state after picking up an item
            SaveInventoryState();
        }
    }

    private IEnumerator RespawnBerryBush(Vector3 position)
    {
        yield return new WaitForSeconds(15f); // Wait for 15 seconds

        // Destroy any existing bush at the position after the delay
        DestroyExistingBush(position);

        // Instantiate the berry bush at the same position
        Instantiate(berriesPrefab, position, Quaternion.identity);
    }

    private void DestroyExistingBush(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Bush"))
            {
                Debug.Log("Destroying bush");
                Destroy(collider.gameObject);
            }
        }
    }

    private void OnApplicationQuit()
    {
        // Save inventory state when the application quits
        SaveInventoryState();
    }

    private void SaveInventoryState()
    {
        string saveData = JsonUtility.ToJson(inventory, true);
        PlayerPrefs.SetString("Inventory", saveData);
        PlayerPrefs.Save();
    }
}





