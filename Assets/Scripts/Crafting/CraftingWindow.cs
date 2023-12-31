using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingWindow : MonoBehaviour
{
    public CraftingRecipeUI[] recipeUIs;
    public static CraftingWindow instance;

    void Awake ()
    {
        instance = this;
    }

    void OnEnable()
    {
        Inventory.instance.onOpenInventory.AddListener(OnOpenInventory);
    }

    void OnDiable ()
    {
        Inventory.instance.onOpenInventory.RemoveListener(OnOpenInventory);
    }

    void OnOpenInventory ()
    {
        gameObject.SetActive(false);
    }

    public void Craft (CraftingRecipe recipe)
    {
        for(int i = 0; i < recipe.costs.Length; i++)
        {
            for(int x = 0; x < recipe.costs[i].quantity;  x++)
            {
                Inventory.instance.RemoveItem(recipe.costs[i].item);
            }
        }

        Inventory.instance.AddItem(recipe.itemToCraft);
    }
}
