using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField]
    private LayerMask pickableLayerMask; // only detect colliders for pickupable obkects

    public GameObject flowerParent;

    [SerializeField]
    private Transform playerCameraTransform; // for camera raycast

    [SerializeField]
    [Min(1)]
    public float hitRange = 5;

    private RaycastHit hit;

    [SerializeField]
    private Transform pickUpParent;

    [SerializeField]
    public GameObject inHandItem;

    public Inventory testInventory;

    public GameObject[] holdablePrefabItems;

    private void Update()
    {
        // If switch active slot
        if (Input.GetKeyDown("1")) {
            resetInHandItemToActiveSlot(0);
        }
        if (Input.GetKeyDown("2")) {
            resetInHandItemToActiveSlot(1);
        }
        if (Input.GetKeyDown("3")) {
            resetInHandItemToActiveSlot(2);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
         {
            if (testInventory.activeSlot > 0) {
                resetInHandItemToActiveSlot(testInventory.activeSlot - 1);
            } else {
                resetInHandItemToActiveSlot(testInventory.numberOfSlots - 1);
            }
             
         }
         if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
         {
             if (testInventory.activeSlot < testInventory.numberOfSlots - 1) {
                resetInHandItemToActiveSlot(testInventory.activeSlot + 1);
            } else {
                resetInHandItemToActiveSlot(0);
            }
         }

    }

    public void placeObject(RaycastHit hitCast) {
        if (inHandItem != null) {
            //Debug.Log("Passed HITTER: " + hitCast);
            int modifiedSlot = testInventory.RemoveFromInventory(inHandItem.GetComponent<Item>().itemData, 1);

            //Debug.Log("HITCAST X: " + hitCast.point.x);
            //Debug.Log("HITCAST Z: " + hitCast.point.z);
            //Debug.Log("HITCAST OBJ ID: " + inHandItem.GetComponent<Item>().itemData.obj.id);

            int prefabId = findInPrefabs(inHandItem.GetComponent<Item>().itemData.obj.id);
            if (prefabId == -1) return;

            // if object is jar
            bool objectIsJar = false;
            (ProductObj, float) productInfo = (null, 0);
            if (prefabId == 3) { // if item's id is Jar's id
                objectIsJar = true;
                // get product info 
                productInfo = inHandItem.GetComponent<Jar>().getProductNoMutate();
            }           

            GameObject tmpItem = Instantiate<GameObject>(holdablePrefabItems[findInPrefabs(inHandItem.GetComponent<Item>().itemData.obj.id)], new Vector3(hitCast.point.x, hitCast.point.y, hitCast.point.z), Quaternion.identity);

            // if object is jar
            if (prefabId == 3) {
                // setProduct of new jar
                tmpItem.GetComponent<Jar>().setProduct(productInfo.Item1, productInfo.Item2);
            } else if (tmpItem.CompareTag("Flower"))
            {
                tmpItem.transform.SetParent(flowerParent.transform);
                tmpItem.GetComponent<Flower>().Nectar = inHandItem.GetComponent<Flower>().Nectar;
            }

            inHandItem.transform.SetParent(null);
            Destroy(inHandItem);

            // clear in hand item
            inHandItem = null;
            if (modifiedSlot != -1 && testInventory.inventoryItems[modifiedSlot].stackAmount > 0)
            {
                setInHandItem(testInventory.inventoryItems[modifiedSlot].item.obj.id);
            }
        }                
    }

    public bool isInventoryFull() {
        for (int i = 0; i < testInventory.inventoryItems.Count; i++) {
            if (testInventory.inventoryItems[i].isEmpty()) {
                return false;
            }
        }
        return true;
    }

    
    public void clearPickupParent() {
        foreach (Transform child in pickUpParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void resetInHandItemToActiveSlot(int slot) {
        // before moving away
        // check if jar
        bool isJar = false;
        (ProductObj, float) jarProductInfo = (null, 0);
        if (testInventory.GetActiveSlotItemId() == 3) {
            // store product info (in the slot obj)
            jarProductInfo = inHandItem.GetComponent<Jar>().getProductNoMutate();
            testInventory.setJarProductInfo(testInventory.activeSlot, jarProductInfo.Item1, jarProductInfo.Item2);
        }

        testInventory.SetActiveSlot(slot);

        // set in hand item as active slot item
        clearInHandItem();
        if (testInventory.inventoryItems[slot].item != null) {
            // for this new active slot item, if jar
            if (testInventory.GetActiveSlotItemId() == 3) {
                // set in hand item while keeping contents
                setInHandItem(testInventory.inventoryItems[slot].item.obj.id, testInventory.inventoryItems[slot].jarProductInfo);
            } else { // if not jar, set item without retrieving jar contents
                setInHandItem(testInventory.inventoryItems[slot].item.obj.id);
            }
        }
    }

    public void setInitialInHandItem(GameObject setItem) {
        //Debug.Log("REACHED INITIAL SET");
        foreach (Transform child in pickUpParent.transform)
        {
            Destroy(child.gameObject);
        }
        inHandItem = setItem;
        Rigidbody rb = setItem.GetComponent<Rigidbody>();
        inHandItem.transform.position = Vector3.zero;
        //inHandItem.transform.rotation = Quaternion.identity;
        inHandItem.transform.rotation = new Quaternion(0f, -0.90f, 0f, 1);
        GameObject testParent = GameObject.Find("PickUpSlot");
        inHandItem.transform.SetParent(testParent.transform, false);
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    // get set item
    public void setInHandItem(int itemId) {
        clearInHandItem();
        inHandItem = Instantiate(holdablePrefabItems[findInPrefabs(itemId)], Vector3.zero, new Quaternion(0f, -0.90f, 0f, 1));
        inHandItem.transform.SetParent(pickUpParent.transform, false);
        Rigidbody rb = inHandItem.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.isKinematic = true;
        }
    }

    // get set item
    public void setInHandItem(int itemId, (ProductObj, float) jarProductInfo) {
        clearInHandItem();
        inHandItem = Instantiate(holdablePrefabItems[findInPrefabs(itemId)], Vector3.zero, new Quaternion(0f, -0.90f, 0f, 1));
        // if object is jar
        if (itemId == 3) {
            // set contents to original
            inHandItem.GetComponent<Jar>().setProduct(jarProductInfo.Item1, jarProductInfo.Item2);
        }
        inHandItem.transform.SetParent(pickUpParent.transform, false);
        Rigidbody rb = inHandItem.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.isKinematic = true;
        }
    }

    private void clearInHandItem() {
        foreach(Transform child in pickUpParent.transform) {
            Destroy(child.gameObject);
        }
    }

    public int findInPrefabs(int itemId) {
        for (int i = 0; i < holdablePrefabItems.Length; i++) {
            if (holdablePrefabItems[i].GetComponent<Item>().itemData.obj.id == itemId) {
                return i;
            }
        }
        return -1;
    }
    
}
