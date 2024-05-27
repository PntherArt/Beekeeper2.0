using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Beekeeper Game/TestItem", order = 0)]
public class InventoryItem : ScriptableObject
{
    public VisibleObject obj;
    public GameObject itemMesh; // mesh for the item
    public int maxStackSize; // prevent stack size from getting too big

}
