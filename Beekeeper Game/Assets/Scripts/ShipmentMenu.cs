using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipmentMenu : ToggleMenu
{
    public CatalogContent cart;
    public DayCycle dayCycle;
    public AlertManager alertManager;

    ShipmentContent shipmentContent;

    private void Start()
    {
        anim = GetComponent<Animator>();
        shipmentContent = GetComponentInChildren<ShipmentContent>();
        dayCycle.hourlyEvents[dayCycle.nightEndHour].AddListener(deliverShipments);
    }

    public void deliverShipments()
    {
        Dictionary<CatalogObject, int> cartDict = cart.storage;
        int amt = cartDict.Count;
        foreach (CatalogObject boughtObj in cartDict.Keys)
        {
            for (int i = 0; i < cartDict[boughtObj]; i++)
                shipmentContent.addItem(boughtObj);
        }
        cart.storage.Clear();
        cart.emptyCatalog();
        cart.updateCatalog();
        if (amt > 0)
            alertManager.queueAlert("You've got mail!");
    }
}
