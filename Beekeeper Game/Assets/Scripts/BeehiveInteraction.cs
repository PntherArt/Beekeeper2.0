using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Beehive))]
public class BeehiveInteraction : Interactable
{
    public GameObject player;
    public AlertManager alertManager;
    Beehive beehive;

    void Start()
    {
        loadPopup();
        beehive = GetComponent<Beehive>();
        //onInteracted.AddListener(() => fillJar());
    }

    public void fillJar(ProductObj product = null)
    {
        GameObject inHandItem = player.GetComponent<PlayerRaycast>().inHandItem;
        Jar jar = inHandItem.GetComponent<Jar>();
        if (!jar.isNonEmpty())
        {
            if (product == null)
            {
                (ProductObj, float) maxProd = extractMaxProduct();
                jar.setProduct(maxProd.Item1, maxProd.Item2);
                alertManager.queueAlert("extracted " + maxProd.Item2 + " of " + maxProd.Item1.name + " from beehive");
            } 
            else
            {
                (ProductObj, float) prod = extractProduct(product);
                jar.setProduct(prod.Item1, prod.Item2);
                alertManager.queueAlert("extracted " + prod.Item2 + " of " + prod.Item1.name + " from beehive");
            }
            
        } else
        {
            if (product == jar.productType)
            {
                (ProductObj, float) prod = extractProduct(product);
                jar.setProduct(prod.Item1, jar.productAmt + prod.Item2);
                alertManager.queueAlert("extracted " + prod.Item2 + " of " + prod.Item1.name + " from beehive");
            }
            else
            {
                alertManager.queueAlert("Your jar already has a different kind of product! Sell it first before getting another type of product.");
            }
        }
    }

    (ProductObj, float) extractProduct(ProductObj product)
    {
        (ProductObj, float) prodExtracted = (null, 0);
        Dictionary<ProductObj, float> pDict = beehive.productDict;
        if (pDict.ContainsKey(product))
        {
            prodExtracted = (product, pDict[product]);
            pDict.Remove(product);
        } else
        {
            Debug.LogError("Tried to extract " + product.objectName + " when there is none in this beehive");
        }
        return prodExtracted;


    }

    (ProductObj, float) extractMaxProduct()
    {
        (ProductObj, float) maxProd = (null, 0);
        Dictionary<ProductObj, float> pDict = beehive.productDict;

        foreach (ProductObj prod in pDict.Keys)
        {
            if (pDict[prod] >= maxProd.Item2)
            {
                maxProd = (prod, pDict[prod]);
            }
        }

        // remove product from beehive
        if (maxProd.Item1)
            pDict.Remove(maxProd.Item1);

        return maxProd;
    }

    public override bool playerInInteractableCondition()
    {
        GameObject inHandItem = player.GetComponent<PlayerRaycast>().inHandItem;
        Item item = null;
        if (inHandItem != null)
            item = inHandItem.GetComponent<Item>();

        return inHandItem != null && item != null && item.itemData.obj.id == 3;

        //return true;
    }
    
    
}
