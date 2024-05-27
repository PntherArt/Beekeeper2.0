using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIInventory : MonoBehaviour
{
    public List<UIItem> uiItems = new List<UIItem>();
    public GameObject slotPrefab;
    public Transform slotPanel;
    public int numberOfSlots = 3;

    private void Awake() {
        for (int i = 0; i < numberOfSlots; i++) {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotPanel);
            uiItems.Add(instance.GetComponentInChildren<UIItem>());
        }
    }

    /*public void Start() {
        for (int i = 0; i < numberOfSlots; i++) {
            uiItems.Add(new UIItem());
        }
        Debug.Log("UI INVENTORY: " + uiItems.Count);
    }*/

    public void UpdateSlot(int slot, InventoryItem item) {
        //Debug.Log("Slot: " + slot);
        uiItems[slot].UpdateItem(item);
    }

    public void AddNewItem(int slot, InventoryItem item, int amountToAdd) {
        //Debug.Log("UI ADD ITEM: " + item);
        //Debug.Log("UI items: " + uiItems[1]);
        //UpdateSlot(uiItems.FindIndex(i => i.item == null), item);
        // /uiItems[slot].item = item;
        UpdateSlot(slot, item);
        UpdateSlotText(slot, amountToAdd);
    }

    public void RemoveItem(int slot) {
        //Debug.Log("UI REMOVE ITEM: " + item);
        UpdateSlot(slot, null);
        UpdateSlotText(slot, 0);
    }

    public void SetActiveSlot(int slotNumber) {
        for (int i = 0; i < uiItems.Count; i++) {
            uiItems[i].UnsetActiveSlot();
        }
        uiItems[slotNumber].SetActiveSlot();
    }

    public void UpdateSlotText(int slotNumber, int numItems) {
        uiItems[slotNumber].UpdateSlotText(numItems);
    }
}
