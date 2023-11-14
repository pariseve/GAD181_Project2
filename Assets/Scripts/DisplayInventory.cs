using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

public class DisplayInventory : MonoBehaviour
{
    public MouseItem mouseItem = new MouseItem();
    public GameObject playerCharacter;

    public GameObject inventoryPrefab;
    public InventoryObject inventory;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUM;
    public int Y_SPACE_BETWEEN_ITEMS;

    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    void Start()
    {
        CreateDisplay();
    }

    private void Update()
    {
        UpdateDisplay();

        if (Input.GetMouseButtonDown(0) && mouseItem.hoverItem != null && !FindAnyObjectByType<BeastScript>())
        {
            Vector3 characterPosition = playerCharacter.transform.position;
            float desiredYLevel = 0.25f; // Set your desired Y-level here
            Vector3 dropPosition = new Vector3(
                characterPosition.x,
                desiredYLevel,
                characterPosition.z
            );
            inventory.DropSelectedItem(mouseItem.hoverItem.item, dropPosition);
        }
        else if (Input.GetMouseButtonDown(1) && mouseItem.hoverItem != null)
        {
            Debug.Log("give item to beast");
            GiveItemToBeast(mouseItem.hoverItem.item);
        }
    }

    public void UpdateDisplay()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsDisplayed)
        {
            if (_slot.Value.ID >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[_slot.Value.item.Id].uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void CreateDisplay()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
            mouseItem.hoverItem = itemsDisplayed[obj];
    }
    public void OnExit(GameObject obj)
    {
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
    }
    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);

        if (itemsDisplayed[obj].ID >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            img.raycastTarget = false;
        }

        mouseItem.obj = mouseObject;
        mouseItem.item = itemsDisplayed[obj]; // Access the item using the obj parameter
    }
   public void OnDragEnd(GameObject obj)
{
    bool droppedOnBeast = false;

    // Check if the mouse pointer is over the beast
    if (mouseItem.hoverObj != null && mouseItem.hoverObj.CompareTag("Beast"))
    {
        droppedOnBeast = true;
    }

    if (droppedOnBeast)
    {
        // Attempt to get the BeastScript from the hovered object
        BeastScript beastScript = mouseItem.hoverObj.GetComponent<BeastScript>();

        // Check if the BeastScript is not null
        if (beastScript != null && mouseItem.item != null)
        {
            // Give the item to the beast
            beastScript.ReceiveSacrifice(mouseItem.item.item as SacrificeObject);
        }
    }
    else
    {
        // The item was not dropped on the beast, so either move it or keep it in the inventory.
        if (mouseItem.hoverObj)
        {
            inventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
        }
        else
        {
            // The item was dragged out of the inventory, so remove it from the display.
            inventory.RemoveDisplayItem(itemsDisplayed[obj].item);
        }
    }

    Destroy(mouseItem.obj);
    mouseItem.item = null;
}

    public void OnDrag(GameObject obj)
    {
        if (mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

    public void DropSelectedItem(ItemObject item, Vector3 dropPosition)
    {
        if (item != null && item.prefab != null)
        {
            // Instantiate the item's prefab at the specified position.
            GameObject droppedItem = Instantiate(item.prefab, dropPosition, Quaternion.identity);

            // Remove exactly one from the display without affecting the stack size
            inventory.RemoveDisplayItem(item);

            // Trigger inventory change when dropping an item.
            inventory.onInventoryChanged();
        }
    }


    private void RemoveItem(ItemObject item)
    {
        inventory.RemoveItem(item, 1); // Remove one item
    }


    public Vector3 GetPosition(int i)
    {
        if (NUMBER_OF_COLUM != 0)
        {
            return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUM)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUM)), 0f);
        }
        else
        {
            // Handle the case where NUMBER_OF_COLUM is zero, possibly by returning a default position.
            // For example, if you want to center an item in this case:
            return new Vector3(0, 0, 0);
        }
    }

    private void GiveItemToBeast(ItemObject item)
    {
        Debug.Log("GiveItemToBeast method called");

        // Check if the player is in the "insidehousescene"
        if (IsPlayerInsideHouseScene())
        {
            Debug.Log("Inside house scene");

            // Attempt to get the BeastScript from the beast GameObject
            GameObject beastObject = GameObject.FindWithTag("Beast");

            if (beastObject != null)
            {
                BeastScript beastScript = beastObject.GetComponent<BeastScript>();

                // Check if the BeastScript is not null
                if (beastScript != null && item != null && item is SacrificeObject)
                {
                    Debug.Log("BeastScript is not null and item is a SacrificeObject");
                    // Give the sacrifice item to the beast
                    beastScript.ReceiveSacrifice(item as SacrificeObject);

                    // Remove the item from the inventory
                    inventory.RemoveItem(item, 1);

                    // Trigger inventory change
                    inventory.onInventoryChanged();
                }
            }
            else
            {
                Debug.LogWarning("Beast not found in the scene. Make sure the Beast GameObject is correctly tagged.");
            }
        }
    }

    private bool IsPlayerInsideHouseScene()
    {
        // Check if the current scene is the "insidehousescene"
        return SceneManager.GetActiveScene().name == "InsideHouseScene";
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
}
public class MouseItem
{
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
}