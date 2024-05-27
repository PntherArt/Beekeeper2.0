using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CatalogContent : MonoBehaviour
{
    public Dictionary<CatalogObject, int> storage = new Dictionary<CatalogObject, int>();
    public List<CatalogObject> testList;
    public GameObject itemPrefab;
    public CatalogContent other;
    public GlobalVariables globalVariables;
    public bool isCart = false;
    public bool catalogLimited = true;
    public int numColumns = 5;
    public RectTransform windowRect;
    public RectMask2D rectMask;
    public int verticalPadding = 100;
    public Vector2Int edgePaddingInPixels = new Vector2Int(100, 100);
    
    protected RectTransform rTransform;
    [SerializeField] protected Vector2 rectSize;

    void Start()
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
        rectSize = windowRect.sizeDelta;
        // get the rect transform of this ui object
        rTransform = GetComponent<RectTransform>();

    }
    public void emptyCatalog()
    {
        // flush catalog
        Transform[] items = GetComponentsInChildren<Transform>();
        foreach (Transform item in items)
        {
            if (item != this.transform)
                GameObject.Destroy(item.gameObject);
        }
    }

    public virtual void addItem(CatalogObject objToAdd)
    {
        if (storage.ContainsKey(objToAdd))
        {
            storage[objToAdd]++;
        }
        else
        {
            storage.Add(objToAdd, 1);
        }
        updateCatalog();

    }

    public virtual void removeItem(CatalogObject objToRemove)
    {
        int cost = objToRemove.buyValue;

        if (storage.ContainsKey(objToRemove))
        {
            storage[objToRemove]--;
            if (storage[objToRemove] <= 0)
            {
                storage.Remove(objToRemove);
            }
        }
        updateCatalog();
    }

    public Vector2 _resizeContentRect(Vector2 itemSize)
    {

        // set vertical size depending on num of rows and item count
        int numItems = storage.Count;
        int numRows = Mathf.CeilToInt((float)numItems / numColumns);
        // Debug.Log(numRows);

        float contentRectVerticalLength = 2 * edgePaddingInPixels.y + (numRows - 1) * (itemSize.y + verticalPadding) + itemSize.y;
        rTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentRectVerticalLength);
        rectSize.y = contentRectVerticalLength;
        Debug.Log(contentRectVerticalLength + ", " + windowRect.rect.height);
        // Debug.Log(rectSize);

        // return distance between items in 2d
        float deltaX, deltaY;
        deltaX = (numColumns <= 1) ? 0 : (rectSize.x - 2 * edgePaddingInPixels.x - itemSize.x * numColumns) / (numColumns - 1);
        deltaY = (numRows <= 1) ? 0 : (rectSize.y - 2 * edgePaddingInPixels.y - itemSize.y * numRows) / (numRows - 1);

        return new Vector2(deltaX, deltaY);
    }

    public virtual void updateCatalog()
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
            CatalogItemUI itemUI = item.GetComponent<CatalogItemUI>();
            itemUI.catalogObject = cObj;
            itemUI.amt = numObj;

            if (!isCart)
            {
                // set button to add item to cart
                itemUI.loadUI(() =>
                {

                    if (catalogLimited)
                    {
                        if (this.storage.ContainsKey(itemUI.catalogObject))
                        {
                            this.removeItem(itemUI.catalogObject);
                            globalVariables.changeMoney(-itemUI.catalogObject.buyValue);
                            other.addItem(itemUI.catalogObject);
                        }
                    }
                    else
                    {
                        globalVariables.changeMoney(-itemUI.catalogObject.buyValue);
                        other.addItem(itemUI.catalogObject);
                        this.updateButtons();
                    }



                }, catalogLimited, false);
            }
            else
            {
                // set button to remove item from cart
                itemUI.loadUI(() =>
                {
                    if (other.catalogLimited)
                    {
                        if (this.storage.ContainsKey(itemUI.catalogObject))
                        {
                            other.addItem(itemUI.catalogObject);
                            globalVariables.changeMoney(itemUI.catalogObject.buyValue);
                            this.removeItem(itemUI.catalogObject);
                        }
                    }
                    else
                    {
                        if (this.storage.ContainsKey(itemUI.catalogObject))
                        {
                            globalVariables.changeMoney(itemUI.catalogObject.buyValue);
                            this.removeItem(itemUI.catalogObject);
                            other.updateButtons();
                        }
                    }



                }, true, true);
            }



            // set position
            Vector2 pos = new Vector2(
                -rectSize.x * 0.5f + edgePaddingInPixels.x + itemSize.x * 0.5f + col * (delta.x + itemSize.x),
                rectSize.y * 0.5f - edgePaddingInPixels.y - itemSize.y * 0.5f - row * (delta.y + itemSize.y)
            );
            Debug.Log(row + " " + col + ", delta:" + delta + ", pos:" + pos);
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

    public void updateButtons()
    {
        foreach (CatalogItemUI itemUI in GetComponentsInChildren<CatalogItemUI>())
        {
            itemUI.updateButtons();
        }
    }

}
