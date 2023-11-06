using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crafting Recipe", menuName = "New Crafting Recipe")]

public class CraftingRecipe : ScriptableObject
{
    public ItemObject itemToCraft;
    public ResourceCost[] costs;
}

[System.Serializable]
public class ResourceCost
{
    public ItemObject item;
    public int quantity;
}
