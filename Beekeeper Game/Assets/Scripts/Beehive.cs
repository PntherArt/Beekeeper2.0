using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beehive : MonoBehaviour
{
    public int beeCapacity;
    public int beeOccupancy;
    public float interactionRadius;
    public DayCycle dayCycle;

    // true if beehive keeps bees searching.
    public bool beesSearching = true;

    int activeBees = 0;

    public int productMax = 1;

    // dictionary with keys as products, and values as the amount of that product in beehive
    public Dictionary<ProductObj, float> productDict = new Dictionary<ProductObj, float>();

    public GameObject beePrefab;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    void Update()
    {
        beesSearching = (
            !dayCycle.isNight() // its not night
            && FlowerManager.getFlowerList().Exists(
                flowerObject =>
                {
                    return (transform.position - flowerObject.transform.position).magnitude <= interactionRadius;
                }
            ) // there are flowers within interaction radius
        );
        if (beesSearching && activeBees < beeOccupancy) // if its searching period, send any bees in the hive outside to search
        {
            activeBees++;

            Bee newBee = Instantiate(beePrefab, this.transform.position + Random.onUnitSphere * 0.1f + new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<Bee>();
            newBee.parentBhive = this;
            newBee.dayCycle = dayCycle;
        }
        else
        {
            beesSearching = false;
        }
    }

    public void arrivedHome(Bee bee)
    {
        // receive product from bee if it exists
        ProductObj productType = bee.heldProduct;
        if (productType != null)
        {
            if (!productDict.ContainsKey(productType))
            {
                productDict.Add(productType, 0);
            }

            // fill product if you can
            productDict[productType] = Mathf.Min(productMax, productDict[productType] + bee.product);

            //Debug.Log("bee arrived home with " + bee.product + " " + productType.objectName + ", total amount now " + productDict[productType]);

            // reset bee values and send it over
            bee.heldProduct = null;
            bee.product = 0;
        }
        if (beesSearching)
        {
            bee.StartCoroutine("startSearch");
        }
        else
        {
            Destroy(bee.gameObject);
            activeBees--;
        }
    }
}
