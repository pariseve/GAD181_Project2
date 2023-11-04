using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public InventoryObject inventory;
    private static InventoryManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Items = new InventorySlot[7];
        SaveInventory(); // Save the inventory when quitting the application
    }

    private void SaveInventory()
    {
        string saveData = JsonUtility.ToJson(inventory, true);
        PlayerPrefs.SetString("Inventory", saveData);
        PlayerPrefs.Save();
    }

    public void ResetInventory()
    {
        inventory.Container.Items = new InventorySlot[7];
        SaveInventory();
    }

    private void LoadInventory()
    {
        if (PlayerPrefs.HasKey("Inventory"))
        {
            string saveData = PlayerPrefs.GetString("Inventory");
            JsonUtility.FromJsonOverwrite(saveData, inventory);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartScreen")
        {
            ResetInventory();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}