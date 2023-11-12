using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public DisplayInventory displayInventory;

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

            // Save inventory state after picking up an item
            SaveInventoryState();
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
