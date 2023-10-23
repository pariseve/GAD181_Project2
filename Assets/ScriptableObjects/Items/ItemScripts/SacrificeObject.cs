using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sacrifice Object", menuName = "Inventory System/Items/Sacrifice")]
public class SacrificeObject : ItemObject
{
    public int sacrificeValue;
    public void Awake()
    {
        type = ItemType.Sacrifices;
    }
}
