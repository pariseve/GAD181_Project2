using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crafting Recipe", menuName = "New Crafting Recipe")]

public class CraftingRecipe : ScriptableObject
{
    public ItemDataBaseObject itemToCraft;
    public ResourceCost[] costs;
}

[System.Serializable]
public class ResourceCost
{
    public ItemDataBaseObject item;
    public int quantity;
}
