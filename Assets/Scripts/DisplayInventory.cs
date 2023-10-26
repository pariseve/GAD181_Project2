using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class DisplayInventory : MonoBehaviour
{
    public GameObject inventoryPrefab;
    public InventoryObject inventory;

    public int xStart;
    public int yStart;
    public int xSpaceBetweenItem;
    public int numberOfColumn;
    public int ySpaceBetweenItem;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }
    //private void Update()
    //{
    //    UpdateDisplay();
    //}
    //update is called once per frame
    public void UpdateDisplay()
    {
        Debug.Log("Update Display is called");
        for (int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];
            // Check if the slot is already displayed in the inventory
            if (itemsDisplayed.ContainsKey(slot))
            {
                // Use null conditional operator to access the TextMeshProUGUI component
                TextMeshProUGUI textComponent = itemsDisplayed[slot]?.GetComponentInChildren<TextMeshProUGUI>();

                if (textComponent != null)
                {
                    textComponent.text = slot.amount.ToString("n0");
                }
            }
            else
            {
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                Debug.Log("Instantiated prefab: " + obj.name);

                // Use null conditional operator to access the Image component
                //Image imageComponent = obj?.transform.GetChild(0)?.GetComponentInChildren<Image>();
                //if (imageComponent != null)
                //{
                //    imageComponent.sprite = inventory.database.GetItem[slot.item.Id].uiDisplay;
                //}

                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

                // Use null conditional operator to access the TextMeshProUGUI component
                TextMeshProUGUI textComponent = obj?.GetComponentInChildren<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = slot.amount.ToString("n0");
                }

                itemsDisplayed.Add(slot, obj);
            }
        }
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];

            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.Id].uiDisplay;
            Debug.Log("Instantiated prefab: " + obj.name);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            itemsDisplayed.Add(slot, obj);
        }
    }
    public Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (xSpaceBetweenItem * (i % numberOfColumn)), yStart + (-ySpaceBetweenItem * (i / numberOfColumn)), 0f);
    }
}
