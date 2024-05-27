using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlowerObject", menuName = "Beekeeper Game/FlowerObject", order = 0)]
public class FlowerObject : CatalogObject
{
    // Information about a type of flower.

    // The flower mesh prefab associated with this flower
    public GameObject flowerMesh;

    // The height of the flower (bees will go to this height)
    public float height;

    // The radius of the flower (bees will not try to get closer than this)
    public float radius;

    // The product the flower creates
    public ProductObj product;

    // The amount of product the flower starts out with every day
    public float productPerDay;
}

