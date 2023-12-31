using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject selectedItem;
    public ItemObject database;
    public Inventory Container;

    public void onInventoryChanged()
    {
        SaveInventory();
    }

    public void AddItem(ItemObject item, int amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID == item.Id)
            {
                Container.Items[i].AddAmount(amount);
                onInventoryChanged(); //trigger save when inventory changes
                return;
            }
        }
        SetEmptySlot(item, amount);
        onInventoryChanged();
    }

    public InventorySlot SetEmptySlot(ItemObject _item, int _amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID <= -1)
            {
                Container.Items[i].UpdateSlot(_item.Id, _item, _amount);
                return Container.Items[i];
            }
        }
        // handle the case where the inventory is full
        return null;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(temp.ID, temp.item, temp.amount);
    }

    public void RemoveItem(ItemObject item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item == item)
            {
                Container.Items[i].UpdateSlot(-1, null, 0);
            }
        }
    }

    public void RemoveDisplayItem(ItemObject item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID == item.Id)
            {
                if (Container.Items[i].amount > 1)
                {
                    // Decrease the display amount by 1 without affecting the stack size
                    Container.Items[i].amount--;
                }
                else
                {
                    // If there's only one left in the display, remove the item from the display
                    Container.Items[i].UpdateSlot(-1, null, 0);
                }

                onInventoryChanged(); // Trigger save when inventory changes
                return;
            }
        }
    }

    public void DropSelectedItem(ItemObject item, Vector3 dropPosition)
    {
        if (item != null && item.prefab != null)
        {
            // Instantiate the item's prefab at the specified position.
            Instantiate(item.prefab, dropPosition, Quaternion.identity);

            // Now, you can remove the item from the inventory.
            RemoveItem(item);
        }
    }
    private void SaveInventory()
    {
        string saveData = JsonUtility.ToJson(this, true);
        PlayerPrefs.SetString("Inventory", saveData);
        PlayerPrefs.Save();
    }

    private void LoadInventory()
    {
        if (PlayerPrefs.HasKey("Inventory"))
        {
            string saveData = PlayerPrefs.GetString("Inventory");
            JsonUtility.FromJsonOverwrite(saveData, this);
        }
    }

    public void OnAfterDeserialize()
    {
        //for (int i = 0; i < Container.Items.Count; i++)
        //{
        //    ItemObject itemObject;
        //    if (database.GetItem.TryGetValue(Container.Items[i].ID, out itemObject))
        //    {
        //        Container.Items[i].item = itemObject;
        //    }
        //}
    }

    public void OnBeforeSerialize()
    {
    }
}

[System.Serializable]
public class Inventory
{
    internal static object instance;
    public InventorySlot[] Items = new InventorySlot[7];
}

[System.Serializable]
public class InventorySlot
{
    public int ID = -1;
    public ItemObject item;
    public int amount;

    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }

    public InventorySlot(int _id, ItemObject _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void UpdateSlot(int _id, ItemObject _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}