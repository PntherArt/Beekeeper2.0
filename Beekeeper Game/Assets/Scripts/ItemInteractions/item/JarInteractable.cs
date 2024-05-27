using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class JarInteractable : Interactable
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
            return true;
        }
        return false;
         //TODO: change depending
        // GameObject inHandItem = player.GetComponent<PlayerRaycast>().inHandItem;
        // Item item = null;
        // if (inHandItem != null)
        //     item = inHandItem.GetComponent<Item>();

        // return inHandItem != null && item != null && item.itemData.obj.id == 3;
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