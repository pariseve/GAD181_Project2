using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CraftingSystem : MonoBehaviour
{
    public InventoryObject playerInventory;

    public CraftingRecipe smallTrapRecipe;
    public CraftingRecipe largeTrapRecipe;

    public void CraftSmallTrap()
    {
        if (CanCraft(smallTrapRecipe))
        {
            DeductResources(smallTrapRecipe);
            playerInventory.AddItem(smallTrapRecipe.craftedItem, 1);
        }
        else
        {
            Debug.Log("Insufficient resources to craft a small trap.");
        }
    }

    public void CraftLargeTrap()
    {
        if (CanCraft(largeTrapRecipe))
        {
            DeductResources(largeTrapRecipe);
            playerInventory.AddItem(largeTrapRecipe.craftedItem, 1);
        }
        else
        {
            Debug.Log("Insufficient resources to craft a large trap.");
        }
    }

    private bool CanCraft(CraftingRecipe recipe)
    {
        foreach (var requiredItem in recipe.requiredItems)
        {
            if (!playerInventory.HasItem(requiredItem.item, requiredItem.amount))
            {
                return false;
            }
        }
        return true;
    }

    private void DeductResources(CraftingRecipe recipe)
    {
        foreach (var requiredItem in recipe.requiredItems)
        {
            playerInventory.RemoveItem(requiredItem.item, requiredItem.amount);
        }
    }
}



