using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipmentContent : CatalogContent
{

    public PlayerRaycast playerRaycast;
    public Inventory playerInventory;
    public AlertManager alertManager;

    private void Start()
    {
        // DEBUG
        foreach (CatalogObject cObj in testList)
        {
            if (storage.ContainsKey(cObj))
            {
                storage[cObj]++;
            }
            else
            {
                storage.Add(cObj, 1);
            }
        }

        // get window size for later
        rectSize = new Vector2(Display.main.systemWidth, Display.main.systemHeight);
        // get the rect transform of this ui object
        rTransform = GetComponent<RectTransform>();

        // for the shipment catalog is always limited
        catalogLimited = true;
    }

    public override void updateCatalog()
    {
        // empty anything from before
        emptyCatalog();

        Vector2 itemSize = itemPrefab.GetComponent<RectTransform>().sizeDelta;
        // set up catalog screen properties
        Vector2 delta = _resizeContentRect(itemSize);

        // add mask padding for vertical edge 
        rectMask.padding = new Vector4(0, 0, 0, edgePaddingInPixels.y);
        int col = 0; int row = 0;

        // insert items into catalog
        foreach (CatalogObject cObj in storage.Keys)
        {
            // set up item and load into game
            int numObj = storage[cObj];
            GameObject item = Instantiate(itemPrefab, transform.position, rTransform.rotation, transform);
            item.name = row + " " + col;
            ShipmentItemUI itemUI = item.GetComponent<ShipmentItemUI>();
            CatalogObject obj = cObj;
            int prefabId = playerRaycast.findInPrefabs(obj.id);
            InventoryItem invItem = null;
            if (prefabId != -1)
            {
                invItem = playerRaycast.holdablePrefabItems[prefabId].GetComponent<Item>().itemData;
                itemUI.item = invItem;
                itemUI.amt = numObj;
            }
            else
                Debug.LogError("no ID found for this shipment object!");


            // set button to add item to cart
            itemUI.loadUI(() =>
            {
                if (this.storage.ContainsKey(obj))
                {
                    int added;

                    if (prefabId != -1)
                        added = playerInventory.AddToInventory(invItem, 1);
                    else
                    {
                        Debug.LogError("no ID found for this shipment object!");
                        added = -1;
                    }

                    if (added >= 0)
                        this.removeItem(obj);
                    else
                    {
                        alertManager.queueAlert("Inventory full!");
                    }

                }
            });

            // set position
            Vector2 pos = new Vector2(
                -rectSize.x * 0.5f + edgePaddingInPixels.x + itemSize.x * 0.5f + col * (delta.x + itemSize.x),
                rectSize.y - edgePaddingInPixels.y - itemSize.y * 0.5f - row * (delta.y + itemSize.y) - verticalPadding
            );
            Debug.Log(row + " " + col + ", delta:" + delta + ", pos:" + pos + ", rectsize: " + rectSize);
            itemUI.GetComponent<RectTransform>().anchoredPosition = pos;

            col++;
            if (col == numColumns)
            {
                col = 0;
                row++;
            }
            if (!isCart)
                updateButtons();
        }
    }
}
