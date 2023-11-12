using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public InventoryObject inventory;
    private static InventoryManager _instance;



    // Singleton pattern
    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InventoryManager>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("InventoryManager");
                    _instance = go.AddComponent<InventoryManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Items = new InventorySlot[7];
        SaveInventory();
    }

    public void SaveInventory()
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

    public void LoadInventory()
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
