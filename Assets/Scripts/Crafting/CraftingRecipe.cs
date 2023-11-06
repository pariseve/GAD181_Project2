using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crafting Recipe", menuName = "New Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public Item itemToCraft;
    public ResourceCost[] costs;
}

public class ResourceCost
{
    public Item item;
    public int quantity;
}
