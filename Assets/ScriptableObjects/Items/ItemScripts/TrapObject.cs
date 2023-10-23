using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trap Object", menuName = "Inventory System/Items/Trap")]
public class TrapObject : ItemObject
{
    public float atk;
    public void Awake()
    {
        type = ItemType.Traps;
    }
}
