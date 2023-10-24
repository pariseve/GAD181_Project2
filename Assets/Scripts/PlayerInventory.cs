using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public DisplayInventory displayInventory;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player has collided with item");
        var groundItem = other.GetComponent<GroundItem>();
        if (groundItem)
        {
            ItemObject itemObject = groundItem.item;
            inventory.AddItem(itemObject, 1);
            // Call the UpdateDisplay method from the DisplayInventory component
            displayInventory.UpdateDisplay();
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            inventory.Load();
        }
    }
    private void OnApplicationQuit()
    {
        inventory.Container.Items.Clear();
    }
}
