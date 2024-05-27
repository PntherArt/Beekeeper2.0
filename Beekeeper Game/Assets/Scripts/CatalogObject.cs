using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CatalogObject", menuName = "Beekeeper Game/CatalogObject", order = 0)]
public class CatalogObject : VisibleObject
{
    // Abstract class for scriptable objects that can be bought and sold.
    public int buyValue = 0;
    public int sellValue = 0;
}
