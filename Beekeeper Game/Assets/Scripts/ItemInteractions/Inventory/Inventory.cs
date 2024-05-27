using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Transform pickUpParent;

    public List<InventorySlotCount> inventoryItems = new List<InventorySlotCount>();
    public int numberOfSlots = 3;
    //// TODO: add and use number of slots global var?
    //public InventorySlotCount[] inventoryItems = new InventorySlotCount[3];
    public UIInventory inventoryUI;
    
    public int activeSlot = 0;

    // set oninteracted, on start hover, on end hover, interactKey(if different)

    private void Awake() {
        // set slots of the list to empty InventorySlotCount class
        for (int i = 0; i < numberOfSlots; i++) {
            inventoryItems.Add(new InventorySlotCount(null, 0));
        }
    }

    public void Start() {
        inventoryUI.SetActiveSlot(activeSlot);
    }

    public void Update() {
        //placeDownObject();
    }

    public void SetActiveSlot(int newActiveSlot)
    {
        activeSlot = newActiveSlot;
        // modify UIInventory
        inventoryUI.SetActiveSlot(activeSlot);
    }

    public InventoryItem GetActiveSlotItem()
    {
        return inventoryItems[activeSlot].item;
    }

    public int GetActiveSlotItemId()
    {
        if (!inventoryItems[activeSlot].isEmpty()) {
            return inventoryItems[activeSlot].item.obj.id;
        }
        return -1;
    }

    public bool IsActiveSlotEmpty() {
        return inventoryItems[activeSlot].isEmpty();
    }

    public int AddToInventory(InventoryItem item, int amountToAdd)
    {
        if (inventoryItems.Count <= numberOfSlots)
        {
            bool hasItem = false;
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].item == item)
                {
                    int numItems = inventoryItems[i].AddStackAmount(amountToAdd);
                    inventoryUI.UpdateSlotText(i, numItems);
                    // TODO: UI stack increase function
                    hasItem = true;
                    return i;
                }
            }
            // if item doesn't exist in inventory
            if(!hasItem) {
                int addedSlotIndex = addToEmptySlot(item, amountToAdd);
                if (addedSlotIndex != -1) {
                    // to UI as well
                    inventoryUI.AddNewItem(addedSlotIndex, item, amountToAdd);
                    return addedSlotIndex;
                }
                // TODO: add inventory full UI message
                return -1;
                
            }
        }
        // if reached here, slots full
        return -1;

    }

    private int addToEmptySlot(InventoryItem newItem, int amountToAdd) {
        for (int i = 0; i < inventoryItems.Count; i++) {
            if (inventoryItems[i].isEmpty()) {
                inventoryItems[i].item = newItem;
                inventoryItems[i].stackAmount = amountToAdd;
                //Debug.Log("ADDED: " + inventoryItems[i].item.displayName);
                return i;
            }
        }
        return -1; // no slot left in inventory, could not add
    }

    /*public InventoryItem CheckInventoryHas(int id) {
        return inventoryItems.Find(itemSlot => itemSlot.item.id == id);
    }*/

    public int RemoveFromInventory(InventoryItem item, int amountToRemove) {
        for (int i = 0; i < inventoryItems.Count; i++) {
            if (inventoryItems[i].item == item) {
                int numItems = inventoryItems[i].RemoveFromStack(amountToRemove);
                inventoryUI.UpdateSlotText(i, numItems);
                //Debug.Log("Removed item: " + item.displayName);
                if (inventoryItems[i].stackAmount == 0)
                {
                    // remove from inventoryItems
                    inventoryItems[i].EmptySlot();
                    inventoryUI.RemoveItem(i);
                    return -1;
                }
                return i;
            }
        }
        return -1;
    }

    public void setJarProductInfo(int slot, ProductObj product, float amount) {
        inventoryItems[slot].jarProductInfo = (product, amount);
    }
}

[System.Serializable]
public class InventorySlotCount
{
    public InventoryItem item;
    public int stackAmount;
    public InventorySlotCount(InventoryItem _item, int _stackAmount)
    {
        item = _item;
        stackAmount = _stackAmount;
    }

    public (ProductObj, float) jarProductInfo = (null, 0);

    public int AddStackAmount(int value)
    {
        stackAmount += value;
        return stackAmount;
    }

    public int RemoveFromStack(int value)
    {
        if (value >= stackAmount)
        {
            stackAmount = 0;
        }
        else
        {
            stackAmount -= value;
        }
        return stackAmount;
    }

    public bool isEmpty() {
        return item == null && stackAmount == 0;
    }

    public void EmptySlot() {
        item = null;
        stackAmount = 0;
    }
}
