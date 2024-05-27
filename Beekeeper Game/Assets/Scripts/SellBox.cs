using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellBox : Interactable
{
    public GameObject player;
    public GlobalVariables globalVars;
    public AlertManager alertManager;

    public Animator anim;

    private void Start()
    {
        /*if (interactPopup != null)
        {
            onStartHover.AddListener(() =>
            {
                interactPopup.GetComponent<ButtonIndicatorUI>().open();
            }
            );

            onEndHover.AddListener(() =>
            {
                interactPopup.GetComponent<ButtonIndicatorUI>().close();
            }
            );
        }*/
        onStartHover.AddListener(openBox);
        onEndHover.AddListener(closeBox);
        onInteracted.AddListener(sellJarContents);
        
    }

    public void sellJarContents()
    {
        GameObject inHandItem = player.GetComponent<PlayerRaycast>().inHandItem;
        Jar jar = inHandItem.GetComponent<Jar>();
        (ProductObj, float) extracted = jar.extractProduct();
        if (extracted.Item1 == null || extracted.Item2 == 0)
        {
            alertManager.queueAlert("Jar is empty!");
        }
        else
        {
            int moneyMade = Mathf.FloorToInt(extracted.Item1.sellValue * extracted.Item2);
            globalVars.changeMoney(moneyMade);
            alertManager.queueAlert("Sold " + Mathf.Round(extracted.Item2 * 100f) * 0.01f + " of " + extracted.Item1.objectName + " for $" + moneyMade);
        }
    }

    void openBox()
    {
        anim.Play("Open");
    }

    void closeBox()
    {
        anim.Play("Close");
    }

    public override bool playerInInteractableCondition()
    {
        GameObject inHandItem = player.GetComponent<PlayerRaycast>().inHandItem;
        Item item = null;
        if (inHandItem != null)
            item = inHandItem.GetComponent<Item>();

        return inHandItem != null && item != null && item.itemData.obj.id == 3;
    }
}
