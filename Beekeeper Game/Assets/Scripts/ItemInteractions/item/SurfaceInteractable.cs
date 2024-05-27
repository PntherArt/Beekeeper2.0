using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceInteractable : Interactable
{
    public GameObject player;
    public Inventory inventory;
    
    // Start is called before the first frame update
    void Start()
    {
        // get player
        // Note: couldn't just drag scene's player onto prefab (because Unity forbids it?)
        player = GameObject.Find("Player");
        //interactKey = KeyCode.Mouse1;
        inventory = player.GetComponent<Inventory>();

        onInteracted.AddListener(pickUp);
        onStartHover.AddListener(highlightObj);
        onEndHover.AddListener(unhighlightObj);
    }

    public override bool playerInInteractableCondition()
    {
        if (player != null) {
            
            if (player.GetComponent<Inventory>().IsActiveSlotEmpty()) { // if hand empty, can pickup table
                Debug.Log("TABLE INTERACTABLE: " + player.GetComponent<Inventory>().activeSlot);
                return true;
            } else { // if item in hand, cannot pick up table
                Debug.Log("TABLE NOT INTERACTABLE: " + player.GetComponent<Inventory>().activeSlot);
                return false;
            }
        }
        return false;
    }

    public void highlightObj() {
        GetComponent<Highlight>()?.highlightObject();
    }

    public void unhighlightObj() {
        GetComponent<Highlight>()?.unhighlightObject();
    }

    public void pickUp() {
        if (!player.GetComponent<PlayerRaycast>().isInventoryFull()) {
            InventoryItem item = GetComponent<Item>().itemData;

            // deactivate box collider so objects can be placed properly when moving
            GetComponent<BoxCollider>().enabled = false;

            // Add to inventory
            int modifiedItemSlot = inventory.AddToInventory(item, 1);
            //Set inventory active slot
            if (modifiedItemSlot != -1)
            {
                inventory.SetActiveSlot(modifiedItemSlot);
            }

            // set in hand item
            player.GetComponent<PlayerRaycast>().setInitialInHandItem(gameObject);
        }
    }
}
