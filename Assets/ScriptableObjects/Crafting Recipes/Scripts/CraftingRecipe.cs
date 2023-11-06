using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting System/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public ItemObject craftedItem;
    public RecipeRequirement[] requiredItems;
}

[System.Serializable]
public class RecipeRequirement
{
    public ItemObject item;
    public int amount;
}
